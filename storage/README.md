# Azure Storage

---

## Getting Started

There are 2 options to get started using Azure Storage:

1. [Azure Storage Emulator](#1-azure-storage-emulator)
1. [Azure Storage Service](#2-azure-storage-service)

---

## 1. Azure Storage Emulator

It is possible to development and test your code with *__Azure Storage Queue__* by using an emulator called **_[Azurite]_**. This allows one to familiarize oneself with Azure Storage without having to have an Azure account.

**_[Azurite]_** is a storage emulator that can be used to emulate Azure storage services in development.

Find more information at the following links:

- [Use the Azurite emulator for local Azure Storage development](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio)
- [Run automated tests by using Azurite](https://docs.microsoft.com/en-us/azure/storage/blobs/use-azurite-to-run-automated-tests)
- [Azurite Github]
- [Azurite on DockerHub]

### Start Emulator Using Docker

If you already have [Docker] installed, this option is quite possibly the easiest way to get started with the [Azurite] emulator.

To get started, try any of the following options depending on your OS (operating system).

#### Start Emulator Using Docker For Windows

```powershell
# using powershell, create directory called 'azurite' in your home directory (~)
mkdir ~/azurite

# get azurite docker image
docker pull mcr.microsoft.com/azure-storage/azurite

# start azurite docker container to emulate azure storage
docker run --detach `
    -p 10000:10000 `
    -p 10001:10001 `
    -p 10002:10002 `
    -v ~/azurite:/data mcr.microsoft.com/azure-storage/azurite

# check status of container
docker container ls
```

#### Start Emulator Using Docker For Linux and Mac

```bash
# create directory called 'azurite' in your home directory (~)
mkdir ~/azurite

# get azurite docker image
docker pull mcr.microsoft.com/azure-storage/azurite

# start azurite docker container to emulate azure storage
docker run --detach \
    -p 10000:10000 \
    -p 10001:10001 \
    -p 10002:10002 \
    -v ~/azurite:/data mcr.microsoft.com/azure-storage/azurite

# check status of container
docker container ls
```

### Using Emulator With `Azure CLI`

**Configure ConnectionString:**

Working with the emulator using the `Azure CLI` is as simple as specifying a connection string when executing storage commands. For example:

```bash
az storage queue list --connection-string "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"
```

It can be tedious to continuously add the connection string parameter. Therefore, as an alternative, one can specify a value for the environment variable **_'AZURE_STORAGE_CONNECTION_STRING'_***

```bash
# using bash

AZURE_STORAGE_CONNECTION_STRING="AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"
export AZURE_STORAGE_CONNECTION_STRING

# exclude --connection-string argument
az storage queue list
```

```bash
# using powershell

$env:AZURE_STORAGE_CONNECTION_STRING = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"

# exclude --connection-string argument
az storage queue list
```

### Using Emulator With `Azure Powershell`

**Assign "local context (emulator)" to Azure Storage Context:**

```powershell
# using powershell

# Get the local storage context
$SA_CONTEXT = New-AzStorageContext -Local

# Show SA_CONTEXT contents
echo $SA_CONTEXT

# output
StorageAccountName  : devstoreaccount1
BlobEndPoint        : http://127.0.0.1:10000/devstoreaccount1
TableEndPoint       : http://127.0.0.1:10002/devstoreaccount1
QueueEndPoint       : http://127.0.0.1:10001/devstoreaccount1
FileEndPoint        :
Context             : Microsoft.WindowsAzure.Commands.Storage.AzureStorageContext
Name                :
StorageAccount      : UseDevelopmentStorage=true
TableStorageAccount : UseDevelopmentStorage=true
Track2OauthToken    :
EndPointSuffix      :
ConnectionString    : UseDevelopmentStorage=true
ExtendedProperties  : {}
```

**List Queues Using Powershell:**

```powershell
Get-AzStorageQueue -Context $SA_CONTEXT
```

---

## 2. Azure Storage Service

An [Azure Storage Account] is required to work with any of the Azure Storage Services like [Azure Storage Queue].

### Create Azure Storage Account (ASA)

[Azure Storage Queue] is one of the [Azure Storage] core services. Therefore, before [Azure  Storage Queue] can be used, an [Azure Storage Account] must be created. See '[create Azure Storage Account]' for more information.

<table>
<tr>
<th>Azure CLI</th>
<th>Azure Powershell</th>
</tr>
<tr>
<td>

```bash
# List available locations
az account list-locations \
    --query "[].{Region:name}" \
    --out table

# Create resource group
az group create \
    --name <storage-resource-group> \
    --location <location>

# Create general purpose v2 storage account with Local Redundant Store (LRS) storage
az storage account create \
  --name <account-name> \
  --resource-group <storage-resource-group> \
  --location <location> \
  --sku Standard_LRS \
  --kind StorageV2

# List storage accounts
az storage account list
```

</td>
<td>

```powershell
# List available locations
Get-AzLocation | select Location

# Create resource group
$resourceGroup = "<resource-group>"
$location = "<location>"
New-AzResourceGroup -Name $resourceGroup -Location $location

# Create general purpose v2 storage account with Local Redundant Store (LRS) storage
New-AzStorageAccount -ResourceGroupName $resourceGroup `
  -Name <account-name> `
  -Location $location `
  -SkuName Standard_LRS `
  -Kind StorageV2
```

</td>
</tr>
</table>

### Manage Azure Storage Queue using `Azure CLI`

**Get Azure Storage Account Connection String:**

We can use the Azure Storage Account connection string to connect to queue. Type the following command to obtain the connection string:

```bash
# declare variables
SA_NAME=<storage_account_name>
SA_RESOURCE_GROUP_NAME=<storage_account_resource-group_name>

# get connection string for storage account
 az storage account show-connection-string \
    --name $SA_NAME \
    --resource-group $SA_RESOURCE_GROUP_NAME

# extract and assign connection string value to a variable
SA_CONNECTION_STRING=$(az storage account show-connection-string --name $SA_NAME --resource-group $SA_RESOURCE_GROUP_NAME --output tsv)
```

**List Queues:**

```bash
az storage queue list --connection-string $SA_CONNECTION_STRING
```

It can be tedious to continuously add the connection string parameter. Therefore, as an alternative, one can specify a value for the environment variable **_'AZURE_STORAGE_CONNECTION_STRING'_***

```bash
AZURE_STORAGE_CONNECTION_STRING=$SA_CONNECTION_STRING
export AZURE_STORAGE_CONNECTION_STRING

az storage queue list
```

---

### Manage Azure Storage Queue using `Azure Powershell`

**Assign Azure Storage Context:**

```powershell
# Get storage context for storage account
$SA_CONTEXT = (Get-AzStorageAccount -AccountName 'your_account_name' -ResourceGroupName 'your_group_name').Context

# Show $SA_CONTEXT content
echo $SA_CONTEXT
```

**List Queues:**

```powershell
Get-AzStorageQueue -Context $SA_CONTEXT
```

---

[Azure Cloud Shell]: https://shell.azure.com
[Azure CLI]: https://docs.microsoft.com/en-gb/cli/azure/install-azure-cli
[Azure Storage]: https://docs.microsoft.com/en-us/azure/storage/
[Azure Storage Queue]: https://docs.microsoft.com/en-us/azure/storage/queues/
[Azure Storage Account]: https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?toc=/azure/storage/blobs/toc.json
[create Azure Storage Account]: https://docs.microsoft.com/en-us/azure/storage/common/storage-account-overview?toc=/azure/storage/blobs/toc.json
[Azurite Github]: https://github.com/Azure/Azurite
[Azurite on DockerHub]: https://hub.docker.com/_/microsoft-azure-storage-azurite
[Azurite]: https://github.com/Azure/Azurite
[Azure Powershell]: https://github.com/Azure/azure-powershell
[Azure Storage Emulator Using Azure CLI]: /storage-queue/docs/azure-storage-emulator-using-cli.md
[Powershell]: /storage-queue/docs/azure-storage-emulator-using-powershell.md
[Azure Storage Emulator Using Powershell]: /storage-queue/docs/azure-storage-emulator-using-powershell.md
[Azure Storage Emulator]: /storage-queue/docs/azure-storage-emulator.md