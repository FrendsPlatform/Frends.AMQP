# Frends.Amqp

[Frends](https://frends.com/) task for sending Advanced Message Queuing Protocol (= AMQP) messages.

[Installation](#installation)

[Parameters](#Parameters)

[License](License)

[Building](#Building)

[Testing](#Testing)

[Contributing](#Contributing)

[Change Log](#Change-Log)


# Installation
You can install a nuget package of Frends.Amqp task via FRENDS UI Task View. Read Building-topic below for instructions on how to get the nuget package.


Parameters
=====

### Input


| Property          | Type                      | Description          | Example    |
|-------------------|---------------------------|----------------------|----------------------|
| Body as string | Text | Message body. | `Hello there!` |
| Bus uri     | Text                    | The URI for the AMQP message bus. Username and key must be url encoded.       |`amqp://guest:guest@localhost:5672` `amqps://user:password@somewhere:1234`|
| Queue or topic name | Text                    | Name of target queue or topic.        |`somequeue`|



### Options


| Property          | Type                      | Description          | Example    |
|-------------------|---------------------------|----------------------|----------------------|
| Timeout        | Integer        | Timeout in seconds for receiving or sending message to the queue. |`30`|
| Link name   | Text                    | Name that uniquely identifies the link. |`d7e1d24b-4c13-451d-aaf9-8fdcbd1ac607` `VeryUniqueName123`|
| Search client certificate by | Choice | Select whether certificate is used and where it can be found. |`DontUserCertificate` or `Issuer` or `File`|
| Disable server cert validation | Yes/No               | Enable or disable server certificate validation. |`Yes` or `No`|



### Message properties

For descriptions and more information about the properties below, see [AMQP 1.0 specification](https://www.amqp.org/sites/amqp.org/files/amqp.pdf).


| Property          | Type                      |
|-------------------|---------------------------|
| Application properties | Array of key/value pairs |
| Message ID | Text |
| Absolute expiry time | Expression: `DateTime?` |
| Content encoding | Text |
| Content type | Text |
| Correlation ID | Text |
| Creation time | Expression: `DateTime?` |
| Group ID | Text |
| Group sequence | Integer |
| Reply to group ID | Text |
| Reply to | Text |
| Subject | Text |
| User ID | Array of byte |
| To | Text |

# License

This project is licensed under the MIT License - see the LICENSE file for details.

# Building

1. Clone a copy of the repo

```bash
git clone https://github.com/CommunityHiQ/Frends.Amqp.git
```

2. Restore dependencies

```bash
dotnet restore
```

3. Rebuild the project

```bash
dotnet build
```

4. Create a nuget package

```bash
dotnet pack Frends.Amqp
```

# Testing

Tests automate running and stopping TestAmqpBroker, which is used to mock an ActiveMQ server. [TestAmqpBroker is part of azure-amqp repository](https://github.com/Azure/azure-amqp/tree/master/test/TestAmqpBroker), but it is included in this repository as well as a zip file.

# Contributing

When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repo on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes

# Change Log

| Version             | Changes                 |
| ---------------------| ---------------------|
| 1.0.0 | Initial version of AMQP Tasks. |
