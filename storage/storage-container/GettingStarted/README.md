---
page_type: sample
languages:
- C#
products:
- Azure
- Azure Storage
- Aszure Storage Blobs
description: "Demonstrates how to use Azure.Storage.Blobs SDK"
---

# Azure Storage Queue - Getting Started

A simple example demonstrating how to use Azure.Storage.Blobs SDK.

The following concepts are demonstrated:

- Create container
- Upload file to container
- Retrieve file from container
- Download a file from container
- Delete file from container
- Delete container

---

## Getting Started

Please see [Azure Storage Queue - Getting Started] guide where I explain how to get started with Azure storage using both an emulator (Azurite) and the actual Azure Storage Service.

## Configure Connection String

```powershell
# configure connection string to use local emulator

dotnet user-secrets set "ConnectionStrings:StorageAccount" "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"
```

## Run App

```powershell
# change to project folder
cd .\StorageContainer.GettingStarted\

# run project
dotnet run
```

![storage-blob-1](https://user-images.githubusercontent.com/33935506/142062351-37d21cfb-c355-4ba4-a3b9-cc83a5db24af.png)

---

[Azure Storage Queue - Getting Started]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/storage/README.md
