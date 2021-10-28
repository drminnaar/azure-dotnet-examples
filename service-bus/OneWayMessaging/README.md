---
page_type: sample
languages:
- C#
products:
- Azure
- Service Bus
description: "An implementation of OneWay Messaging using Azure Service Bus"
---

# Azure Service Bus - One Way Messaging

---

## Description

This is a basic example that demonstrates how to create a producer that sends messages to a service bus queue, where a consumer receives the message from the queue and processes the message.

![sb-onewaymessaging-1](https://user-images.githubusercontent.com/33935506/139213118-734311c3-3172-458a-ba16-28709f5b890d.png)

The following topics are covered in this examples:

- How to create a Service Bus Namespace using Azure CLI and Azure Powershell
- How to create a Service Bus Queue using Azure CLI and Azure Powershell
- How to obtain a Service Bus connection string using Azure CLI and Azure Powershell
- Create a Producer to send messages to Service Bus
- Create a Consumer to receive messages from Service Bus

---

## Flow

- A Producer (Console App) generates reviews (game, movie, book, music) and sends reviews as messages to a Service Bus queue
- A Consumer (Console App) receives reviews, and processes reviews

---

## Service Bus Setup

Before running the example, you are required to create the following setup for Azure Service Bus:

- [Create Service Bus Namespace](#create-service-bus-namespace)
- [Get Service Bus Namespace ConnectionString](#get-service-bus-namespace-connectionstring)
- [Create Service Bus Queue (optional)](#create-service-bus-queue)

### Create Service Bus Namespace

Create a new Service Bus Namespace using Standard Tier.

<table>
<tr>
<td>Azure CLI</td>
<td>Azure Powershell</td>
</tr>
<tr>
<td>

```bash
SB_NAMESPACE_NAME=sb-examples-dev
SB_RESOURCE_GROUP_NAME=sandbox
SB_LOCATION=australiaeast

az servicebus namespace create \
    --name $SB_NAMESPACE_NAME \
    --resource-group $SB_RESOURCE_GROUP_NAME \
    --location $SB_LOCATION \
    --sku Standard \
    --output table
```

</td>
<td>

```powershell
$SB_NAMESPACE_NAME='sb-examples-dev'
$SB_RESOURCE_GROUP_NAME='sandbox'
$SB_LOCATION='australiaeast'

New-AzServiceBusNamespace `
    -Name $SB_NAMESPACE_NAME `
    -ResourceGroupName $SB_RESOURCE_GROUP_NAME `
    -Location $SB_LOCATION `
    -SkuName Standard
```

</td>
</tr>
</table>

### Get Service Bus Namespace ConnectionString

<table>
<tr>
<td>Azure CLI</td>
<td>Azure Powershell</td>
</tr>
<tr>
<td>

```bash
SB_NAMESPACE_NAME='sb-examples-dev'
SB_RESOURCE_GROUP_NAME='sandbox'

SB_CONNECTION_STRING=$(az servicebus namespace authorization-rule keys list \
    --resource-group $SB_RESOURCE_GROUP_NAME \
    --namespace-name $SB_NAMESPACE_NAME \
    --name RootManageSharedAccessKey \
    --query primaryConnectionString \
    --out tsv)

echo $SB_CONNECTION_STRING
```

</td>
<td>

```powershell
$SB_NAMESPACE_NAME='sb-examples-dev'
$SB_RESOURCE_GROUP_NAME='sandbox'

$SB_CONNECTION_STRING = (Get-AzServiceBusKey `
    -ResourceGroupName $SB_RESOURCE_GROUP_NAME `
    -Namespace $SB_NAMESPACE_NAME `
    -Name RootManageSharedAccessKey `
    | Select-Object -ExpandProperty PrimaryConnectionString)

echo $SB_CONNECTION_STRING
```

</td>
</tr>
</table>

### Create Service Bus Queue

This step is optional as the code in the example will automatically create queue if it does not exist.

<table>
<tr>
<td>Azure CLI</td>
<td>Azure Powershell</td>
</tr>
<tr>
<td>

```bash
SB_QUEUE_NAME=reviews
SB_NAMESPACE_NAME=sb-examples-dev
SB_RESOURCE_GROUP_NAME=sandbox

az servicebus queue create \
    --name $SB_QUEUE_NAME \
    --resource-group $SB_RESOURCE_GROUP_NAME \
    --namespace-name $SB_NAMESPACE_NAME
```

</td>
<td>

```bash
$SB_QUEUE_NAME='reviews'
$SB_NAMESPACE_NAME='sb-examples-dev'
$SB_RESOURCE_GROUP_NAME='sandbox'

New-AzServiceBusQueue `
    -Name $SB_QUEUE_NAME `
    -ResourceGroupName $SB_RESOURCE_GROUP_NAME `
    -NamespaceName $SB_NAMESPACE_NAME
```

</td>
</tr>
</table>

---

## Project Setup

### Configure Consumer

<table>
<tr>
<td>BASH</td>
<td>Powershell</td>
</tr>
<tr>
<td>

```bash
# change to Consumer directory
cd ./ServiceBus.OneWayMessaging.Consumer/

# add user secrets
SERVICE_BUS_CONNECTION_STRING=''
dotnet user-secrets set "ConnectionStrings:ServiceBus" $SERVICE_BUS_CONNECTION_STRING
```

</td>
<td>

```powershell
# change to Consumer directory
cd .\ServiceBus.OneWayMessaging.Consumer\

# add user secrets
$SERVICE_BUS_CONNECTION_STRING=''
dotnet user-secrets set "ConnectionStrings:ServiceBus" $SERVICE_BUS_CONNECTION_STRING
```

</td>
</tr>
</table>

### Configure Producer

<table>
<tr>
<td>BASH</td>
<td>Powershell</td>
</tr>
<tr>
<td>

```bash
# change to Producer directory
cd ./ServiceBus.OneWayMessaging.Producer/

# add user secrets
SERVICE_BUS_CONNECTION_STRING=''
dotnet user-secrets set "ConnectionStrings:ServiceBus" $SERVICE_BUS_CONNECTION_STRING
```

</td>
<td>

```powershell
# change to Producer directory
cd .\ServiceBus.OneWayMessaging.Producer\

# add user secrets
$SERVICE_BUS_CONNECTION_STRING=''
dotnet user-secrets set "ConnectionStrings:ServiceBus" $SERVICE_BUS_CONNECTION_STRING
```

</td>
</tr>
</table>

---

## Run Demo

```bash
```bash
# change to Consumer directory
cd ./ServiceBus.OneWayMessaging.Consumer/
dotnet run

# open new console window
# change to Producer directory
cd ./ServiceBus.OneWayMessaging.Producer/
dotnet run
```

---

## Screenshots

![sb-onewaymessaging-2](https://user-images.githubusercontent.com/33935506/139215712-dbab920b-67e2-478e-9600-2d82963530e9.png)

---