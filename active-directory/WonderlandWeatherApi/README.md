![banner](https://user-images.githubusercontent.com/33935506/135551999-391fa165-76bd-4984-a4c7-818ef2c934a9.png)

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

## Getting Started

In order to run this protected WebAPI, you will need to ensure the following steps have been followed:

- You have a valid Tenant (default or custom)
- You have a user that can be used to test access to WebAPI
- You have created an **_App Registration_** to protect the WebAPI
- You have configured the WebAPI application with the appropriate registered application details
- You have completed the section on setting up postman to make api requests to protected WebAPI

Once you have complete all the configuration and setup that has been outlined in this document, you can run the API as follows:

```bash
# start API
dotnet run --project ./WonderlandWeather.Api/
```

See the section [Configure Postman](#configure-postman) on how to make calls to WebAPI.

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

Whether you're using your default Tenant, or a custom Tenant (see above [Azure AD Setup](#azure-ad-setup)),you will need a user in the Tenant that can be used to test WebAPI access.

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

- Switch To Appropriate Tenant

  ![app-reg-1](https://user-images.githubusercontent.com/33935506/135538347-ddda9997-9152-46ea-baec-4728db230e78.png)

- Choose `App Registrations`

  ![app-reg-2](https://user-images.githubusercontent.com/33935506/135538351-1d39a5b1-1210-4010-9f38-dcc534126b4e.png)

- Select `New Registration`

  ![app-reg-3](https://user-images.githubusercontent.com/33935506/135538352-abf39f66-d168-469d-9921-5276717eb089.png)

- Provide basic App Registration information, and select `Register`

  ![app-reg-4](https://user-images.githubusercontent.com/33935506/135538355-c2c57211-0743-4839-ac60-cdad649e7b6c.png)

### Expose API

**Steps:**

- Review newly created App Registration
- Select option to `Expose an API`

  ![app-reg-5](https://user-images.githubusercontent.com/33935506/135547774-0cf5101f-f7eb-4b7a-af43-e7493841c234.png)

### Configure Scopes

**Steps:**

- Take note that the initial Application ID URI has not been set yet.
- Select option to `Add a scope`

  ![app-reg-6](https://user-images.githubusercontent.com/33935506/135538357-3731b6d0-15cd-410f-a755-369d4e0a71b2.png)

### Add Scopes

**Steps:**

- Configure **_Application ID URI_**

  ![app-reg-7](https://user-images.githubusercontent.com/33935506/135547880-639d74e8-a14d-4b19-a685-3a6b230a66a3.png)

- Enter scope information. Use a naming standard that makes sense in your environment. I typically use the convention of **_"Endpoint.AccessType"_** which in this instance translates to **_Forecasts.Read_**.

  ![app-reg-8](https://user-images.githubusercontent.com/33935506/135548081-e970c130-d4d0-47f1-bef5-f6297974d62d.png)

### App Registration Complete

  ![app-reg-9](https://user-images.githubusercontent.com/33935506/135557469-b7f85e5e-c8b8-4f0f-9c1c-142ac0d883ae.png)

---

## Code Setup

The following 3 files are the most important in terms of understanding how to configure Azure AD with our API:

- Program.cs - This is where we configure Azure AD services and authentication middleware
- ForecastsController.cs - This is the actual controller that we're securing
- appsettings.json - This is where one would typically store "AzureAd" configuration

The following Nuget Packages are required dependencies:

- <span>Microsoft.AspNetCore.Authentication.JwtBearer</span>
- <span>Microsoft.AspNetCore.Authentication.OpenIdConnect</span>
- <span>Microsoft.Identity.Web</span>

Because .NET 6 has not been released as LTS, I am using Release Candidate (RC) versions:

```xml
<ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="6.0.0-rc.1.21452.15" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.OpenIdConnect" Version="6.0.0-rc.1.21452.15" />
    <PackageReference Include="Microsoft.Identity.Web" Version="1.17.0" />
</ItemGroup>
```

### Configure Azure AD Services

This section demonstrates the most important parts to get Azure AD configured with the API.

#### Configure Services

```csharp
/// Program.cs

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);


// ---------------------------------------------------------------------------
// Configure Azure AD Services
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
// ---------------------------------------------------------------------------


builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "WonderlandWeather API", Version = "v1" });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WonderlandWeather API v1"));
}

app.UseHttpsRedirection();


// ---------------------------------------------------------------------------
// Configure Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();
// ---------------------------------------------------------------------------


app.MapControllers();
app.Run();
```

### Protect ForecastsController

```csharp
/// ForecastsController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using WonderlandWeather.Api.Models;

namespace WonderlandWeather.Api.Controllers;

[Authorize] // Provides authentication
[ApiController]
[Route("[controller]")]
public sealed class ForecastsController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")] // Provides authorization
    public IEnumerable<Forecast> Get()
    {
        return Enumerable
            .Range(1, 5)
            .Select(index => new Forecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [AllowAnonymous] // make unprotected
    [HttpOptions]
    public IActionResult GetOptions()
    {
        Response.Headers.Add("Allow", "GET,OPTIONS");
        return NoContent();
    }
}
```

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

---

## Postman Setup

### Configure App Registration

Before setting up a Postman Collection to handle Azure AD auth, we need to first configure the Wonderland Weather API to provide authentication to Postman application.

- Open appropriate Tenant and select the `Authentication` option from the sie menu
- Select the option to `Add platform`

  ![app-reg-postman-auth-1](https://user-images.githubusercontent.com/33935506/135540360-d899f997-9223-4557-8790-174b3d306b4f.png)

- For platform configuration, select `Mobile and desktop applications`

  ![app-reg-postman-auth-2](https://user-images.githubusercontent.com/33935506/135540362-56e56038-1e1e-4cef-86d2-81b8e8687d16.png)

- Provide the Postman callback url for `Custom redirect URIs`

  ![app-reg-postman-auth-3](https://user-images.githubusercontent.com/33935506/135540364-b7e31d11-dd7a-440b-bd65-2e6205eb3a92.png)

  ![app-reg-postman-auth-4](https://user-images.githubusercontent.com/33935506/135540365-5f5e911f-65ce-461e-a185-0825c123305f.png)

- We will require both the auth and token endpoints to configure Postman auth.
- Select the `Overview` option from the side-menu
- Select the `Endpoints` option at the top

  ![app-reg-postman-auth-5](https://user-images.githubusercontent.com/33935506/135548334-01c21b20-69a0-4797-bfc1-7f61421509e8.png)

- Take note of both the authorization (v2) and token (v2) endpoints

  ![app-reg-postman-auth-6](https://user-images.githubusercontent.com/33935506/135540368-4bbf89ad-f073-46ff-81a3-578ebeb062be.png)

### Configure Postman

#### Import Postman Files

The project repository has both an environment and collection file that can be imported.

##### Environment File

```json
// WonderlandWeather.postman_environment.json

{
 "id": "f14c3d9f-1e19-422c-9ea9-3774efe321d4",
 "name": "WonderlandWeather",
 "values": [
  {
   "key": "ClientId",
   "value": "client_id-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
   "enabled": true
  },
  {
   "key": "Scopes",
   "value": "api://client_id-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Forecasts.Read",
   "enabled": true
  },
  {
   "key": "AuthUrl",
   "value": "https://login.microsoftonline.com/tenant_id-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/oauth2/v2.0/authorize",
   "enabled": true
  },
  {
   "key": "AccessTokenUrl",
   "value": "https://login.microsoftonline.com/tenant_id-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/oauth2/v2.0/token",
   "enabled": true
  }
 ],
 "_postman_variable_scope": "environment",
 "_postman_exported_at": "2021-10-01T02:54:08.591Z",
 "_postman_exported_using": "Postman/9.0.2"
}
```

##### Collection File

```json
// Wonderland Weather.postman_collection.json


{
 "info": {
  "_postman_id": "a625b970-155d-416b-b879-24d520a16249",
  "name": "Wonderland Weather",
  "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
 },
 "item": [
  {
   "name": "Forecasts",
   "item": [
    {
     "name": "Get All Forecasts",
     "request": {
      "method": "GET",
      "header": [],
      "url": {
       "raw": "https://localhost:5001/forecasts",
       "protocol": "https",
       "host": [
        "localhost"
       ],
       "port": "5001",
       "path": [
        "forecasts"
       ]
      }
     },
     "response": []
    },
    {
     "name": "Get Options (no auth)",
     "request": {
      "method": "OPTIONS",
      "header": [],
      "url": {
       "raw": "https://localhost:5001/forecasts",
       "protocol": "https",
       "host": [
        "localhost"
       ],
       "port": "5001",
       "path": [
        "forecasts"
       ]
      }
     },
     "response": []
    }
   ]
  }
 ],
 "auth": {
  "type": "oauth2",
  "oauth2": [
   {
    "key": "client_authentication",
    "value": "body",
    "type": "string"
   },
   {
    "key": "state",
    "value": "{{$randomUUID}}",
    "type": "string"
   },
   {
    "key": "accessTokenUrl",
    "value": "{{AccessTokenUrl}}",
    "type": "string"
   },
   {
    "key": "clientSecret",
    "value": "",
    "type": "string"
   },
   {
    "key": "authUrl",
    "value": "{{AuthUrl}}",
    "type": "string"
   },
   {
    "key": "scope",
    "value": "{{Scopes}}",
    "type": "string"
   },
   {
    "key": "clientId",
    "value": "{{ClientId}}",
    "type": "string"
   },
   {
    "key": "redirect_uri",
    "value": "https://app.getpostman.com/oauth2/callback",
    "type": "string"
   },
   {
    "key": "tokenName",
    "value": "WonderlandWeather",
    "type": "string"
   },
   {
    "key": "addTokenTo",
    "value": "header",
    "type": "string"
   }
  ]
 },
 "event": [
  {
   "listen": "prerequest",
   "script": {
    "type": "text/javascript",
    "exec": [
     ""
    ]
   }
  },
  {
   "listen": "test",
   "script": {
    "type": "text/javascript",
    "exec": [
     ""
    ]
   }
  }
 ]
}
```

#### Create Collection

As an alternative to importing Postman environment and collection, we can create a new Postman Collection as follows.

- Create a new Postman Collection in Postman called **_Wonderland Weather_**.

#### Configure Collection Authorization

For this section, you will require the following information:

- Client ID (Application ID)
- Authorization (v2) endpoint URL
- Token (v2) endpoint URL
- Scopes
- Postman CallbackURL: <https://app.getpostman.com/oauth2/callback>

Follow the following steps to configure Authorization for Collection:

- We start off with `No Auth` selected for our authorization type

 ![app-reg-postman-auth-7](https://user-images.githubusercontent.com/33935506/135540371-51a458b5-2d7d-4ac9-b201-1addf0960e97.png)

- Select `OAuth 2.0`

  ![app-reg-postman-auth-8](https://user-images.githubusercontent.com/33935506/135540373-3374259b-55d5-432c-8cbc-a644fe75fa1d.png)

- Complete the required configuration information. Please take note that I use environment variables for my configuration as some of the information is sensitive.

  ![app-reg-postman-auth-9](https://user-images.githubusercontent.com/33935506/135540374-aea5690d-4888-4d9d-9184-be06e5029ffb.png)

- My environment is configured as follows

  ![app-reg-postman-auth-10](https://user-images.githubusercontent.com/33935506/135540375-f9a0ec1b-41f1-4070-b4fd-fc045367188b.png)

#### Get Token

- At this point we are ready to initiate a `Get New Access Token` action. After completing the action, you will be prompted to enter authentication information. Use the user details that we created in the [User Setup](#user-setup) above.

  ![app-reg-postman-auth-11](https://user-images.githubusercontent.com/33935506/135540377-d9497540-dd8b-441f-b439-09d6bc6e1a2b.png)

- Enter password

  ![app-reg-postman-auth-12](https://user-images.githubusercontent.com/33935506/135540378-de2d7957-e306-4a75-9e0f-6ad0650304f0.png)

- Update password. This will only happen on first login.

  ![app-reg-postman-auth-13](https://user-images.githubusercontent.com/33935506/135540379-23972c9e-a1a2-49fb-935b-d12625d0ffe3.png)

- After signing in successfully, you will be presented with the following screen indicating that authentication is complete

  ![app-reg-postman-auth-14](https://user-images.githubusercontent.com/33935506/135540380-06fa5d05-2d69-4111-a60f-a6f1410f7129.png)

- Select the option to `Use token`

  ![app-reg-postman-auth-15](https://user-images.githubusercontent.com/33935506/135540382-01d4124f-21bc-4857-b70e-bfaf6ce9815d.png)

  ![app-reg-postman-auth-16](https://user-images.githubusercontent.com/33935506/135540384-75ad2a5a-c35d-44ff-871e-de4bf3e69374.png)

#### Create Forecasts Request

- Create a new request in Collection and configure the Authorization to inherit from parent

  ![get-forecasts-1](https://user-images.githubusercontent.com/33935506/135542437-bd6ce5c5-9070-42f7-9ca1-a4e0e8cde56b.png)

- Initiate request. You should see a list of forecasts being returned.

  ![get-forecasts-2](https://user-images.githubusercontent.com/33935506/135542439-f77c6f07-efc8-481d-92f2-b82ae42e0a12.png)
