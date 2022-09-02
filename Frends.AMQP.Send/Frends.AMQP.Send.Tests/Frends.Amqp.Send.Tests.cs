using System;
using System.Threading;
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
            SearchClientCertificateBy = SearchCertificateBy.DontUseCertificate,
            DisableServerCertValidation = true
        };

        var sender = new Input
        {
            Message = new AmqpMessage { BodyAsString = "Hello AMQP!" },
            QueueOrTopicName = queue,
            BusUri = SecureBusAddress
        };

        var amqpMessageProperties = new AmqpMessageProperties
        {
            ApplicationProperties = new[] { new ApplicationProperty { Name = "test", Value = "value" } },
            Properties = new AmqpProperties { MessageId = Guid.NewGuid().ToString() }
        };

        var ret = await AMQP.Send(sender, optionsDontUseClientCert, amqpMessageProperties);
        Assert.NotNull(ret);
        Assert.That(ret.Success, Is.True);
    }
}