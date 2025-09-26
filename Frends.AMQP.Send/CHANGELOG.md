# Changelog

## [2.0.0] - 2025-09-24

### Added
- New **Connection** tab with AMQP connection parameters.
- ThrowErrorOnFailure option in **Options** tab with default value `false`
- ErrorMessageOnFailure option in **Options** tab for custom error messages
- Error object in **Result** with `Message` and `AdditionalInfo` properties for detailed error reporting

### Changed
- [Breaking] Moved **Properties** from AmqpMessageProperties tab to Input tab, renamed to **MessageProperties**
- [Breaking] Moved **ApplicationProperties** from AmqpMessageProperties tab to Input tab
- [Breaking] Moved **BusUri** from Input tab to Connection tab
- [Breaking] Moved **SearchClientCertificateBy** from Options tab to Connection tab, renamed to **ClientCertificate**
  - Enum value `DontUseCertificate` renamed to `None`
- [Breaking] Moved **Issuer** from Options tab to Connection tab, renamed to **CertificateIssuer**
- [Breaking] Moved **PfxFilePath** from Options tab to Connection tab, renamed to **CertificateFilePath**
- [Breaking] Moved **PfxPassword** from Options tab to Connection tab, renamed to **CertificatePassword**
- [Breaking] Moved **DisableServerCertValidation** from Options tab to Connection tab, renamed to **DisableServerCertificateValidation**
- Improved error handling with structured Error object in Result

## [1.0.1] - 2022-08-26
### Changed
- Dependencies update
AMQPNetLite Version=2.2.0 to 2.4.4
Newtonsoft.Json Version=12.0.3 to 13.0.1
System.ComponentModel.Annotations 4.6.0 to 5.0.0

## [1.0.0] - 2022-02-17
### Added
- Initial implementation