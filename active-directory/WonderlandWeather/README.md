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

# Wonderland Weather Example

This is an example application that demonstrates how to use Azure AD to authorize both a React application and .NET 6 API.

---

## Contents

The **_[Overview](#overview)_** section highlights some of the primary goals for this example application. The following points serve as a guide to better understand this example application.

1. [Azure AD Tenant Setup]
1. [Azure AD User Setup]
1. [React Application App Registration]
1. [.NET API App Registration]
1. [Postman Setup]

It is suggested to follow the guide in order.

## Overview

The application is for a fictitious company called **_Wonderland Weather_**. Furthermore, the application is comprised of the following primary components:

- React Application
- .NET 6 API
- Azure AD Tenant and Users
- Azure AD App Registrations for both React application and .NET 6 API

Additionally, I also demonstrate how to setup Postman to initiate authorised requests to the .NET 6 API.

The following diagram illustrates an overview of what this application demonstrates:

![design](https://user-images.githubusercontent.com/33935506/137100747-0bc8a06b-0fbe-4471-9781-ede7e6524c8d.png)

### React Application

The react application is built using create-react-app and typescript. The primary features of the React application are as follows:

- Sign-in - Uses Azure AD to authenticate user sign-in
- Profile - Connects to Microsoft GraphQL API to retrieve profile information for signed-in user
- Forecasts - Provides the functionality to load forecast data from a .NET 6 API

The React application also demonstrates how to use the following Azure AD React components:

- MSAL (Microsoft Authentication Library)
  
  According to the [official documentation](https://www.npmjs.com/package/@azure/msal-react#about)

  > The MSAL library for JavaScript enables client-side JavaScript applications to authenticate users using Azure AD work and school accounts (AAD), Microsoft personal accounts (MSA) and social identity providers like Facebook, Google, LinkedIn, Microsoft accounts, etc. through Azure AD B2C service. It also enables your app to get tokens to access Microsoft Cloud services such as Microsoft Graph.

  To learn more about MSAL, see the [official Microsoft documentation](https://docs.microsoft.com/en-gb/azure/active-directory/develop/msal-overview)

- MsalProvider
- AuthenticatedTemplate and UnauthenticatedTemplate components
- Acquiring authentication token from Azure AD
- Making an authorised request to .NET 6 API to retrieve a list of forecasts
- How to setup basic routing (React Router) for authenticated and unauthenticated views

### .NET 6 API

The .NET 6 API is built using the latest version of .NET 6. The API exposes a single secure endpoint (secured using Azure AD) that provides forecasts data. The primary features of the API are as follows:

- Provides a single secured endpoint (secured using Azure AD) that returns weather forecast data
- Shows how to configure API to use Azure AD
- Shows how to use .NET user secrets (for development purposes) to store Azure AD configuration
- Shows how to setup CORS to allow requests from the React application

### Azure AD Tenant and Users

In order to setup authorization for the React application and .NET 6 API, we need to have an Azure Tenant (with associated Azure AD) and some test users. This guide will illustrate how to setup a new Tenant (called Wonderland Weather) from scratch. There is also a guide that illustrates how to add users to Azure AD.

The following guides illustrate the above requirements and concepts:

- [Azure AD Tenant Setup]
- [Azure AD User Setup]

### Azure AD App Registrations for both React application and .NET 6 API

An App Registration is the mechanism to enable Azure AD authorization for your applications.

According to the [official Microsoft documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app#register-an-application)

> Registering your application establishes a trust relationship between your app and the Microsoft identity platform. The trust is unidirectional: your app trusts the Microsoft identity platform, and not the other way around.

For more information on how to do App Registrations for React application and .NET 6 API, see the following:

- [React Application App Registration]
- [.NET API App Registration]

---

## Screenshots

### Landing Page

![app-1](https://user-images.githubusercontent.com/33935506/135736348-f131f344-29c0-42ac-9b62-8620fc7e5d2a.png)

### Signin - Username

![app-2](https://user-images.githubusercontent.com/33935506/135736349-b9d2dbd5-cdb1-49dc-870d-29e42e3fc582.png)

### Signin - Password

![app-3](https://user-images.githubusercontent.com/33935506/135736350-40006ed3-24a7-4322-b8f9-6b6663b49ddc.png)

### Home Page

![app-4](https://user-images.githubusercontent.com/33935506/135736352-8afa1d4a-8b44-4bc0-bf88-c2c47de249d6.png)

### Home Page - Load Forecasts

![app-5](https://user-images.githubusercontent.com/33935506/135736354-f8d7da95-d117-4f7a-bc39-7bffa944306a.png)

### Select Profile from Menu

![app-6](https://user-images.githubusercontent.com/33935506/135736355-c4c0b9cf-fc2c-4771-8546-0a5555b6ea43.png)

### Profile Information (from Microsoft Graph)

![app-7](https://user-images.githubusercontent.com/33935506/135736356-9438b53f-df15-40eb-9b7f-186ff17f60a9.png)

### Signout

![app-8](https://user-images.githubusercontent.com/33935506/135736357-77e2f7f6-8207-4d6c-bb43-f73f2d06bfb9.png)

---

[Azure AD Tenant Setup]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeather/docs/1-azuread-tenant-setup.md
[Azure AD User Setup]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeather/docs/2-azuread-user-setup.md
[React Application App Registration]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeather/docs/3-reactapp-azuread-appregistration.md
[.NET API App Registration]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeather/docs/4-api-azuread-appregistration.md
[Postman Setup]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeather/docs/5-postman-setup.md