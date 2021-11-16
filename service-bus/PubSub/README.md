---
page_type: sample
languages:
- C#
products:
- Azure
- Service Bus
description: "Demonstrate how to configure topics for publish/subscribe"
---

# Azure Service Bus - Pub/Sub

---

## Description

This example demonstrates how to configure topics for publish/subscribe.

The following topics are covered in this examples:

- How to create a Service Bus Namespace using Azure CLI and Azure Powershell
- How to create a Service Bus Queue using Azure CLI and Azure Powershell
- How to obtain a Service Bus connection string using Azure CLI and Azure Powershell
- Create a Publisher to send messages to Service Bus topic
- Create 3 Subscribers to receive messages from Service Bus where each subscriber uses a different rule.

---

## Flow

- A Publisher (Console App) generates reviews (game, movie, book, music) and sends reviews as messages to a Service Bus queue.
- 1 Subscriber (Console App) receives all reviews
- 1 Subscriber (Console App) receives all reviews having

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

---

## Project Setup

---

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
cd ./ServiceBus.PubSubWithRules.Subscriber/

# add user secrets
dotnet user-secrets set "ConnectionStrings:ServiceBus" $SB_CONNECTION_STRING
```

</td>
<td>

```powershell
# change to Consumer directory
cd .\ServiceBus.PubSubWithRules.Subscriber\

# add user secrets
dotnet user-secrets set "ConnectionStrings:ServiceBus" $SB_CONNECTION_STRING
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
cd ./ServiceBus.PubSubWithRules.Publisher/

# add user secrets
SERVICE_BUS_CONNECTION_STRING=''
dotnet user-secrets set "ConnectionStrings:ServiceBus" $SERVICE_BUS_CONNECTION_STRING
```

</td>
<td>

```powershell
# change to Producer directory
cd .\ServiceBus.PubSubWithRules.Publisher\

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
# open terminal window/panel
# change to Consumer directory
cd ./ServiceBus.PubSubWithRules.Subscriber/
dotnet run

# open new console window
# change to Publisher directory
cd ./ServiceBus.PubSubWithRules.Publisher/
dotnet run
```

---

## Screenshots

### Create Topics and Subscriptions

![sb-pubsub](https://user-images.githubusercontent.com/33935506/141960155-0a3b85fc-a462-45d0-b712-c0ec543c9402.png)

### Start Processing Messages

![sb-pubsub](https://user-images.githubusercontent.com/33935506/141960159-9c14a473-6663-4258-967f-a4175ab0e6d2.png)

---
