# Project Hermes - Shopping List
## Project Description

## Components

***

## API
The API contains all the application's business logic.

### Setup
To get the API running, a couple of steps are necessary

#### Certificate
The API is running via https and thus needs a .pfx certificate in the *Api/ShoppingList.Api.WebApp* directory.
To create a new certificate (under linux), execute this command

`sudo openssl req -x509 -days 365 -newkey rsa:2048 -keyout shoppinglist-api.key -out shoppinglist-api.crt`

and to export the certificate into .pfx format execute

`sudo openssl pkcs12 -export -out shoppinglist-api.pfx -inkey shoppinglist-api.key -in shoppinglist-api.crt`.

Additionally, you have to specify the certificate's name and passphrase in *Api/ShoppingList.Api.WebApp/certificate.json*.

> Hint: This certificate must also be present in the *ShoppingList.Api.WebApp.dll*'s directory if you deploy the application

> Hint 2: You might want to change the API's default port (443) in the .UseKestrel options in *Api/ShoppingList.Api.WebApp/Program.cs* for local debugging

#### Database setup
The API follows the **code first** approach, which means that you have to create migrations to set up your database.
Create a migration in *Api/ShoppingList.Api.Infrastructure* via `dotnet ef migrations add <migration-name>` 
and deploy it to your database via `dotnet ef database update`.

#### Database connection
Specify the DB's connection string in the Development/Production appsettings under ./ShoppingList.Api.WebApp

***

## Client/Frontend
The client can show and edit active shopping lists. Thanks to retry and queuing logic, it is possible to use the front page while shopping even if there's currently no internet connection. When the connection is re-established, the client syncs with the API.

### Setup
#### Certificate
A new folder *Frontend/ssl* must be created, containing two files:
- shoppinglist-app.crt
- shoppinglist-app.key