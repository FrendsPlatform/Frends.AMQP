using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Frends.AMQP.Send.Definitions;

/// <summary>
/// Options parameters.
/// </summary>
public class Options
{
    /// <summary>
    /// Timeout in seconds for receiving or sending Message to the queue.
    /// </summary>
    /// <example>30</example>
    [DefaultValue(30)]
    public int Timeout { get; set; }

    /// <summary>
    /// Link name.
    /// </summary>
    /// <example>9e2646fe-bbc7-482d-a5dc-679dc5caf951</example>
    [DefaultValue("{{Guid.NewGuid().ToString()}}")]
    [DisplayFormat(DataFormatString = "Text")]
    public string LinkName { get; set; }

    /// <summary>
    /// Select whether certificate is used and where it can be found.
    /// </summary>
    /// <example>DontUseCertificate</example>
    [DefaultValue(SearchCertificateBy.DontUseCertificate)]
    public SearchCertificateBy SearchClientCertificateBy { get; set; }

    /// <summary>
    /// Issuer of certificate.
    /// </summary>
    /// <example>Issuer</example>
    [UIHint(nameof(SearchClientCertificateBy), "", SearchCertificateBy.Issuer)]
    [DisplayFormat(DataFormatString = "Text")]
    public string Issuer { get; set; }

    /// <summary>
    /// Path where .pfx (certificate) file can be found.
    /// </summary>
    /// <example>c:\Windows\foo.pfx</example>
    [UIHint(nameof(SearchClientCertificateBy), "", SearchCertificateBy.File)]
    [DisplayFormat(DataFormatString = "Text")]
    public string PfxFilePath { get; set; }

    /// <summary>
    /// Password for the certificate.
    /// </summary>
    /// <example>123</example>
    [UIHint(nameof(SearchClientCertificateBy), "", SearchCertificateBy.File)]
    [DisplayFormat(DataFormatString = "Text")]
    public string PfxPassword { get; set; }

    /// <summary>
    /// Disable server certificate validation when TLS is used. False means certificate is validated. If connection is not secured with tls this option does not do anything.
    /// </summary>
    /// <example>false</example>
    [DefaultValue(false)]
    public bool DisableServerCertValidation { get; set; }
}