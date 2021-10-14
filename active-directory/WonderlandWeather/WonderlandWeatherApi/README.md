---
page_type: sample
languages:
- C#
products:
- Azure
description: "An example .NET 6 Web API configured to use Azure AD for auth"
---

# Wonderland Weather .NET 6 API

---

## Getting Started

The following steps should be followed in order to run the API.

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

[Postman Setup]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeather/docs/5-postman-setup.md