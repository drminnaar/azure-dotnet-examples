![az-examples](https://user-images.githubusercontent.com/33935506/139220514-ee957e1b-b7f9-41d6-9824-eb70b8450d61.png)

A catalog of examples showing how to develop solutions using _Microsoft Azure_ and _.NET 6_

## Contents

- **Azure Active Directory**

  - [Wonderland Weather .NET 6 Web API]

    An example that shows how to protect a .NET 6 Web API using Azure AD

  - [Wonderland Weather React App]

    An example that shows how to protect a React application using Azure AD and MSAL (Microsoft Authentication Library)

  - [Wonderland Weather Full App] (React App + .NET 6 API)

    An example application that shows how to protect both a React application and .NET 6 Web API using Azure AD. It also shows how to make authenticated requests from a React application to a .NET 6 Web API

- **[Azure Storage]**
  
  Please see the [Azure Storage] README to see how to use Azure storage emulator in development environment

  - **[Azure Storage Queue]**

    Please see the [Azure Storage Queue] README to see how to use Azure Storage Queue via Azure CLI and Azure Powershell

    - [Azure Storage Queue - Getting Started]

      A simple example demonstrating some of the most commonly used features of Azure Storage Queue.

    - [Azure Storage Queue - One Way Messaging]

      This example shows how to implement **_One Way Messaging_** through the use of a single **_Producer_** and a single **_Consumer_**.  In this scenario, a **_Producer_** sends a Message to a Queue. A **_Consumer_** of the Queue receives the Message and processes it.

    - [Azure Storage Queue - Router]

      This example shows how to implement a **_Router_** to distribute messages to different queues, based on message criteria.
  
  - **Azure Storage Blob**

    - [Azure Storage Blob - Getting Started]

      Demonstrates how to use Azure.Storage.Blobs SDK

  - **Azure Storage Table**

    - [Azure Storage Table - Getting Started]

      Demonstrates how to use Azure.Data.Tables SDK

    - [Azure Storage Table - CRUD Web API]

      A .NET 6 API that demonstrates how to use Azure.Data.Tables SDK

- **Azure CosmosDB**

  - [CosmosDB Getting Started]

    A basic example that demonstrates some of the common operations used to interact with CosmosDB.

  - [CosmosDB CRUD API]

    A .NET 6 Web API that exposes CRUD endpoints to interact with CosmosDB

- **Azure Service Bus**

  - [Azure Service Bus - One Way Messaging]

    A basic example that demonstrates how to create a producer that sends messages to a service bus queue, where a consumer receives the message from the queue and processes the message.
  
  - [Azure Service Bus - Pub/Sub]

    Demonstrates how to configure topics for publish/subscribe

  - [Azure Service Bus - Pub/Sub With Rules]

    Demonstrates how to configure topics for publish/subscribe using rules like Correlation Rule Filter and SQL Rule Filter

---

[Wonderland Weather .NET 6 Web API]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeatherApi/README.md
[Wonderland Weather React App]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeatherApp/app/README.md
[Wonderland Weather Full App]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeather/README.md
[Azure Storage]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/storage/README.md
[Azure Storage Queue]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/storage/storage-queue/README.md
[Azure Storage Queue - getting started]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/storage/storage-queue/GettingStarted/README.md
[Azure Storage Queue - One Way Messaging]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/storage/storage-queue/OneWayMessaging/README.md
[Azure Storage Queue - Router]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/storage/storage-queue/Router/README.md
[CosmosDB Getting Started]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/cosmosdb/GettingStarted/README.md
[Azure Service Bus - One Way Messaging]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/service-bus/OneWayMessaging/README.md
[Azure Service Bus - Pub/Sub]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/service-bus/PubSub/README.md
[Azure Service Bus - Pub/Sub With Rules]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/service-bus/PubSubWithRules/README.md
[Azure Storage Blob - Getting Started]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/storage/storage-container/GettingStarted/README.md
[Azure Storage Table - Getting Started]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/storage/table-storage/GettingStarted/README.md
[Azure Storage Table - CRUD Web API]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/storage/table-storage/Crud/README.md