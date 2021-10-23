# Azure Storage - Router

This example shows how to implement a **_Router_** to distribute messages to different queues, based on message criteria.

A **_Producer_** sends messages to a single _Queue_ that has a single **_Consumer_** that behaves as a **_"Router"_**. The Router distributes messages to different worker queues based on message attributes. For each worker Queue, there can be one or more **_"workers"_** that process the messages on their respective queues.

![router-design](https://user-images.githubusercontent.com/33935506/138537520-65253078-56f8-463c-bff8-486d17c2532a.png)

---

## Getting Started

Please see [Azure Storage Queue - Getting Started] guide where I explain how to get started with Azure storage using both an emulator (Azurite) and the actual Azure Storage Service.

## Configure Connection String

```powershell
# configure connection string to use local emulator

# change to consumer project folder
cd .\StorageQueue.Router.Consumer\

dotnet user-secrets set "ConnectionStrings:StorageQueue" "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"

# open new console window
# change to producer project folder
cd .\StorageQueue.Router.Producer\

dotnet user-secrets set "ConnectionStrings:StorageQueue" "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"

# open new console window
# change to router project folder
cd .\StorageQueue.Router\

dotnet user-secrets set "ConnectionStrings:StorageQueue" "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"
```

---

## Run App

### Start Router

```powershell
# change to router project folder
cd .\StorageQueue.Router\

# run router project
dotnet run
```

### Start 'game-reviews' Consumer

```powershell
# open new console window
# change to consumer project folder
cd .\StorageQueue.Router.Consumer\

# run consumer project
dotnet run

# enter queue name: game-reviews
```

### Start 'book-reviews' Consumer

```powershell
# open new console window
# change to consumer project folder
cd .\StorageQueue.Router.Consumer\

# run consumer project
dotnet run

# enter queue name: book-reviews
```

### Start 'music-reviews' Consumer

```powershell
# open new console window
# change to consumer project folder
cd .\StorageQueue.Router.Consumer\

# run consumer project
dotnet run

# enter queue name: music-reviews
```

### Start 'movie-reviews' Consumer

```powershell
# open new console window
# change to consumer project folder
cd .\StorageQueue.Router.Consumer\

# run consumer project
dotnet run

# enter queue name: movie-reviews
```

### Start Producer

```powershell
# open new console window
# change to producer project folder
cd .\StorageQueue.Router.Producer\

dotnet run
```

---

## Screenshots

![router-running-1](https://user-images.githubusercontent.com/33935506/138540787-10fafda8-0a80-404b-b7ac-d95e1328b3b5.png)

![router-running-2](https://user-images.githubusercontent.com/33935506/138540789-7bb496dd-d55e-44c0-80fc-6db6f513f09f.png)

![router-running-3](https://user-images.githubusercontent.com/33935506/138540791-e66b997e-c298-469c-821c-264f05f50105.png)

---

[Azure Storage Queue - Getting Started]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/storage/README.md