---
page_type: sample
languages:
- C#
products:
- Azure
- Azure Storage
- Aszure Storage Table
description: "Demonstrates how to use Azure.Data.Tables SDK"
---

# Azure Storage Queue - Getting Started

A simple example demonstrating how to use Azure.Data.Tables SDK.

The following concepts are demonstrated:

- Create table
- Add entities
- Query entities
- Delete table

---

## Getting Started

Please see [Azure Storage Queue - Getting Started] guide where I explain how to get started with Azure storage using both an emulator (Azurite) and the actual Azure Storage Service.

## Configure Connection String

```powershell
# configure connection string to use local emulator

dotnet user-secrets set "ConnectionStrings:WeatherData" "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"
```

## Run App

```powershell
# change to project folder
cd .\TableStorage.GettingStarted\

# run project
dotnet run
```

---

[Azure Storage Queue - Getting Started]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/storage/README.md
