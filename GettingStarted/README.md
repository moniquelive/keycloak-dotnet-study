# Getting Started with Keycloak ASP.NET Core

This sample demonstrates how to use the [Keycloak ASP.NET Core](https://github.com/keycloak/keycloak-aspnetcore)
library to protect an ASP.NET Core application.

## Setup

1. Create a new ASP.NET Core project

  ```bash
  dotnet new web -n GettingStarted
  ```

2. Add the `Keycloak Authorization Services` packages to the project

  ```bash
  dotnet add package Keycloak.AuthServices.Common
  dotnet add package Keycloak.AuthServices.Authentication
  dotnet add package Keycloak.AuthServices.Authorization
  ```

3. References

  - videos
    - dotnet: https://youtu.be/teNwToJC9OU
    - java: https://youtu.be/wgdo5I53GQo

  - articles
    - dotnet step by step in
      pt-br: https://dev.to/gloinho/autenticacao-e-autorizacao-de-uma-asp-net-web-api-com-keycloak-loe ([github repo](https://github.com/gloinho/mordor-api))
    - simple .net
      project: https://github.com/NikiforovAll/keycloak-authorization-services-dotnet/tree/main/samples/AuthGettingStarted
    - create user: https://www.stefaanlippens.net/keycloak-programmatically-create-clients-and-users.html
    - keycloak config example: https://nikiforovall.blog/keycloak-authorization-services-dotnet/configuration/configuration-keycloak.html

  - documentation
    - keycloak: https://www.keycloak.org
      - server admin: https://www.keycloak.org/docs/latest/server_admin/index.html
      - server developer: https://www.keycloak.org/docs/latest/server_development/index.html
      - server api: https://www.keycloak.org/docs-api/latest/rest-api/index.html

    - dotnet library: https://nikiforovall.blog/keycloak-authorization-services-dotnet/