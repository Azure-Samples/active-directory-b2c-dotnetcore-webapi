---
services: active-directory-b2c
platforms: dotnet
author: dstrockis
---

# An ASP.NET Core web API with Azure AD B2C 
This sample shows how to build a web API with Azure AD B2C using the ASP.Net Core JWT Bearer middleware.  It assumes you have some familiarity with Azure AD B2C.  If you'd like to learn all that B2C has to offer, start with our documentation at [aka.ms/aadb2c](http://aka.ms/aadb2c). 

The app is a simple web API that exposes standard CRUD operations via /api/Values through standard GET, PUT, POST and  DELETE.

## How To Run This Sample

Getting started is simple! To run this sample you will need:

- To install .NET Core 2.0 for Windows by following the instructions at [dot.net/core](http://dot.net/core), which will include Visual Studio 2017.
- An Internet connection
- An Azure subscription (a free trial is sufficient)

### Step 1:  Clone or download this repository

From your shell or command line:

```powershell
git clone https://github.com/Azure-Samples/active-directory-b2c-dotnetcore-webapi.git
```

### [OPTIONAL] Step 2: Get your own Azure AD B2C tenant

You can also modify the sample to use your own Azure AD B2C tenant.  First, you'll need to create an Azure AD B2C tenant by following [these instructions](https://azure.microsoft.com/documentation/articles/active-directory-b2c-get-started).

> *IMPORTANT*: if you choose to perform one of the optional steps, you have to perform ALL of them for the sample to work as expected.

### [OPTIONAL] Step 3: Create your own policies

This sample is configured with a single policy. Which policy is configured is not relevant as it is only used to obtain the metadata to validate the token. By default, this metadata is the same for all policies. Create a policy by following [the instructions here](https://azure.microsoft.com/documentation/articles/active-directory-b2c-reference-policies).

If you already have an existing policy in your Azure AD B2C tenant, feel free to re-use it. No need to create a new one just for this sample.

### [OPTIONAL] Step 4: Create your own Web API

You will need to [register your Web API with Azure AD B2C](https://docs.microsoft.com/azure/active-directory-b2c/active-directory-b2c-app-registration#register-a-web-api) and define the scopes that client applications can request access tokens for. 

Your web API registration should include the following information:

- Enable the **Web App/Web API** setting for your application.
- Enter any **Reply URL**, as indicated previously, because the web API only does token validation and does not obtain tokens, this isn't really required. For example `https://myapi`.
- Make sure you also provide a **AppID URI**, for example `demoapi`, this is used to construct the scopes that are configured in you single page application's code.
- (Optional) Once you're app is created, open the app's **Published Scopes** blade and add any extra scopes you want.
- Copy the **Application ID** generated for your application and **Published Scopes values**, so you can input them in your application's code.

### [OPTIONAL] Step 5: Configure the sample with your app coordinates

1. Open the solution in Visual Studio.
1. Open the `appsettings.json` file.
1. Find the assignment for `Tenant` and replace the value with your tenant name.
1. Find the assignment for `ClientID` and replace the value with the Application ID from Step 4.
1. Find the assignment for `Policy` and replace the value with the name of the policy from Step 3.

### Step 6: Run the sample

Clean the solution, rebuild the solution, and run it. You can now sign up & sign in to your application using the accounts you configured in your respective policies.

## About the code

Here there's a quick guide to the most interesting authentication related bits of the sample.

### Token validation
As it is standard practice for ASP.NET Core Web APIs, the token validation functionality is implemented with the JWT Bearer middleware. Here there's a relevant snippet from the middleware initialization:  

```csharp
  services.AddAuthentication(options =>
  { 
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; 
  })
  .AddJwtBearer(jwtOptions =>
  {
    jwtOptions.Authority = $"https://login.microsoftonline.com/tfp/{Configuration["AzureAdB2C:Tenant"]}/{Configuration["AzureAdB2C:Policy"]}/v2.0/";
    jwtOptions.Audience = Configuration["AzureAdB2C:ClientId"];
    jwtOptions.Events = new JwtBearerEvents
    {
      OnAuthenticationFailed = AuthenticationFailed
    };
  });
```
Important things to notice:
- The Authority points is constructed using the **tfp** path, the tenant name and the policy.
- The OnAuthenticationFailed notification is to print better error messages if you have issues configuring this sample. You should not use this handler in production.

### Scope validation
Optionally, you can enforce more granular access control via **scopes**.

You can access codes via:

```csharp
  var scopes = HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/scope")?.Value;
  if (!string.IsNullOrEmpty(TheScope) && scopes != null && scopes.Split(' ').Any(s => s.Equals(TheScope)))
      // Do stuff
  else 
      // Unauthorized
```

Scopes are space delimited, so use a string split and check that the scope you are want is present via a .Equals. Beware of using .Contains as that will cause issues if you have two scopes like: `read` and `denyread`. Contains on read will also match denyread.
