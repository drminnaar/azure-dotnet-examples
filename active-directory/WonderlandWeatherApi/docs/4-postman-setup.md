# Postman Setup

This guide illustrates how to configure Postman to make authorized requests using Azure AD.

---

## Configure App Registration

Before setting up a Postman Collection to handle Azure AD auth, we need to first configure the Wonderland Weather API to provide authentication to Postman application.

```text

- Open appropriate Tenant and select the `Authentication` option from the side menu
- Select the option to `Add platform`

```

![app-reg-postman-auth-1](https://user-images.githubusercontent.com/33935506/135540360-d899f997-9223-4557-8790-174b3d306b4f.png)

```text

- For platform configuration, select `Mobile and desktop applications`

```

![app-reg-postman-auth-2](https://user-images.githubusercontent.com/33935506/135540362-56e56038-1e1e-4cef-86d2-81b8e8687d16.png)

```text

- Provide the Postman callback url for `Custom redirect URIs`

```

![app-reg-postman-auth-3](https://user-images.githubusercontent.com/33935506/135540364-b7e31d11-dd7a-440b-bd65-2e6205eb3a92.png)

![app-reg-postman-auth-4](https://user-images.githubusercontent.com/33935506/135540365-5f5e911f-65ce-461e-a185-0825c123305f.png)

```text

- We will require both the auth and token endpoints to configure Postman auth.
- Select the `Overview` option from the side-menu
- Select the `Endpoints` option at the top

```

![app-reg-postman-auth-5](https://user-images.githubusercontent.com/33935506/135548334-01c21b20-69a0-4797-bfc1-7f61421509e8.png)

```text

- Take note of both the authorization (v2) and token (v2) endpoints

```

![app-reg-postman-auth-6](https://user-images.githubusercontent.com/33935506/135540368-4bbf89ad-f073-46ff-81a3-578ebeb062be.png)

---

## Configure Postman

You can use 1 of the 2 following options to get your Postman setup ready:

- [Import Postman Files](#import-postman-files)
  
  This is the easiest option. Both `collection` and `environment` files are provided. All you need to do is import them into your Postman workspace of choice.

- [Create and Configure Postman Collection](#create-and-configure-postman-collection)
  
  This is a detailed explanation of how to setup Azure AD authorization, and create a forecast request.

### Import Postman Files

The project repository has both an environment and collection file that can be imported.

<u>Environment File:</u>

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

<u>Collection File:</u>

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

### Create and Configure Postman Collection

As an alternative to importing Postman environment and collection, we can create a new Postman Collection.

For this section, you will require the following information:

- Client ID (Application ID)
- Authorization (v2) endpoint URL
- Token (v2) endpoint URL
- Scopes
- Postman CallbackURL: <https://app.getpostman.com/oauth2/callback>

```text
# STEP 1 - Create collection

- Create a new Postman Collection in Postman called **_Wonderland Weather_**.

```

```text
# STEP 2 - Configure auth

- Now that collection is created, select the `Authorization` tab
- We start off with `No Auth` selected for our authorization type

```

 ![app-reg-postman-auth-7](https://user-images.githubusercontent.com/33935506/135540371-51a458b5-2d7d-4ac9-b201-1addf0960e97.png)

```text
# STEP 3 - Select auth type

- Select `OAuth 2.0`

```

  ![app-reg-postman-auth-8](https://user-images.githubusercontent.com/33935506/135540373-3374259b-55d5-432c-8cbc-a644fe75fa1d.png)

```text
# STEP 4 - Complete required auth infomation

- Complete the required configuration information. Please take note that I use environment variables for my configuration as some of the information is sensitive.

```

  ![app-reg-postman-auth-9](https://user-images.githubusercontent.com/33935506/135540374-aea5690d-4888-4d9d-9184-be06e5029ffb.png)

```text

- My environment is configured as follows

```

  ![app-reg-postman-auth-10](https://user-images.githubusercontent.com/33935506/135540375-f9a0ec1b-41f1-4070-b4fd-fc045367188b.png)

```text
# STEP 5 - Get Access Token

- At this point we are ready to initiate a `Get New Access Token` action. After completing the action, you will be prompted to enter authentication information. Use the user details that we created in the [User Setup](#user-setup) above.
```

![app-reg-postman-auth-11](https://user-images.githubusercontent.com/33935506/135540377-d9497540-dd8b-441f-b439-09d6bc6e1a2b.png)

```text

- Enter password

```

![app-reg-postman-auth-12](https://user-images.githubusercontent.com/33935506/135540378-de2d7957-e306-4a75-9e0f-6ad0650304f0.png)

```text

- Update password. This will only happen on first login.

```

![app-reg-postman-auth-13](https://user-images.githubusercontent.com/33935506/135540379-23972c9e-a1a2-49fb-935b-d12625d0ffe3.png)

```text

- After signing in successfully, you will be presented with the following screen indicating that authentication is complete

```

![app-reg-postman-auth-14](https://user-images.githubusercontent.com/33935506/135540380-06fa5d05-2d69-4111-a60f-a6f1410f7129.png)

```text

- Select the option to `Use token`

```

![app-reg-postman-auth-15](https://user-images.githubusercontent.com/33935506/135540382-01d4124f-21bc-4857-b70e-bfaf6ce9815d.png)

![app-reg-postman-auth-16](https://user-images.githubusercontent.com/33935506/135540384-75ad2a5a-c35d-44ff-871e-de4bf3e69374.png)

```test
# STEP 6 - Create Forecasts Request

- Create a new request in Collection and configure the Authorization to inherit from parent

```

![get-forecasts-1](https://user-images.githubusercontent.com/33935506/135542437-bd6ce5c5-9070-42f7-9ca1-a4e0e8cde56b.png)

---

## Initiate Forecast Request

```text

- Open `Get All Forecasts` request
- Initiate request. You should see a list of forecasts being returned.

NOTE:

Remember that the request will inherit the authorization
that was configured for the collection

```

![get-forecasts-2](https://user-images.githubusercontent.com/33935506/135542439-f77c6f07-efc8-481d-92f2-b82ae42e0a12.png)
