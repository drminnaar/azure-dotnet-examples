---
page_type: sample
languages:
- typescript
products:
- Azure
description: "An example React application configured to use Azure AD for auth"
---

# Wonderland Weather React App

## Getting Started

### Install all NPM modules

From the root of the application (where the package.json file is located), run the following command to install all NPM modules:

```bash
npm install
```

### Configure '.env' File

All configuration for the application is specified in an environment file. Therefore, you will need to add a `.env` file to the root of application (should be in same folder as package.json).

The `.env` file should resemble the following:

```javascript
REACT_APP_CLIENTID='<Application (Client) ID from app registration>' // see React app registration guide
REACT_APP_AUTHORITY='https://login.microsoftonline.com/<Directory (tenant) ID>'
REACT_APP_REDIRECTURI='http://localhost:3000'
REACT_APP_FORECAST_API_URL='https://localhost:5001'
REACT_APP_FORECAST_API_SCOPES='api://<Application ID URI>/Forecasts.Read' //see scope configuration in api app registration guide
REACT_APP_FORECAST_API_ENABLED=true // change to false to load data from a file instead of an API
```

You will need to specify your specific settings in the above file. Please see the following 2 guides to better understand where to obtain required values:

- [React Application App Registration]
- [.NET API App Registration]

### Start App

From the root of the application (where the package.json file is located), run the following command to run application:

```bash
npm start
```

The application runs at the following address:

```bash
http://localhost:3000
```

---

[React Application App Registration]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeather/docs/3-reactapp-azuread-appregistration.md
[.NET API App Registration]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/active-directory/WonderlandWeather/docs/4-api-azuread-appregistration.md