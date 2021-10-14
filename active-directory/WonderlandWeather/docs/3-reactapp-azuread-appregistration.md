# Wonderland Weather React Application Azure AD App Registration

![design](https://user-images.githubusercontent.com/33935506/137100747-0bc8a06b-0fbe-4471-9781-ede7e6524c8d.png)

In this guide, we will create a new App Registration to enable authorization on the React application.

---

## App Registration

According to the [official Microsoft Azure documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app#register-an-application),

> Registering your application establishes a trust relationship between your app and the Microsoft identity platform. The trust is unidirectional: your app trusts the Microsoft identity platform, and not the other way around.

The following steps show how to create an App Registration for *Wonderland Weather API*.

### Steps

```text

- In your Azure AD Tenant, select the `App Registrations` option from the side menu
- Select `New Registration` option

```

![app-reg-1](https://user-images.githubusercontent.com/33935506/135735830-ca2f822e-91b5-4d5f-80a7-73773ce278eb.png)

```text

- Provide App Registration information
- Ensure that you select "Single-page application (SPA) and provide a redirect URI that matches the address of the React application
- Select `Register` button

```

![app-reg-2](https://user-images.githubusercontent.com/33935506/135735831-533f67cc-515a-4f40-9114-59b46ff6fd45.png)

```text

- After App Registration is complete, you are direcrect to the Overview of the newly registered application
- Take note of the Application (client) ID and Directory (tenant) ID as they will be used when we configure our React application

```

![app-reg-3](https://user-images.githubusercontent.com/33935506/135735833-0321f40b-a453-4f64-b8b4-da04c2604952.png)

```text

- Select the `Authentication` option from the side menu
- Ensure that redirect URI (http//:localhost:3000) is provided
- Ensure that none of the token options are selected
- Click `Save` if you make any changes

```

![app-reg-4](https://user-images.githubusercontent.com/33935506/135735834-1bb8f32d-60ee-4827-9b7b-796f1b29b4d8.png)

```text

- Select the `API Permissions` option from side menu. Select the option `Grant admin consent for Wonderland Weather`. This will authorize our React application to make calls to API's like Microsoft Graph.

```

![app-reg-5](https://user-images.githubusercontent.com/33935506/135735835-00fda1dc-0e90-42e5-9828-09b4602ba93f.png)
![app-reg-6](https://user-images.githubusercontent.com/33935506/135735836-84c223d7-e1bf-476c-b7e1-9d6201efa4ba.png)

```text

- The status of permissions consent should show as "Granted"

```

![app-reg-7](https://user-images.githubusercontent.com/33935506/135735837-be20eba7-7952-4017-93d6-0719d2ae92ab.png)
