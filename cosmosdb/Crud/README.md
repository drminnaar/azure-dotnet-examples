---
page_type: sample
languages:
- C#
products:
- Azure
- Azure CosmosDB
description: "Demonstrates how to build an API with a CosmosDB database"
---

# CosmosDB - CRUD Example

A .NET 6 Web API that exposes CRUD endpoints to interact with CosmosDB

The following operations are demonstrated:

- Create database
- Create container
- Create game
- Update game
- Get game by id
- Get list of games
- Delete game

---

## Getting Started

There are 2 primary ways to run this demo application and they are listed as follows:

- [Azure Cosmos DB free tier](https://docs.microsoft.com/en-us/azure/cosmos-db/free-tier)
- [Install and use the Azure Cosmos DB Emulator for local development and testing](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=ssl-netstd21)

## Configure Connection String

Find the emulator connection details at <https://localhost:8081/_explorer/index.html>

```powershell
# configure connection string to use local emulator
# The following initialization demonstrates how to set
# connection string using the cosmosdb emulator connection string

dotnet user-secrets set "ConnectionStrings:CosmosDb" "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
```

## Run App

```powershell
# change to project folder
cd .\CosmosDb.CrudApi\

# run project
dotnet run
```

---
