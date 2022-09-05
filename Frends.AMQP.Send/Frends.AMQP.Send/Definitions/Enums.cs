namespace Frends.AMQP.Send.Definitions;

/// <summary>
/// SearchCertificateBy options.
/// </summary>
public enum SearchCertificateBy
{
#pragma warning disable CS1591 // self explanatory
    DontUseCertificate,
    Issuer,
    File
#pragma warning restore CS1591 // self explanatory
}