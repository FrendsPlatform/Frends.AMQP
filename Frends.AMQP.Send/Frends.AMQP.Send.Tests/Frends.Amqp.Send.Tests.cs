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

    [Test]
    public async Task TestWithFileCertificate_ShouldReturnError()
    {
        var options = new Options
        {
            Timeout = 5,
            LinkName = Guid.NewGuid().ToString(),
            ThrowErrorOnFailure = false
        };

        var input = new Input
        {
            Message = new AmqpMessage { BodyAsString = "Hello File Cert!" },
            QueueOrTopicName = queue,
            ApplicationProperties = new[] { new ApplicationProperty { Name = "key", Value = "val" } },
            MessageProperties = new AmqpProperties { MessageId = Guid.NewGuid().ToString() }
        };

        var connection = new Connection
        {
            BusUri = SecureBusAddress,
            ClientCertificate = SearchCertificateBy.File,
            CertificateFilePath = "nonexistent.pfx",
            CertificatePassword = "",
            DisableServerCertificateValidation = true
        };

        var result = await AMQP.Send(input, options, connection);
        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error.Message, Does.Contain("Could not find"));
    }

    [Test]
    public async Task TestWithIssuerCertificate_ShouldReturnError()
    {
        var options = new Options
        {
            Timeout = 5,
            LinkName = Guid.NewGuid().ToString(),
            ThrowErrorOnFailure = false
        };

        var input = new Input
        {
            Message = new AmqpMessage { BodyAsString = "Hello Issuer Cert!" },
            QueueOrTopicName = queue,
            ApplicationProperties = Array.Empty<ApplicationProperty>(),
            MessageProperties = new AmqpProperties { MessageId = Guid.NewGuid().ToString() }
        };

        var connection = new Connection
        {
            BusUri = SecureBusAddress,
            ClientCertificate = SearchCertificateBy.Issuer,
            CertificateIssuer = "NonExistentIssuer",
            DisableServerCertificateValidation = true
        };

        var result = await AMQP.Send(input, options, connection);
        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.Not.Null);
    }

    [Test]
    public async Task TestWithInvalidApplicationProperties_ShouldReturnError()
    {
        var options = new Options
        {
            Timeout = 5,
            LinkName = Guid.NewGuid().ToString(),
            ThrowErrorOnFailure = false
        };

        var input = new Input
        {
            Message = new AmqpMessage { BodyAsString = "Invalid props" },
            QueueOrTopicName = queue,
            ApplicationProperties = null,
            MessageProperties = new AmqpProperties { MessageId = Guid.NewGuid().ToString() }
        };

        var connection = new Connection
        {
            BusUri = SecureBusAddress,
            ClientCertificate = SearchCertificateBy.None,
            DisableServerCertificateValidation = true
        };

        var result = await AMQP.Send(input, options, connection);
        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.Message, Does.Contain("Error in message properties").Or.Contain("Object reference"));
    }
}