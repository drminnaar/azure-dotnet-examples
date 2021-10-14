# Wonderland Weather API Azure AD App Registration

![design](https://user-images.githubusercontent.com/33935506/137100747-0bc8a06b-0fbe-4471-9781-ede7e6524c8d.png)

In this guide, we will create a new App Registration to enable authorization on the .NET 6 API.

If you're interested in seeing how to configure Postman to use Azure AD to make authorized requests, plese see the section [Configure Postman](#configure-postman).

---

## API App Registration

According to the [official Microsoft Azure documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app#register-an-application),

> Registering your application establishes a trust relationship between your app and the Microsoft identity platform. The trust is unidirectional: your app trusts the Microsoft identity platform, and not the other way around.

The following steps show how to create an App Registration for *Wonderland Weather API*.

### Steps

```text

- Switch To Appropriate Tenant

```

![app-reg-1](https://user-images.githubusercontent.com/33935506/135538347-ddda9997-9152-46ea-baec-4728db230e78.png)

```text

- Choose `App Registrations`

```

![app-reg-2](https://user-images.githubusercontent.com/33935506/135538351-1d39a5b1-1210-4010-9f38-dcc534126b4e.png)

```text

- Select `New Registration`

```

![app-reg-3](https://user-images.githubusercontent.com/33935506/135538352-abf39f66-d168-469d-9921-5276717eb089.png)

```text

- Provide basic App Registration information, and select `Register`

```

![app-reg-4](https://user-images.githubusercontent.com/33935506/135538355-c2c57211-0743-4839-ac60-cdad649e7b6c.png)

```text

- Review newly created App Registration
- Select option to `Expose an API`

```

![app-reg-5](https://user-images.githubusercontent.com/33935506/135547774-0cf5101f-f7eb-4b7a-af43-e7493841c234.png)

```text

- Take note that the initial Application ID URI has not been set yet.
- Select option to `Add a scope`

```

![app-reg-6](https://user-images.githubusercontent.com/33935506/135538357-3731b6d0-15cd-410f-a755-369d4e0a71b2.png)

```text

- Configure **_Application ID URI_**

```

![app-reg-7](https://user-images.githubusercontent.com/33935506/135547880-639d74e8-a14d-4b19-a685-3a6b230a66a3.png)

```text

- Enter scope information. Use a naming standard that makes sense in your environment. I typically use the convention of **_"Endpoint.AccessType"_** which in this instance translates to **_Forecasts.Read_**.

```

![app-reg-8](https://user-images.githubusercontent.com/33935506/135548081-e970c130-d4d0-47f1-bef5-f6297974d62d.png)

```text

- See the newly added scopes

```

  ![app-reg-9](https://user-images.githubusercontent.com/33935506/135557469-b7f85e5e-c8b8-4f0f-9c1c-142ac0d883ae.png)

```text

- We need to add the React application (created above) as a `client application`
- Select the `Add a client application` option

```

![1-expose-api](https://user-images.githubusercontent.com/33935506/137070287-ff3bae29-b8e1-4d2c-9a5d-3c00566ab3ee.png)

```text

- You will require the client application id from the React app created above
- In a separate window, open the registered app for the React application
- Copy the `Application (client) ID`

```

![2-get-app-id](https://user-images.githubusercontent.com/33935506/137070288-adc7b729-6032-4ab7-8676-a22193ccdc01.png)

```text

- Add the required `Client ID` by entering the Application (client) ID copied above
- Select all appropriate scopes

```

![3-add-client-app](https://user-images.githubusercontent.com/33935506/137070290-6bf20b25-04db-4188-9eb5-d594d46214e9.png)

```text

- When complete, you should have configuration settings resembling the following screenshot with your React App added as an `Authorised client application`

```

![4-add-client-app](https://user-images.githubusercontent.com/33935506/137070292-cc6c0c89-8484-4a6c-8baa-baf50cb44785.png)