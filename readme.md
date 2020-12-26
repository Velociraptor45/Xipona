# Project Hermes - Shopping List
## Project Description

## Components

***

## API
The API contains all the application's business logic.

### Setup
To get the API running, a couple of steps are necessary

#### Certificate

The API is running via https and needs a shoppinglist-api.crt and shoppinglist-api.key file in the *Api/ShoppingList.Api.WebApp/ssl* directory (you might have to create the folder).

> Hint: This folder must also be present in the *ShoppingList.Api.WebApp.dll*'s directory if you deploy the application

> Hint 2: You might want to change the API's default port (443) in the .UseKestrel options in *Api/ShoppingList.Api.WebApp/Program.cs* for local debugging

#### Database setup
The API follows the **code first** approach, which means that you have to create migrations to set up your database.
Create a migration in *Api/ShoppingList.Api.Infrastructure* via `dotnet ef migrations add <migration-name>` 
and deploy it to your database via `dotnet ef database update`.

#### Database connection
Specify the DB's connection string in the Development/Production appsettings under *./ShoppingList.Api.WebApp*

***

## Client/Frontend
The client can show and edit active shopping lists. Thanks to retry and queuing logic, it is possible to use the front page while shopping even if there's currently no internet connection. When the connection is re-established, the client syncs with the API.

### Setup
#### Certificate
A new folder *Frontend/ssl* must be created, containing two files:
- shoppinglist-app.crt
- shoppinglist-app.key