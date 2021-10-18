# Azure Storage Queue

The `storage-queue` folder is comprised of a number of examples that demonstrate how to use `Azure Queue Storage`.

For more information on ~Azure Storage Queue`, please see the following links:

- [What is Azure Queue Storage](https://docs.microsoft.com/en-us/azure/storage/queues/storage-queues-introduction)
- [Get started with Azure Queue Storage using .NET](https://docs.microsoft.com/en-us/azure/storage/queues/storage-dotnet-how-to-use-queues?tabs=dotnet)
- [Naming Queues](https://docs.microsoft.com/en-us/rest/api/storageservices/Naming-Queues-and-Metadata)
- [Azure Storage Queue Pricing](https://azure.microsoft.com/en-us/pricing/details/storage/queues/)
- [Official Microsoft Azure Storage Examples](https://docs.microsoft.com/en-us/samples/browse/?expanded=azure&products=azure-storage%2Cazure-storage-accounts&languages=azurecli%2Ccsharp%2Cpowershell)
- [Azure Queue Storage Tutorial - Adam Marczak](https://youtu.be/JQ6KhjU5Zsg)

---

## Getting Started

There are 2 options to get started using Azure Storage Queue:

1. [Azure Storage Emulator](#1-azure-storage-emulator)
1. [Azure Storage Service](#2-azure-storage-service)

---

## 1. Azure Storage Emulator

It is possible to development and test your code with *__Azure Storage Queue__* by using an emulator called **_[Azurite]_**. This allows you to familiarise yourself with Azure Storage Queue without having to have an Azure account.

[Azurite] is a storage emulator that can be used to emulate Azure storage services in development.

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

### Using Emulator With Azure CLI

**Configure ConnectionString:**

Working with the emulator using the Azure CLI is as simple as specifying a connection string when executing storage commands. For example:

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

**Create Queue Azure Using CLI:**

```bash
SA_QUEUE_NAME=<storage_account_queue_name>
az storage queue create --name $SA_QUEUE_NAME --timeout 10
```

**List Queues Using Azure CLI:**

```bash
az storage queue list
```

**Delete Queue Using Azure ClI:**

```bash
az storage queue delete --name $SA_QUEUE_NAME
```

### Using Emulator With Azure Powershell

**Assign "local context (emulator)" to Azure Storage Context:**

```powershell
# Get the local storage context
$SA_CONTEXT = New-AzStorageContext -Local

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

**Create Queue Using Powershell:**

```powershell
$SA_QUEUE_NAME = "<storage_account_queue_name>"
$queue = New-AzStorageQueue –Name $SA_QUEUE_NAME -Context $SA_CONTEXT
```

**List Queues Using Powershell:**

```powershell
Get-AzStorageQueue -Context $SA_CONTEXT
```

**Get Queue by name:**

```powershell
Get-AzStorageQueue -Name $SA_QUEUE_NAME -Context $SA_CONTEXT
```

**Send messages:**

```powershell
# Create a new football event message
$queueMessage = [Microsoft.Azure.Storage.Queue.CloudQueueMessage]::new("Football event")

# Add message to the queue
$queue.CloudQueue.AddMessageAsync($QueueMessage)

# Create a new golf event message
$queueMessage = [Microsoft.Azure.Storage.Queue.CloudQueueMessage]::new("Golf event")

# Add message to the queue
$queue.CloudQueue.AddMessageAsync($QueueMessage)

# Create a new rugby event message
$queueMessage = [Microsoft.Azure.Storage.Queue.CloudQueueMessage]::new("Rugby event")

# Add message to the queue
$queue.CloudQueue.AddMessageAsync($QueueMessage)
```

**Read and Delete Messages:**

```powershell
# Set the amount of time you want to entry to be invisible after read from the queue
# If it is not deleted by the end of this time, it will show up in the queue again
$invisibleTimeout = [System.TimeSpan]::FromSeconds(10)

# Read the message from the queue, then show the contents of the message. Read the other two messages, too.
$queueMessage = $queue.CloudQueue.GetMessageAsync($invisibleTimeout,$null,$null)
$queueMessage.Result

AsBytes         : {70, 111, 111, 116…}
Id              : d7335880-f8ce-49b8-a3ff-ce79ecdf2938
PopReceipt      : MTRBdWcyMDIxMDU6NDg6NTFkODgw
InsertionTime   : 8/14/2021 5:42:53 AM +00:00
ExpirationTime  : 8/21/2021 5:42:53 AM +00:00
NextVisibleTime : 8/14/2021 5:49:01 AM +00:00
AsString        : Football event
DequeueCount    : 2

# After 10 seconds, these messages reappear on the queue.
# Read them again, but delete each one after reading it.
# Delete the message.
$queueMessage = $queue.CloudQueue.GetMessageAsync($invisibleTimeout, $null, $null)
$queueMessage.Result
$queue.CloudQueue.DeleteMessageAsync($queueMessage.Result.Id, $queueMessage.Result.popReceipt)
```

**Delete Queue:**

```powershell
Remove-AzStorageQueue –Name $SA_QUEUE_NAME –Context $SA_CONTEXT
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

### Manage Azure Storage Queue using Azure CLI

**Get ASA Connection String:**

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

**Create Azure Storage Queue:**

```bash
SA_QUEUE_NAME=<storage_account_queue_name>
az storage queue create --name $SA_QUEUE_NAME --timeout 10
```

**Delete Queue:**

```bash
SA_QUEUE_NAME=<storage_account_queue_name>
az storage queue delete --name $SA_QUEUE_NAME
```

---

### Manage Azure Storage Queue using Azure Powershell

**Assign Azure Storage Context:**

```powershell
# Get storage context for storage account
$SA_CONTEXT = (Get-AzStorageAccount -AccountName 'your_account_name' -ResourceGroupName 'your_group_name').Context

# Show $SA_CONTEXT content
echo $SA_CONTEXT
```

**Create Queue:**

```powershell
$SA_QUEUE_NAME = "<storage_account_queue_name>"
$queue = New-AzStorageQueue –Name $SA_QUEUE_NAME -Context $SA_CONTEXT
```

**List Queues:**

```powershell
Get-AzStorageQueue -Context $SA_CONTEXT
```

**Get Queue by name:**

```powershell
Get-AzStorageQueue -Name $SA_QUEUE_NAME -Context $SA_CONTEXT
```

**Send messages:**

```powershell
# Create a new football event message
$queueMessage = [Microsoft.Azure.Storage.Queue.CloudQueueMessage]::new("Football event")

# Add message to the queue
$queue.CloudQueue.AddMessageAsync($QueueMessage)

# Create a new golf event message
$queueMessage = [Microsoft.Azure.Storage.Queue.CloudQueueMessage]::new("Golf event")

# Add message to the queue
$queue.CloudQueue.AddMessageAsync($QueueMessage)

# Create a new rugby event message
$queueMessage = [Microsoft.Azure.Storage.Queue.CloudQueueMessage]::new("Rugby event")

# Add message to the queue
$queue.CloudQueue.AddMessageAsync($QueueMessage)
```

**Read and Delete Messages:**

```powershell
# Set the amount of time you want to entry to be invisible after read from the queue
# If it is not deleted by the end of this time, it will show up in the queue again
$invisibleTimeout = [System.TimeSpan]::FromSeconds(10)

# Read the message from the queue, then show the contents of the message. Read the other two messages, too.
$queueMessage = $queue.CloudQueue.GetMessageAsync($invisibleTimeout,$null,$null)
$queueMessage.Result

# output
AsBytes         : {70, 111, 111, 116…}
Id              : d7335880-f8ce-49b8-a3ff-ce79ecdf2938
PopReceipt      : MTRBdWcyMDIxMDU6NDg6NTFkODgw
InsertionTime   : 8/14/2021 5:42:53 AM +00:00
ExpirationTime  : 8/21/2021 5:42:53 AM +00:00
NextVisibleTime : 8/14/2021 5:49:01 AM +00:00
AsString        : message
DequeueCount    : 1

# After 10 seconds, these messages reappear on the queue.
# Read them again, but delete each one after reading it.
# Delete the message.
$queueMessage = $queue.CloudQueue.GetMessageAsync($invisibleTimeout, $null, $null)
$queueMessage.Result
$queue.CloudQueue.DeleteMessageAsync($queueMessage.Result.Id, $queueMessage.Result.popReceipt)
```

**Delete Queue:**

```powershell
Remove-AzStorageQueue –Name $queueName –Context $context
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