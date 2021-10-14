![az-examples-ad-reactapp](https://user-images.githubusercontent.com/33935506/137251575-d9a0a111-46d4-4687-8a99-5043f1dd49bb.png)

# Azure AD with React

This project was bootstrapped with [Create React App](https://github.com/facebook/create-react-app) using the **_TYPESCRIPT_** template.

## Contents

1. [Description](#description)
1. [Overview](#overview)
1. [Screenshots](#screenshots)
1. [Getting Started](#getting-started)

## Description

In this example, we will build a React Application (written in Typescript) that is protected by Azure AD. This example is inspired from the offical [Microsoft example](https://docs.microsoft.com/en-us/azure/active-directory/develop/tutorial-v2-react).

The application is for a fictitious company called Wonderland Weather. The following features comprise the application:

- Signin
- Signout
- Forecasts View
- Profile View

This example demonstrates the following primary topics:

- Create a React application
- Register a React application in Azure AD
- Configure React application to use Azure AD for authentication and authorization
- Use [Microsoft Authentication Library (MSAL)](https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-overview)
- Add code to call Microsoft Graph API

---

## Overview

The application is for a fictitious company called **_Wonderland Weather_**. Furthermore, the application is comprised of the following primary components:

- React Application
- Azure AD Tenant and Users
- Azure AD App Registrations for React application

### React Application

The react application is built using create-react-app and typescript. The primary features of the React application are as follows:

- Sign-in - Uses Azure AD to authenticate user sign-in
- Profile - Connects to Microsoft GraphQL API to retrieve profile information for signed-in user
- Forecasts - Provides the functionality to load forecast data from a json file

The React application also demonstrates how to use the following Azure AD React components:

- MSAL (Microsoft Authentication Library)
  
  According to the [official documentation](https://www.npmjs.com/package/@azure/msal-react#about)

  > The MSAL library for JavaScript enables client-side JavaScript applications to authenticate users using Azure AD work and school accounts (AAD), Microsoft personal accounts (MSA) and social identity providers like Facebook, Google, LinkedIn, Microsoft accounts, etc. through Azure AD B2C service. It also enables your app to get tokens to access Microsoft Cloud services such as Microsoft Graph.

  To learn more about MSAL, see the [official Microsoft documentation](https://docs.microsoft.com/en-gb/azure/active-directory/develop/msal-overview)

- MsalProvider
- AuthenticatedTemplate and UnauthenticatedTemplate components
- How to setup basic routing (React Router) for authenticated and unauthenticated views

### Azure AD Tenant and Users

In order to setup authorization for the React application, we need to have an Azure Tenant (with associated Azure AD) and some test users. This guide will illustrate how to setup a new Tenant (called Wonderland Weather) from scratch. There is also a guide that illustrates how to add users to Azure AD.

The following guides illustrate the above requirements and concepts:

- [Azure AD Tenant Setup]
- [Azure AD User Setup]

### Azure AD App Registration for React application

An App Registration is the mechanism to enable Azure AD authorization for your applications.

According to the [official Microsoft documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app#register-an-application)

> Registering your application establishes a trust relationship between your app and the Microsoft identity platform. The trust is unidirectional: your app trusts the Microsoft identity platform, and not the other way around.

For more information on how to do App Registration for React application, see the following:

- [React Application App Registration]

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

## Getting Started

In order to run the React application, you will need to ensure the following:

- You have a valid Tenant (default or custom)
- You have a user that can be used to test access to the React application
- You have created an **_App Registration_** to protect the React application
- You have configured the React application with the appropriate registered application details

### App Registration

The following steps should be followed to **_CONFIGURE_** application in Azure AD:

1. [Azure AD Tenant Setup]
1. [Azure AD User Setup]
1. [React Application App Registration]

### Run Application

The following steps should be followed to **_RUN_** application:

#### Install all NPM modules

From the root of the application (where the package.json file is located), run the following command to install all NPM modules:

```bash
npm install
```

#### Configure '.env' File

All configuration for the application is specified in an environment file. Therefore, you will need to add a `.env` file to the root of application (should be in same folder as package.json).

The `.env` file should resemble the following:

```javascript
REACT_APP_CLIENTID='<Application (Client) ID from app registration>' // see React app registration guide
REACT_APP_AUTHORITY='https://login.microsoftonline.com/<Directory (tenant) ID>'
REACT_APP_REDIRECTURI='http://localhost:3000'
```

You will need to specify your specific settings in the above file. Please see the [React Application App Registration] guide for more details.

#### Start App

From the root of the application (where the package.json file is located), run the following command to run application:

```bash
npm start
```

The application runs at the following address:

```bash
http://localhost:3000
```

---

[Azure AD Tenant Setup]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeatherApp/app/docs/1-azuread-tenant-setup.md
[Azure AD User Setup]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeatherApp/app/docs/2-azuread-user-setup.md
[React Application App Registration]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeatherApp/app/docs/3-reactapp-azuread-appregistration.md
