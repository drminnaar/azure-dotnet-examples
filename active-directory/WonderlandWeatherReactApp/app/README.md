# Azure AD with React

This project was bootstrapped with [Create React App](https://github.com/facebook/create-react-app).

## Description

In this example, we will build a React Application that is protected by Azure AD. This example is inspired from the offical [Microsoft example](https://docs.microsoft.com/en-us/azure/active-directory/develop/tutorial-v2-react).

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

## Screenshots

### Landing Page

![app-1](https://user-images.githubusercontent.com/33935506/135736348-f131f344-29c0-42ac-9b62-8620fc7e5d2a.png)

### Signin - Username

![app-2](https://user-images.githubusercontent.com/33935506/135736349-b9d2dbd5-cdb1-49dc-870d-29e42e3fc582.png)

### Signin - Password

![app-3](https://user-images.githubusercontent.com/33935506/135736350-40006ed3-24a7-4322-b8f9-6b6663b49ddc.png)

### Landing Page

![app-4](https://user-images.githubusercontent.com/33935506/135736352-8afa1d4a-8b44-4bc0-bf88-c2c47de249d6.png)

### Landing Page - Load Forecasts

![app-5](https://user-images.githubusercontent.com/33935506/135736354-f8d7da95-d117-4f7a-bc39-7bffa944306a.png)

### Select Profile from Menu

![app-6](https://user-images.githubusercontent.com/33935506/135736355-c4c0b9cf-fc2c-4771-8546-0a5555b6ea43.png)

### Profile Information (from Microsoft Graph)

![app-7](https://user-images.githubusercontent.com/33935506/135736356-9438b53f-df15-40eb-9b7f-186ff17f60a9.png)

### Signout

![app-8](https://user-images.githubusercontent.com/33935506/135736357-77e2f7f6-8207-4d6c-bb43-f73f2d06bfb9.png)

---

## Design

![design](https://user-images.githubusercontent.com/33935506/135556236-7da6783c-6140-4b4c-9cbe-5881a11d5825.png)

---

## Getting Started

In order to run the React application, you will need to ensure the following:

- You have a valid Tenant (default or custom)
- You have a user that can be used to test access to the React application
- You have created an **_App Registration_** to protect the React application
- You have configured the React application with the appropriate registered application details

Once you have complete all the configuration and setup that has been outlined in this document, you can run the API as follows:

```bash
# start React application
npm start
```

---

## Azure AD Setup

### Create Tenant (optional)

If you would prefer to use a new Tenant over your existing default Tenant, then you can follow the steps in this section.

In this section we will create a new Tenant to represent our fictitious organization called **_Wonderland Weather_**.

#### Manage Tenants

**Steps:**

- Open **_Azure Active Directory_** and select the option to `Manage tenants`

![create-tenant-1](https://user-images.githubusercontent.com/33935506/135533941-e551db0b-f24d-4276-a25f-4340a21800dd.png)

#### Create Tenant - Basics

**Steps:**

- Select the option to `Create` new Tenant.

![create-tenant-2](https://user-images.githubusercontent.com/33935506/135533943-d7fd7f25-9eaa-472b-a965-b42b8e12081f.png)

**Steps:**

- Select the `Azure Active Directory` option.
- Click `Next: Configuration` button

![create-tenant-3](https://user-images.githubusercontent.com/33935506/135533944-4e85b46d-66b8-4c50-bdcd-8f4948600945.png)

### Create Tenant - Configuration

**Steps:**

- Complete the configuration information for Organization name, Domain name, and Country
- Click `Next: Review + Create` button

![create-tenant-4](https://user-images.githubusercontent.com/33935506/135533946-ebad87da-af72-46d8-bb7f-0a2a141af8df.png)

### Review & Create

**Steps:**

- Click `Create` button to create Tenant

![create-tenant-5](https://user-images.githubusercontent.com/33935506/135533947-7c21db2b-0bfb-4d36-ab5c-cd42d3ffefb9.png)

![create-tenant-6](https://user-images.githubusercontent.com/33935506/135533950-a4e9976e-1085-4327-8be4-96560e95e35b.png)

---

## User Setup

Whether you're using your default Tenant, or a custom Tenant (see above [Azure AD Setup](#azure-ad-setup)),you will need a user in the Tenant that can be used to test React application access.

The follow steps show how to create a user:

- Open Active Directory in preferred Tenant
- Select `Users` option from side-menu
- Select option `New user` to start user creation process

![create-user-1](https://user-images.githubusercontent.com/33935506/135537826-bbe9967a-7f4a-42cf-a431-796fb75fbf37.png)

- Complete required user information
- Click `Create` button to create user

![create-user-2](https://user-images.githubusercontent.com/33935506/135537827-f7451c9f-0333-442f-ab90-6101132d65b7.png)

- The newly created user should display in your users list

![create-user-3](https://user-images.githubusercontent.com/33935506/135537829-05f70b72-4952-4d9e-b047-67557ca3da8b.png)

---

## App Registration

According to the [official Microsoft Azure documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app#register-an-application),

> Registering your application establishes a trust relationship between your app and the Microsoft identity platform. The trust is unidirectional: your app trusts the Microsoft identity platform, and not the other way around.

The following steps show how to create an App Registration for *Wonderland Weather API*.

### Register App

**Steps:**

- In your Azure AD Tenant, select the `App Registrations` option from the side menu
- Select `New Registration` option

![app-reg-1](https://user-images.githubusercontent.com/33935506/135735830-ca2f822e-91b5-4d5f-80a7-73773ce278eb.png)

- Provide App Registration information
- Ensure that you select "Single-page application (SPA) and provide a redirect URI that matches the address of the React application
- Select `Register` button

![app-reg-2](https://user-images.githubusercontent.com/33935506/135735831-533f67cc-515a-4f40-9114-59b46ff6fd45.png)

- After App Registration is complete, you are direcrect to the Overview of the newly registered application
- Take note of the Application (client) ID and Directory (tenant) ID as they will be used when we configure our React application

![app-reg-3](https://user-images.githubusercontent.com/33935506/135735833-0321f40b-a453-4f64-b8b4-da04c2604952.png)

- Select the `Authentication` option from the side menu
- Ensure that redirect URI (<http:localhost:3000>) is provided
- Ensure that none of the token options are selected
- Click `Save` if you make any changes

![app-reg-4](https://user-images.githubusercontent.com/33935506/135735834-1bb8f32d-60ee-4827-9b7b-796f1b29b4d8.png)

- Select the `API Permissions` option from side menu. Select the option `Grant admin consent for Wonderland Weather`. This will authorize our React application to make calls to API's like Microsoft Graph.

![app-reg-5](https://user-images.githubusercontent.com/33935506/135735835-00fda1dc-0e90-42e5-9828-09b4602ba93f.png)
![app-reg-6](https://user-images.githubusercontent.com/33935506/135735836-84c223d7-e1bf-476c-b7e1-9d6201efa4ba.png)

- The status of permissions consent should show as "Granted"

![app-reg-7](https://user-images.githubusercontent.com/33935506/135735837-be20eba7-7952-4017-93d6-0719d2ae92ab.png)

---

## React Application Setup

### App.ts

```typescript
import {
    AuthenticatedTemplate,
    UnauthenticatedTemplate,
} from '@azure/msal-react';

// styles
import './App.css';

// components
import { AuthenticatedView } from './features/auth/AuthenticatedView';
import { UnauthenticatedView } from './features/auth/UnauthenticatedView';

function App() {
    return (
        <>
            <AuthenticatedTemplate>
                <AuthenticatedView />
            </AuthenticatedTemplate>

            <UnauthenticatedTemplate>
                <UnauthenticatedView />
            </UnauthenticatedTemplate>
        </>
    );
}

export { App };
```

### Unauthenticated View

```typescript
export const UnauthenticatedView = () => {
    return (
        <LandingPage />
    )
}
```

### Authenticated View

```typescript
export const AuthenticatedView = () => {
    return (
        <BrowserRouter>
            <NavBar />
            <div className='container-fluid px-3 pt-3'>
                <Route exact path='/' component={HomePage} />
                <Route exact path='/profile' component={ProfilePage} />
                <Route exact path='/forecasts' component={HomePage} />
            </div>
        </BrowserRouter>
    );
};
```

### Signin

### Signout

---
