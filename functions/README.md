# Azure Functions

## Examples

### Product Manager
  
This project demonstrates how to develop an Azure Function application that can be used to manage a catalog of products. There are multiple versions of the same project where each new version increases in functionality and complexity.

The goal of this project is to introduce Azure Function concepts by slowly introducing them in different project versions. The following Azure Function concepts will be demonstrated:

- Azure triggers (http, blob, servicebus)
- Azure bindings (blob)
- In-process and Isolated Process
- Dependency Injection
- Local development
- Infrastructure as Code (Bicep, Azure CLI, Azure Powershell)

The available versions are as follows:

  - [Version 1]
  
    Create inital *Product Manager Function App* having the following functionality:

    - Add *'ProductsFunction'* function (HTTP Triggered) to serve as products API.

  - [Version 2]
    
    Extend *Product Manager Version 1* with the following changes:

    - Add depedency injection
    - Use in-memory database provided by *Entity Framework*
    - Modify *'ProductsFunction'* function to use dependency injection and in-memory database
    - Add *'SeedFunction'* function to help seed the products database

  - [Version 3]
    
    Extend *Product Manager Version 2* with the following changes:

    - Modify the *'ProductsFunction'* function to include product images
    - Add *Azure Storage* integration to store product images
    - Add *'user secrets'* to store connection information

  - [Version 4]
    
    Extend *Product Manager Version 3* with the following changes:

    - Add *'ProductImageResizerFunction'* function that uses a *'Blob Trigger'* to react to newly uploaded product images by resizing product images to 3 sizes (xs, sm, md) and uploading them to an Azure Storage Container.
    - The *'ProductImageResizerFunction'* function also saves new product image data to a relevant product in the database.
    - Update 'ProductsFunction' function to accomodate additional product image information.

  - [Version 5]
    
    Extend *Product Manager Version 4* with the following changes:

    - Use Cosmos Db as primary database
    - Add *CosmosDbService* as a convenient wrapper of CosmosClient
    - Create *ProductService* to handle all product management
    - Create *ProductImageService* to handle all product image requirements
    - Introduce hashing to produce product entity id
    - Create *ProductFaker (uses Bogus library)* to seed Cosmos Db database

---

## Configure Your Environment

Before you get started, make sure you have the following requirements in place:

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Azure Functions Core Tools version 4.x](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#install-the-azure-functions-core-tools)
- [Visual Studio Code] on one of the [supported platforms](https://code.visualstudio.com/docs/supporting/requirements#_platforms)
- [C# extension for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
- [Azure Functions extension for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)
- [Azure Storage Emulator]. [See my guide](https://github.com/drminnaar/azure-dotnet-examples/blob/main/storage/README.md) on getting started with Azure Storage.

You also need an Azure account with an active subscription. [Create an account for free](https://azure.microsoft.com/free)

---

## Getting Started

### Configure Local Azure Storage

All Azure Function examples require access to Azure Storage. For development, the [Azurite] emulator is recommended by Microsoft for local Azure Storage development.

According to the [official Microsoft documentation](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite):

> The Azurite open-source emulator provides a free local environment for testing your Azure blob, queue storage, and table storage applications.

Find more information about Azurite by visiting the following links:

- [Use the Azurite emulator for local Azure Storage development](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite)
- [Official Azure Azurite Github Repository](https://github.com/Azure/Azurite)

I recommend using Azurite via the [NPM approach](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=npm#install-azurite) as follows:

```bash
# Install Azurite

npm install -g azurite

```

```bash
# Run Azurite

azurite --silent --location ~/azurite --debug ~/azurite/debug.log
```

```bash
# Get Help
azurite -h
azurite --help
```

### Access Azure Storage

#### Azure Storage Explorer

[Azure Storage Explorer] allows one to upload, download, and manage Azure Storage blobs, files, queues, and tables, as well as Azure Data Lake Storage entities and Azure managed disks.

[Download the official tool offering from microsoft called Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer)

![](https://azurecomcdn.azureedge.net/cvt-0879e029a7a5b54a9507be2df02436498bec59b543ab57d2433e0c67a2a12d0a/images/page/features/storage-explorer/value-prop1.png)

#### Azure Tools Visual Studio Code Extension Pack

Alternatively, if you have not done so already, install the [Azure Tools](https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-node-azure-pack) extension pack for Visual Studio Code.

![](https://github.com/microsoft/vscode-node-azure-pack/raw/HEAD/explorer.png)

### Open Workspace

#### Product Manager

```bash
# open product manager workspace folder
cd azure-dotnet-examples/functions/product-manager

# open product manager workspace
code ./product-manager.code-workspace
```

---

[Visual Studio Code]: https://code.visualstudio.com/
[Azure Storage Emulator]: https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator
[Azure Storage Explorer]: https://azure.microsoft.com/en-us/features/storage-explorer
[Version 1]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/functions/product-manager/product-manager-v1/README.md
[Version 2]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/functions/product-manager/product-manager-v2/README.md
[Version 3]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/functions/product-manager/product-manager-v3/README.md
[Version 4]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/functions/product-manager/product-manager-v4/README.md
[Version 5]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/functions/product-manager/product-manager-v5/README.md