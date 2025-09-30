using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Frends.AMQP.Send.Definitions
{
    /// <summary>
    /// Connection parameters.
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// The URI for the AMQP Message bus, username and key must be url encoded.
        /// </summary>
        /// <example>amqps://&lt;username&gt;:&lt;key&gt;@&lt;host&gt;:&lt;port&gt;</example>
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("\"amqps://<username>:<key>@<host>:<port>\"")]
        public string BusUri { get; set; }

        /// <summary>
        /// Select whether certificate is used and where it can be found.
        /// </summary>
        /// <example>None</example>
        [DefaultValue(SearchCertificateBy.None)]
        public SearchCertificateBy ClientCertificate { get; set; }

        /// <summary>
        /// Issuer of certificate.
        /// </summary>
        /// <example>Issuer</example>
        [UIHint(nameof(ClientCertificate), "", SearchCertificateBy.Issuer)]
        [DisplayFormat(DataFormatString = "Text")]
        public string CertificateIssuer { get; set; }

        /// <summary>
        /// Path where .pfx (certificate) file can be found.
        /// </summary>
        /// <example>c:\Windows\foo.pfx</example>
        [UIHint(nameof(ClientCertificate), "", SearchCertificateBy.File)]
        [DisplayFormat(DataFormatString = "Text")]
        public string CertificateFilePath { get; set; }

        /// <summary>
        /// Password for the certificate.
        /// </summary>
        /// <example>123</example>
        [PasswordPropertyText(true)]
        [UIHint(nameof(ClientCertificate), "", SearchCertificateBy.File)]
        [DisplayFormat(DataFormatString = "Text")]
        public string CertificatePassword { get; set; }

        /// <summary>
        /// Disable server certificate validation when TLS is used. False means certificate is validated. If connection is not secured with tls this option does not do anything.
        /// </summary>
        /// <example>false</example>
        [DefaultValue(false)]
        public bool DisableServerCertificateValidation { get; set; }
    }
}
