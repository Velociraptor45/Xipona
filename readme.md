# Project Hermes - Shopping List
## Project Description

## Components

***

## API
The API contains all the application's business logic.

### Setup
To get the API running, a couple of steps are necessary

#### Certificate

The API is running via https and needs a shoppinglist-api.crt and shoppinglist-api.key file in the *Api/ShoppingList.Api.WebApp/ssl/* directory (you might have to create the folder).

> Hint: This folder must also be present in the *ShoppingList.Api.WebApp.dll*'s directory if you deploy the application

#### Database setup
The API follows the **code first** approach, which means that you have to deploy the provided migrations to your database. Specify the connection string of your database in ShoppingContextFactory.cs under *Api/ShoppingList.Api.Infrastructure/* and open a terminal in this directory. Now execute `dotnet ef database update`. If this command fails, you might not have the ef tools installed.

#### Database connection
Specify the DB's connection string in the Development/Production appsettings under *Api/ShoppingList.Api.WebApp/*

***

## Frontend
The frontend/client can show and edit active shopping lists. Thanks to retry and queuing logic, it is possible to use the front page while shopping even if there's currently no internet connection. When the connection is re-established, the client syncs with the API.

### Setup
#### Certificate
A new folder *Frontend/ssl/* must be created, containing two files:
- shoppinglist-app.crt
- shoppinglist-app.key

***

## Docker
To run all required services in containers, Dockerfiles and docker-compose files are provided. In general, all above descriptions apply, plus some additions specified in the following sections.
Notice: All Dockerfiles use 32-bit images as base images to be able to run on Raspberry Pis.

### Api
No further actions are required.

### Frontend
#### Webserver configuration
The webserver address must be configured in shoppinglist.conf under *Frontend/*. Currently, 192.168.178.92 is registered which must be changed to your webserver's ip address.

#### Dockerfile
The Dockerfile expects the file rootCA.crt (your root certificate so that the API's certificate can be trusted) in *Frontend/ssl*.

#### Blazor WASM setup
Blazor WASM is not able to load appsettings based on the environment it runs in. Thus, you have to specify the environment before building the application.
The place to do this is in index.html under *Frontend/ShoppingList.Frontend.WebApp/wwwroot/*. Change `'blazor-environment': 'Development'` to 'Production' or leave it as it is, depending on your needs.

### docker-compose
Under *Docker/Compose/* are compose files for development and production. You have to
- specify the database password you provided in the API's appsettings
- create the database's Docker volume specified in the compose file (for prd is 'prd-ph-shoppinglist-database')
- specify the frontend and API's Docker images that you built with the respective Dockerfiles in *Api/* and *Frontend/* (official image releases via DockerHub are planned but not here yet)

Now start the containers via e.g. `docker-compose -f docker-compose-prd.yml -p prd-ph-shoppinglist up -d`.