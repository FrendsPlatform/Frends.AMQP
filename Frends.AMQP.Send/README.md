# Frends.AMQP.Send

![Frends.AMQP.Send Main](https://github.com/FrendsPlatform/Frends.AMQP/actions/workflows/Send_main.yml/badge.svg)
![MyGet](https://img.shields.io/myget/frends-tasks/v/Frends.AMQP.Send?label=NuGet)
![GitHub](https://img.shields.io/github/license/FrendsPlatform/Frends.AMQP?label=License)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Frends.AMQP/Frends.AMQP.Send|main)

[Frends](https://frends.com) task for sending Advanced Message Queuing Protocol (= AMQP) messages.

## Installing

You can install the Task via frends UI Task View or you can find the NuGet package from the following NuGet feed
https://www.myget.org/F/frends-tasks/api/v2.

## Task Parameters


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

# Building

1. Clone a copy of the repo

```bash
git clone https://github.com/CommunityHiQ/Frends.AMQP.git
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
dotnet pack
```

# Testing

## Unit Tests

Unit tests are run on each push and can be run manually by `dotnet test` command.

## Integration Tests

Integration tests in Fare run as part of unit test runs.

## Performance Tests

No performance tests are done as the AMQP server itself is the main component during execution.

# Contributing

When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repo on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes
