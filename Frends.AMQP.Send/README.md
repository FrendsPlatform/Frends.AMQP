# Frends.AMQP.Send
Frends task for sending Advanced Message Queuing Protocol (= AMQP) messages.

![Frends.AMQP.Send Main](https://github.com/FrendsPlatform/Frends.AMQP/actions/workflows/Send_main.yml/badge.svg)
![MyGet](https://img.shields.io/myget/frends-tasks/v/Frends.AMQP.Send?label=NuGet)
![GitHub](https://img.shields.io/github/license/FrendsPlatform/Frends.AMQP?label=License)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Frends.AMQP/Frends.AMQP.Send|main)

# Installing

You can install the Task via frends UI Task View or you can find the NuGet package from the following NuGet feed https://www.myget.org/F/frends-tasks/api/v2.

## Building


Rebuild the project

`dotnet build`

Run tests

Docker commands if you want to test this against a real ActiveMQ server:
 `docker pull rmohr/activemq`
 `docker run -p 5672:5672 -p 8161:8161 rmohr/activemq`

`dotnet test`


Create a NuGet package

`dotnet pack --configuration Release`