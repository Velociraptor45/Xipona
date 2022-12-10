# Project Hermes - Shopping List
## Project Description

## Components

### API
The API contains all the application's business logic.

### Frontend
The frontend can show and edit active shopping lists, items & shops. Thanks to retry and queuing logic, it is possible to use the front page while shopping even if there's currently no internet connection. When the connection is re-established, the client syncs with the API.

***

## Docker
To run all required services in containers, Dockerfiles and docker-compose files are provided. Since v0.7.0 Docker Secrets are being used and thus the services must be started via a stack deploy on a Docker Swarm. Starting via docker-compose is not supported anymore.

In general, all above descriptions apply, plus some additions specified in the following sections.
Notice: All Dockerfiles use 32-bit images as base images to be able to run on Raspberry Pis.
The ProjectHermes ShoppingList comes with the option to connecto to a Hashicorp KeyVault. The setup for it is, however, a bit complicated. That's why there's an alternative option to provide the db connection string to the api via a Docker Secret.

### Prerequisits
Prepare the following things:
- Certificates<br/>
  You need a root certificate (with .crt file ending) and generate the follwing certificates with it
  - Api
    - shoppinglist-api.crt
    - shoppinglist-api.key
    - shoppinglist-api-local.crt (only for local development)
    - shoppinglist-api-local.key (only for local development)
  - Frontend
    - shoppinglist-frontend.crt
    - shoppinglist-frontend.key
  - Vault (only if you want to use the key vault)
    - vault-cert.pem (the certificate)
    - vault-key.pem (the certificate key)
- Docker Volumes
  - Api
    - (prd/dev)-ph-shoppinglist-api-tls
    - (prd/dev)-ph-shoppinglist-api-logs
    - (prd/dev)-ph-shoppinglist-api-config
  - Frontend
    - (prd/dev)-ph-shoppinglist-frontend-config
    - (prd/dev)-ph-shoppinglist-frontend-tls
  - Vault (only if you want to use the key vault)
    - (prd/dev)-ph-shoppinglist-vault-tls
    - (prd/dev)-ph-shoppinglist-vault-file
    - (prd/dev)-ph-shoppinglist-vault-config
  - Database
    - (prd/dev)-ph-shoppinglist-database
- Docker Secrets (if you want to use the key vault)<br/>
  You can choose the username & password, but 'api' (without the ') for the username is recommeded
  - (prd/dev)-ph-shoppinglist-vault-api-username 
  - (prd/dev)-ph-shoppinglist-vault-api-password
- Docker Secrets (if you don't want to use the key vault; see [here](#key-vault) for connection string dummy)
  - (prd/dev)-ph-shoppinglist-db-connection-string

### Vault
If you don't want to use the key vault, you can skip this section.

- Move the vault.hcl file from *Docker/Files/* into the root directory of the (prd/dev)-ph-shoppinglist-vault-**config** volume
- Move the vault certificate files (vault-cert.pem & vault-key.pem) into the root directory of the (prd/dev)-ph-shoppinglist-vault-**tls** volume
- Make sure your root certificat has the file ending .crt and move it to */usr/local/share/ca-certificates/*. This will allow the api container at startup to trust your root certificate and call the key vault over https.
- Set the vault address in the api's appsettings files (*Api/ShoppingList.Api.WebApp/appsettings.\*.json*)

### Api
Copy the api certificate files (shoppinglist-api.crt & shoppinglist-api.key) into the root directory of the (prd/dev)-ph-shoppinglist-api-**tls** volume.
The appsettings file (*Api/ShoppingList.Api.WebApp/appsettings.\*.json*) will not be delivered with the docker image and must be placed inside the (prd/dev)-ph-shoppinglist-api-**config** volume.

### Frontend
- Copy the api certificate files (shoppinglist-frontend.crt & shoppinglist-frontend.key) into the root directory of the (prd/dev)-ph-shoppinglist-frontend-**tls** volume.
Configure the webserver address & the frontend's environment in shoppinglist.conf under *Frontend/Docker* and copy it into the root directory of the (prd/dev)-ph-shoppinglist-frontend-**config**.
- Copy the appsettings file (*Frontend/ShoppingList.Frontend.WebApp/wwwroot/appsettings.\*.json*) into a directory of your choice on your host and set the api's address in the files. Then, set the absolute path of said directory in the *Docker/Compose/.env* file
- Make sure your root certificate is inside your host's */usr/local/share/ca-certificates/* directory so that the frontend will be able to verify the api's certificate.

### Images
There are currently no Docker images for api and frontend provided (but are planned for the future), so you have to build them yourself. Before you do that, you have to configure some things.

#### URIs
Configure the correct api's URI in in the frontend's appsettings (under *Frontend/ShoppingList.Frontend.WebApp/wwwroot/*) and, if you're using the key vault, the correct key vault URI in the api's appsettings (under *Api/ShoppingList.Api.WebApp/*).

### yml files
Under *Docker/Compose/* are yml files for development and production. You have to 
- specify the frontend and API's Docker images that you built with the respective Dockerfiles in *Api/* and *Frontend/*
- set the DB root password

Now start the containers via e.g. `docker stack deploy --compose-file docker-compose-prd.yml prd-ph-shoppinglist`. Your Api container will probably fail because the key vault hasn't been initialized yet. This will be done in the [General Setup](#general-setup) section.

***

## Local Development Setup
To get the everything running at your dev machine, at least a running dev DB is necessary. However, it's recommended to start the whole dev stack in Docker. You'll then be able to start the api & frontend locally where the frontend connects to the api and the api to the dev database.

### API

#### Certificate
The API is running via https and needs the local certificate files (shoppinglist-api-local.crt & shoppinglist-api-local.key) in the *Api/ShoppingList.Api.WebApp/ssl/* directory (you might have to create the folder).

#### Database connection
To mimic Docker Secrets, there are two variables in the *Api/ShoppingList.Api.WebApp/Properties/launchSettings.json*: PH_SL_VAULT_USERNAME_FILE & PH_SL_VAULT_PASSWORD_FILE. Create two files with username and password to your dev key vault, respectively and specify their full absolute file path here. A normal .txt is enough. Fill the files with the same values you chose for the respective secrets during the [Prerequisits](#prerequisits) step.

If you don't want to use the key vault, create a PH_SL_DB_CONNECTION_STRING_FILE variable in the launchSettings.json file next to the other ones and specify the location of the file holding the connection string there (after you created the file). An example for the connection string can be found in the [General Setup](#general-setup) section.

### Frontend
No further actions are required

***

## General Setup
Before performing the general setup, make sure you did the local development setup and/or the Docker setup before. This step required a running DB container and a running Vault container (if you want to use the key vault).

### Key Vault
Make sure your key vault container is running and go to https://\<vault-address\>:\<vault-port\>/ui where you'll initialize the vault. Specify that the root key should be split into 1 shared key and that 1 key is needed to unseal the vault. After that, this shared key (used to unseal the vault) and the root token (used to login) are displayed. Store both of them somewhere safe and login using the Token method and your root token.
> **Caution: The key vault must be unsealed each time the vault container starts. If you redeploy the stack, the vault is automatically sealed again. There is, however, an option for automatic unseal but it's not part of the stack yet. Also, the api has a built-in retry mechanism to connect to the key vault at startup, so that you have time to unseal it.**

Create a new KV engine with path name "ph-shoppinglist-api" with a secret "ConnectionStrings" holding a key "ShoppingDatabase" with your DB connection string as the value:
> server=\<server-ip-address\>;port=\<prd-port: 15909, dev-port: 15906\>;database=(prd/dev)-shoppinglist;user id=root;pwd=\<root-pwd\>;AllowUserVariables=true;UseAffectedRows=false

Now click on "Access" in the toolbar and create a new auth method "Username & Password" under the path "userpass". In there, create a user (for the api) with the name and password you chose in the [Prerequisits](#prerequisits) step and save it.

After that, click on "Policies" in the toolbar, open the default policy, click "Edit policy" and append the following at the bottom and save it.
> path "ph-shoppinglist-api/*" {
>    capabilities = ["read", "list"]
>}

This will allow authenticated users to access the engine you just created. Now the Api will be able to connect the key vault.

### Database migration
The API follows the **code first** approach, which means that you have to deploy the provided migrations to your database. Under *Api/Scripts/* are two shell scripts that deploy the migrations to the respective database. Before you execute them, open the files and check if the PH_SL_VAULT_USERNAME_FILE and PH_SL_VAULT_PASSWORD_FILE paths are set correctly (if you're using the key vault) or the PH_SL_DB_CONNECTION_STRING_FILE is set correctly (if you're not using the key vault).

And now you're done. Happy shopping!