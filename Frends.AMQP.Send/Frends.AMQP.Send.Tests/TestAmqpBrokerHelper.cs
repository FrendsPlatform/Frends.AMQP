using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Frends.AMQP.Send.Tests
{
    internal static class TestAmqpBrokerHelper
    {
        private static Process TestAmqpBrokerProcess;
        internal static int[] TestAmqpBrokerPorts;

        private static string ExtractTestBrokerZip()
        {
            var zipFilePath = Path.Combine(Environment.CurrentDirectory, "TestAmqpBroker.zip");
            var extractedDirPath = Path.Combine(Environment.CurrentDirectory, "TestAmqpBroker");

            if (!File.Exists(zipFilePath))
                throw new FileNotFoundException("Can't find TestAmqpBroker.zip from current directory.");

            if (Directory.Exists(extractedDirPath))
                Directory.Delete(extractedDirPath, true);

            ZipFile.ExtractToDirectory(zipFilePath, extractedDirPath);
            return GetTestBrokerPath();
        }

        private static string GetTestBrokerPath()
        {
            var extractedDirPath = Path.Combine(Environment.CurrentDirectory, "TestAmqpBroker");
            var extractedExeFilePath = Directory.GetFiles(extractedDirPath, "TestAmqpBroker.exe", SearchOption.AllDirectories).FirstOrDefault();
            if (string.IsNullOrEmpty(extractedExeFilePath))
                throw new FileNotFoundException("Can't find TestAmqpBroker.exe.");
            else
                return extractedExeFilePath;
        }

        private static Process RunTestBroker(string testBrokerExePath, string args)
        {
            var process = Process.Start(new ProcessStartInfo(testBrokerExePath) { Arguments = args });
            Console.WriteLine("TestAmqpBroker started.");
            return process;
        }

        internal static void EnsureTestBrokerIsRunning()
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
                    throw new FileNotFoundException("Can't find TestAmqpBroker.zip from current directory.");
                var zipArchive = ZipFile.OpenRead(zipFilePath);
                foreach (var entry in zipArchive.Entries)
                    if (!File.Exists(Path.Combine(extractedDirPath, entry.Name)))
                        entry.ExtractToFile(Path.Combine(extractedDirPath, entry.Name));
            }
            else
                ExtractTestBrokerZip();

            return GetTestBrokerPath();
        }

        internal static void KillTestBroker()
        {
            if (TestAmqpBrokerProcess != null)
            {
                TestAmqpBrokerProcess.Kill(true);
                TestAmqpBrokerProcess = null;
            }

            var processes = Process.GetProcessesByName("TestAmqpBroker.exe");
            if (processes?.Length > 0)
                foreach (var process in processes)
                    process.Kill();
        }

        private static int NextFreeTcpPort()
        {
            TcpListener l = new(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}