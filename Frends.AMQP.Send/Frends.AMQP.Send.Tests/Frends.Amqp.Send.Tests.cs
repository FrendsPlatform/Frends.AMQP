using System;
using System.Threading.Tasks;
using Frends.AMQP.Send.Definitions;
using NUnit.Framework;

namespace Frends.AMQP.Send.Tests;

[TestFixture]
public class AMQPSendTests
{
    private static readonly string queue = "task-tests";
    private static string SecureBusAddress;

    [SetUp]
    public void SetUp()
    {
        var externalAmqpsConnectionString = Environment.GetEnvironmentVariable("AMQPS_CONN_STR");

        if (string.IsNullOrWhiteSpace(externalAmqpsConnectionString))
        {
            TestAmqpBrokerHelper.KillTestBroker();
            TestAmqpBrokerHelper.EnsureTestBrokerIsRunning();
            SecureBusAddress = $"amqps://guest:guest@localhost:{TestAmqpBrokerHelper.TestAmqpBrokerPorts[1]}";
        }
        else
            SecureBusAddress = externalAmqpsConnectionString;
    }

    [TearDown]
    public void TearDown()
    {
        TestAmqpBrokerHelper.KillTestBroker();
    }

    [Test]
    public async Task TestWithSecureConnection()
    {
        var optionsDontUseClientCert = new Options
        {
            Timeout = 15,
            LinkName = Guid.NewGuid().ToString(),
        };

        var sender = new Input
        {
            Message = new AmqpMessage { BodyAsString = "Hello AMQP!" },
            QueueOrTopicName = queue,
            ApplicationProperties = new[] { new ApplicationProperty { Name = "test", Value = "value" } },
            MessageProperties = new AmqpProperties { MessageId = Guid.NewGuid().ToString() }
        };

        var amqpMessageProperties = new Connection
        {
            BusUri = SecureBusAddress,
            ClientCertificate = SearchCertificateBy.None,
            DisableServerCertificateValidation = true
        };

        var ret = await AMQP.Send(sender, optionsDontUseClientCert, amqpMessageProperties);
        Assert.NotNull(ret);
        Assert.That(ret.Success, Is.True);
    }
}