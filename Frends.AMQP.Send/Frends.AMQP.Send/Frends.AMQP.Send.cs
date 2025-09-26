using System;
using Amqp;
using Amqp.Framing;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Amqp.Sasl;
using System.Linq;
using System.ComponentModel;
using Frends.AMQP.Send.Definitions;
using ConnectionAmqp = Amqp.Connection;
using Connection = Frends.AMQP.Send.Definitions.Connection;
using Frends.AMQP.Send.Helpers;

namespace Frends.AMQP.Send;

/// <summary>
/// AMQP send task.
/// </summary>
public static class AMQP
{
    /// <summary>
    /// Task for sending Advanced Message Queuing Protocol (= AMQP) messages.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.AMQP.Send)
    /// </summary>
    /// <param name="input">Defines how to connect AMQP queue and message being sent.</param>
    /// <param name="options">Defines additional properties of connection.</param>
    /// <param name="connection">Defines additional properties of message.</param>
    /// <returns>Object { bool Success }</returns>
    public static async Task<Result> Send([PropertyTab] Input input, [PropertyTab] Options options, [PropertyTab] Connection connection)
    {
        try
        {
            var conn = await CreateConnection(connection.BusUri, connection.ClientCertificate, connection.DisableServerCertificateValidation, connection.CertificateIssuer, connection.CertificateFilePath, connection.CertificatePassword);
            var session = new Session(conn);
            var sender = new SenderLink(session, options.LinkName, input.QueueOrTopicName);

            try
            {
                await sender.SendAsync(CreateMessage(input), new TimeSpan(0, 0, 0, options.Timeout));
            }
            finally
            {
                await sender.CloseAsync();
                await session.CloseAsync();
                await conn.CloseAsync();
            }

            return new Result
            {
                Success = true,
            };
        }

        catch (Exception ex)
        {
            return ErrorHandler.Handle(ex, options);
        }
    }

    private static Message CreateMessage(Input input)
    {
        var message = new Message(input.Message.BodyAsString)
        {
            Properties = new Properties(),
            ApplicationProperties = new ApplicationProperties()
        };

        try
        {
            foreach (var applicationProperty in input.ApplicationProperties)
                message.ApplicationProperties.Map.Add(applicationProperty.Name, applicationProperty.Value);

            message.Properties.MessageId = input.MessageProperties.MessageId;
            message.Properties.AbsoluteExpiryTime = input.MessageProperties.AbsoluteExpiryTime ?? DateTime.MaxValue;
            message.Properties.ContentEncoding = input.MessageProperties.ContentEncoding;
            message.Properties.ContentType = input.MessageProperties.ContentType;
            message.Properties.CorrelationId = input.MessageProperties.CorrelationId;
            message.Properties.CreationTime = input.MessageProperties.CreationTime ?? DateTime.UtcNow;
            message.Properties.GroupId = input.MessageProperties.GroupId;
            message.Properties.GroupSequence = input.MessageProperties.GroupSequence;
            message.Properties.ReplyToGroupId = input.MessageProperties.ReplyToGroupId;
            message.Properties.ReplyTo = input.MessageProperties.ReplyTo;
            message.Properties.Subject = input.MessageProperties.Subject;
            message.Properties.UserId = input.MessageProperties.UserId;
            message.Properties.To = input.MessageProperties.To;
        }
        catch
        {
            throw new Exception("Error in message properties.");
        }
        return message;
    }

    private static X509Certificate2 FindCertificateByCn(string issuedBy)
    {
        var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadOnly);

        var certificate = store.Certificates.Cast<X509Certificate2>().FirstOrDefault(c => c.Issuer.Contains($"CN={issuedBy}"));

        store.Close();

        return certificate;
    }

    private static X509Certificate2 FindCertificateByFile(string pfxFilePath, string pfxPassword)
    {
        var certificate = new X509Certificate2(System.IO.File.ReadAllBytes(pfxFilePath)
            , pfxPassword
            , X509KeyStorageFlags.MachineKeySet |
              X509KeyStorageFlags.PersistKeySet |
              X509KeyStorageFlags.Exportable);
        return certificate;
    }

    static readonly RemoteCertificateValidationCallback noneCertValidator = (a, b, c, d) => true;
    private static async Task<ConnectionAmqp> CreateConnection(string busUri, SearchCertificateBy searchClientCertificateBy, bool disableServerCertValidation, string issuer, string pfxFilePath, string pfxPassword)
    {
        var factory = new ConnectionFactory();
        var brokerAddress = new Address(busUri);

        if (searchClientCertificateBy == SearchCertificateBy.None)
        {
            if (disableServerCertValidation)
                factory.SSL.RemoteCertificateValidationCallback = noneCertValidator;
            return await factory.CreateAsync(brokerAddress);
        }
        else
        {
            if (disableServerCertValidation)
                factory.SSL.RemoteCertificateValidationCallback = noneCertValidator;

            X509Certificate2 certificate;
            if (searchClientCertificateBy == SearchCertificateBy.File)
                certificate = FindCertificateByFile(pfxFilePath, pfxPassword);
            else if (searchClientCertificateBy == SearchCertificateBy.Issuer)
                certificate = FindCertificateByCn(issuer);
            else
                throw new ArgumentException("You should not be here!");

            if (certificate == null)
                throw new ArgumentException($"Could not find certificate");

            var expireDate = DateTime.Parse(certificate.GetExpirationDateString());

            if (expireDate < DateTime.Now)
                throw new Exception($"Certificate has already expired: '{expireDate}'");

            factory.SSL.ClientCertificates.Add(certificate);
            factory.SASL.Profile = SaslProfile.External;

            var conn = await factory.CreateAsync(brokerAddress);

            return conn;
        }
    }
}