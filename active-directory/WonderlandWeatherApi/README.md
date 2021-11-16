---
page_type: sample
languages:
- Typescript
- C#
products:
- Azure
- Azure Active Directory
description: "Demonstrates how to use Azure AD to authorize both a React application and .NET 6 API"

---

# Wonderland Weather .NET 6 API

---

## Contents

The following points serve as a guide to better understand this example application.

1. [Description](#description)
1. [Overview](#overview)
1. [Getting Started](#getting-started)

---

## Description

In this example, we will build a C#.NET 6 WebAPI that is protected by Azure AD.

The API is for a fictitious company called Wonderland Weather. There is a single endpoint that returns forecast data.

This example demonstrates the following primary topics:

- Create basic C#.NET6 WebAPI
- Configure WebAPI to use Azure AD for authentication and authorization
- Configure Azure AD to protect WebAPI
- Configure Postman Collection to initiate authenticated requests to WebAPI

The goal is to make a request from Postman to the .NET WebAPI to retrieve a list of weather forecasts. The general flow is illustrated in the following diagram:

![design](https://user-images.githubusercontent.com/33935506/135556236-7da6783c-6140-4b4c-9cbe-5881a11d5825.png)

---

## Overview

The application is for a fictitious company called **_Wonderland Weather_**. Furthermore, the application is comprised of the following primary components:

- .NET 6 API
- Azure AD Tenant and Users
- Azure AD App Registration for .NET 6 API

### .NET 6 API

The .NET 6 API is built using the latest version of .NET 6. The API exposes a single secure endpoint (secured using Azure AD) that provides forecasts data. The primary features of the API are as follows:

- Provides a single secured endpoint (secured using Azure AD) that returns weather forecast data
- Shows how to configure API to use Azure AD
- Shows how to use .NET user secrets (for development purposes) to store Azure AD configuration

### Azure AD Tenant and Users

In order to setup authorization for the .NET 6 API, we need to have an Azure Tenant (with associated Azure AD) and some test users. This guide will illustrate how to setup a new Tenant (called Wonderland Weather) from scratch. There is also a guide that illustrates how to add users to Azure AD.

The following guides illustrate the above requirements and concepts:

- [Azure AD Tenant Setup]
- [Azure AD User Setup]

### Azure AD App Registration for .NET 6 API

An App Registration is the mechanism to enable Azure AD authorization for your applications.

According to the [official Microsoft documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app#register-an-application)

> Registering your application establishes a trust relationship between your app and the Microsoft identity platform. The trust is unidirectional: your app trusts the Microsoft identity platform, and not the other way around.

For more information on how to do App Registrations for .NET 6 API, see the following:

- [.NET API App Registration]

---

## Getting Started

In order to run this protected WebAPI, you will need to ensure the following steps have been followed:

- You have a valid Tenant (default or custom)
- You have a user that can be used to test access to WebAPI
- You have created an **_App Registration_** to protect the WebAPI
- You have configured the WebAPI application with the appropriate registered application details
- You have completed the section on setting up postman to make api requests to protected WebAPI

The following steps should be followed in order to **_CONFIGURE_** the API:

1. [Azure AD Tenant Setup]
1. [Azure AD User Setup]
1. [.NET API App Registration]

For more information on Postman setup, see [Postman Setup] guide.

The following steps should be followed in order to **_RUN_** the API:

### Configure Azure AD Configuration

Typically, one would configure your `appsettings.json` to include the Azure AD configuration as follows:

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com",
    "Domain": "wonderlandweather.onmicrosoft.com",
    "TenantId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
    "ClientId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" // AKA AppId
  }
}
```

Please take not of the following [microsoft documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-protected-web-api-app-configuration#case-where-you-used-a-custom-app-id-uri-for-your-web-api) regarding a scenario where you changed your App ID URI to a custom App ID URI ie. you didn't accept the default "api://xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx". If this is applies to you, then you will need to provide the "Audience" property in the configuration as well.

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "ClientId": "[Client_id-of-web-api-eg-2ec40e65-ba09-4853-bcde-bcb60029e596]",
    "TenantId": "common",
    "Audience": "custom App ID URI for your web API"
  }
}
```

For this example I have opted to use dotnet user secrets to store my Azure AD information. If you would like to do the same, you can follow the following steps:

```bash
# from the root of your WebAPI project, type the following to initialize secrets
dotnet user-secrets init

# add the following user secrets
dotnet user-secrets set "AzureAd:Instance" "https://login.microsoftonline.com"
dotnet user-secrets set "AzureAd:Domain" "wonderlandweather.onmicrosoft.com"
dotnet user-secrets set "AzureAd:TenantId" "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
dotnet user-secrets set "AzureAd:ClientId" "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
dotnet user-secrets set "AzureAd:Scopes" "Forecasts.Read"
```

After configuring user secrets, you can remove the "AzureAD" configuration from the `appsettings.json` file.

### Start API

Start the API by running the following command from the root of the project:

```bash
dotnet run --project ./WonderlandWeatherApi/WonderlandWeather.Api.csproj
```

The API will run at the following address:

```bash
https://localhost:5001
```

### Test API Using Postman

Please see the [Postman Setup] guide to see how to configure Postman to make requests to forecasts API.

---

[Azure AD Tenant Setup]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeatherApi/docs/1-azuread-tenant-setup.md
[Azure AD User Setup]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeatherApi/docs/2-azuread-user-setup.md
[.NET API App Registration]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeatherApi/docs/3-api-azuread-appregistration.md
[Postman Setup]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeatherApi/docs/4-postman-setup.md