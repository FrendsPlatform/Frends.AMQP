using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Frends.Amqp.Definitions;

namespace Frends.Amqp.Tests
{
    [TestFixture]
    class TestClass
    {
        /// These tests here use a test AMQP server.
        /// https://github.com/Azure/amqpnetlite/tree/master/test/TestAmqpBroker
        /// Start AMQP server before executing test by (ensure that you use same addresses in command and in tests):
        /// .\TestAmqpBroker.exe amqp://localhost:5676 amqps://localhost:5677 /creds:guest:guest /cert:localhost
        /// If you have valid tls cert you may want to enable cert validation.
        /// 
        /// If you want to test this against a real ActiveMQ server, Docker is recommended:
        /// docker pull rmohr/activemq
        /// docker run -p 5672:5672 -p 8161:8161 rmohr/activemq

        private static readonly string insecureBusAddress = "amqp://guest:guest@localhost:5676";
        private static readonly string secureBusAddress = "amqps://guest:guest@localhost:5677";
        private static readonly string queue = "q1";
        private static readonly bool disableServerCertValidation = true;

        private static readonly AmqpProperties properties = new AmqpProperties
        {
            MessageId = Guid.NewGuid().ToString()
        };

        private static readonly AmqpMessage testMessage = new AmqpMessage()
        {
            BodyAsString = "Hello AMQP!",
        };

        static readonly AmqpMessageProperties amqpMessageProperties = new AmqpMessageProperties
        {
            ApplicationProperties = new ApplicationProperty[] { },
            Properties = properties
        };

        static InputSender inputSender = new InputSender
        {
            Message = testMessage,
            QueueOrTopicName = queue,
        };

        static Options optionsDontUseClientCert = new Options
        {
            Timeout = 15,
            LinkName = Guid.NewGuid().ToString(),
            SearchClientCertificateBy = SearchCertificateBy.DontUseCertificate,
            DisableServerCertValidation = disableServerCertValidation
        };

        [Test]
        public async Task TestInsecure()
        {
            inputSender.BusUri = insecureBusAddress;
            var ret = await Send.AmqpSend(inputSender, optionsDontUseClientCert, amqpMessageProperties, new System.Threading.CancellationToken());
            Assert.NotNull(ret);
            Assert.That(ret.Success, Is.True);
        }

        [Test]
        public async Task TestWithSecureConnection()
        {
            inputSender.BusUri = secureBusAddress;

            var ret = await Send.AmqpSend(inputSender, optionsDontUseClientCert, amqpMessageProperties, new System.Threading.CancellationToken());

            Assert.NotNull(ret);
            Assert.That(ret.Success, Is.True);
        }
    }
}