# Azure Storage - One Way Messaging

This example shows how to implement **_One Way Messaging_** through the use of a single **_Producer_** and a single **_Consumer_**.  In this scenario, a **_Producer_** sends a Message to a Queue. A **_Consumer_** of the Queue receives the Message and processes it.

![storage-queue-direct-message](https://user-images.githubusercontent.com/33935506/137874634-ba398ca6-f26c-45ff-9c42-d9259f422c62.png)

- Producer sends Message to Queue
- Consumer subscribes to Queue
- Consumer receives and processes Message
- Consumer removes Message from Queue

---

## Getting Started

Please see [Azure Storage Queue - Getting Started] guide where I explain how to get started with Azure storage using both an emulator (Azurite) and the actual Azure Storage Service.

## Configure Connection String

```powershell
# configure connection string to use local emulator

dotnet user-secrets set "ConnectionStrings:StorageQueue" "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"
```

## Run App

```powershell
# change to consumer project folder
cd .\StorageQueue.OneWayMessaging.Consumer\

# run project
dotnet run

# open new console window
# change to producer project folder
cd .\StorageQueue.OneWayMessaging.Producer\
```

**Storage Queue Emulator:**

![storage-queue-emulator](https://user-images.githubusercontent.com/33935506/137874613-5f35aeab-42d1-4ebb-b2a6-79a8643c03f9.png)

**Storage Queue Producer/Consumer Terminals:**

![storage-queue-direct-message-console](https://user-images.githubusercontent.com/33935506/137874622-8904022a-81a1-4d16-82f9-fe6c3438aa7f.png)
