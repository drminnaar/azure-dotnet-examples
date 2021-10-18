# Azure Storage Queue - Getting Started

A simple example demonstrating some of the most commonly used features of Azure Storage Queue.

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
# change to project folder
cd .\StorageQueue.GettingStarted\

# run project
dotnet run
```

![1-create-queue](https://user-images.githubusercontent.com/33935506/137665425-68cc8ba6-9538-42fe-a88b-cdd22e4293ce.png)

![2-send-message-queue](https://user-images.githubusercontent.com/33935506/137665429-b1d1a54d-2b29-4ea7-9ea4-515893f02ce8.png)

![3-receive-messages](https://user-images.githubusercontent.com/33935506/137665430-37cda262-dcd2-4027-bd5d-8951fff37909.png)

---

[Azure Storage Queue - Getting Started]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/storage/README.md
