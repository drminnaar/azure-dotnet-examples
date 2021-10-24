# CosmosDB - Getting Started

A basic example that demonstrates some of the common operations used to interact with CosmosDB.

The following operations are demonstrated:

- Create Database If Not Exists
- Create Container If Not Exists
- Create Reviews
- Get Reviews
- Get Game Reviews Having Rating Greater Than 7
- Get Book Reviews Having Rating Greater Than 5
- Delete Reviews
- Delete Container
- Delete Database

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
cd .\CosmosDb.GettingStarted\

# run project
dotnet run
```

---

## Screenshots

![cosmos-getting-started-1](https://user-images.githubusercontent.com/33935506/138578224-478fc390-9dce-4d47-851d-4439e27b75a0.png)

![cosmos-getting-started-2](https://user-images.githubusercontent.com/33935506/138578227-911539b2-e0b5-497d-a415-939880e664b1.png)

---
