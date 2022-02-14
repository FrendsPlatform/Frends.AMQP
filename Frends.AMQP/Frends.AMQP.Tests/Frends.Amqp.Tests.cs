using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Frends.Amqp.Definitions;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Net;

namespace Frends.Amqp.Tests
{
    [TestFixture]
    class TestClass
    {
        /// These tests here use a test AMQP server. The test server is started and killed automatically.
        /// https://github.com/Azure/amqpnetlite/tree/master/test/TestAmqpBroker
        /// 
        /// If you want to test this against a real ActiveMQ server, Docker is recommended:
        /// docker pull rmohr/activemq
        /// docker run -p 5672:5672 -p 8161:8161 rmohr/activemq

        private static readonly string queue = "q1";
        private static readonly bool disableServerCertValidation = true;

        private static readonly AmqpProperties properties = new()
        {
            MessageId = Guid.NewGuid().ToString()
        };

        private static readonly AmqpMessage testMessage = new()
        {
            BodyAsString = "Hello AMQP!",
        };

        private static Process TestAmqpBrokerProcess;
        private static int[] TestAmqpBrokerPorts;

        private static string InsecureBusAddress => $"amqp://guest:guest@localhost:{TestAmqpBrokerPorts[0]}";
        private static string SecureBusAddress => $"amqps://guest:guest@localhost:{TestAmqpBrokerPorts[1]}";

        static readonly AmqpMessageProperties amqpMessageProperties = new()
        {
            ApplicationProperties = Array.Empty<ApplicationProperty>(),
            Properties = properties
        };

        static readonly InputSender inputSender = new()
        {
            Message = testMessage,
            QueueOrTopicName = queue,
        };

        static readonly Options optionsDontUseClientCert = new()
        {
            Timeout = 15,
            LinkName = Guid.NewGuid().ToString(),
            SearchClientCertificateBy = SearchCertificateBy.DontUseCertificate,
            DisableServerCertValidation = disableServerCertValidation
        };

        /// <summary>
        /// Extracts TestAmqpBroker.zip to current directory and returns full path of TestAmqpBroker.exe file.
        /// </summary>
        /// <exception cref="FileNotFoundException">
        /// Thrown if either TestAmqpBroker.zip is not found, or if
        /// TestAmqpBroker.exe is not found after zip extraction.
        /// </exception>
        private static string ExtractTestBrokerZip()
        {
            var zipFilePath = Path.Combine(Environment.CurrentDirectory, "TestAmqpBroker.zip");
            var extractedDirPath = Path.Combine(Environment.CurrentDirectory, "TestAmqpBroker");

            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException("Can't find TestAmqpBroker.zip from current directory.");
            }

            if (Directory.Exists(extractedDirPath)) {
                Directory.Delete(extractedDirPath, true);
            }

            ZipFile.ExtractToDirectory(zipFilePath, extractedDirPath);

            return GetTestBrokerPath();
        }

        private static string GetTestBrokerPath()
        {
            var extractedDirPath = Path.Combine(Environment.CurrentDirectory, "TestAmqpBroker");
            var extractedExeFilePath = Directory.GetFiles(extractedDirPath, "TestAmqpBroker.exe", SearchOption.AllDirectories).FirstOrDefault();
            if (string.IsNullOrEmpty(extractedExeFilePath))
            {
                throw new FileNotFoundException("Can't find TestAmqpBroker.exe.");
            }
            else
            {
                return extractedExeFilePath;
            }
        }

        private static Process RunTestBroker(string testBrokerExePath, string args)
        {
            var process = Process.Start(new ProcessStartInfo(testBrokerExePath)
            {
                Arguments = args
            });
            Console.WriteLine("TestAmqpBroker started.");
            return process;
        }

        private static void EnsureTestBrokerIsRunning()
        {
            if (TestAmqpBrokerProcess == null) 
            {
                var exePath = EnsureTestBrokerIsExtracted();
                TestAmqpBrokerPorts = new[] { NextFreeTcpPort(), NextFreeTcpPort() };
                TestAmqpBrokerProcess = RunTestBroker(exePath, $"amqp://localhost:{TestAmqpBrokerPorts[0]} amqps://localhost:{TestAmqpBrokerPorts[1]} /creds:guest:guest /cert:localhost");
            }
        }

        private static string EnsureTestBrokerIsExtracted()
        {
            var zipFilePath = Path.Combine(Environment.CurrentDirectory, "TestAmqpBroker.zip");
            var extractedDirPath = Path.Combine(Environment.CurrentDirectory, "TestAmqpBroker");
            if (Directory.Exists(extractedDirPath))
            {
                if (!File.Exists(zipFilePath))
                {
                    throw new FileNotFoundException("Can't find TestAmqpBroker.zip from current directory.");
                }
                var zipArchive = ZipFile.OpenRead(zipFilePath);
                foreach (var entry in zipArchive.Entries)
                {
                    if (!File.Exists(Path.Combine(extractedDirPath, entry.Name)))
                    {
                        entry.ExtractToFile(Path.Combine(extractedDirPath, entry.Name));
                    }
                }
            } else
            {
                ExtractTestBrokerZip();
            }

            return GetTestBrokerPath();
        }

        private static void KillTestBroker()
        {
            if (TestAmqpBrokerProcess != null)
            {
                TestAmqpBrokerProcess.Kill(true);
                TestAmqpBrokerProcess = null;
            }
            
            var processes = Process.GetProcessesByName("TestAmqpBroker.exe");
            if (processes?.Length > 0)
            {
                foreach (var process in processes)
                {
                    process.Kill();
                };
            }
        }

        private static int NextFreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        [Test]
        public void TestInsecure()
        {
            KillTestBroker();
            EnsureTestBrokerIsRunning();

            inputSender.BusUri = InsecureBusAddress;
            var ret = Send.AmqpSend(inputSender, optionsDontUseClientCert, amqpMessageProperties, new System.Threading.CancellationToken()).GetAwaiter().GetResult();
            Assert.NotNull(ret);
            Assert.That(ret.Success, Is.True);

            KillTestBroker();
        }

        [Test]
        public void TestWithSecureConnection()
        {
            KillTestBroker();
            EnsureTestBrokerIsRunning();

            inputSender.BusUri = SecureBusAddress;
            var ret = Send.AmqpSend(inputSender, optionsDontUseClientCert, amqpMessageProperties, new System.Threading.CancellationToken()).GetAwaiter().GetResult();
            Assert.NotNull(ret);
            Assert.That(ret.Success, Is.True);

            KillTestBroker();
        }
    }
}