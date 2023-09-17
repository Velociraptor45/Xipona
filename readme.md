# Project Hermes - Shopping List

[![Docker Image Version (latest semver)](https://img.shields.io/docker/v/velocir4ptor/ph-shoppinglist-api?color=blue&label=docker%20image%20api&sort=semver)](https://hub.docker.com/repository/docker/velocir4ptor/ph-shoppinglist-api)
[![Docker Image Version (latest semver)](https://img.shields.io/docker/v/velocir4ptor/ph-shoppinglist-frontend?color=blue&label=docker%20image%20frontend&sort=semver)](https://hub.docker.com/repository/docker/velocir4ptor/ph-shoppinglist-frontend)

## Project Description

## Components

### API
The API contains all the application's business logic.

### Frontend
The frontend can show and edit active shopping lists, items & shops. Thanks to retry and queuing logic, it is possible to use the front page while shopping even if there's currently no internet connection. When the connection is re-established, the client syncs with the API.

## Setup in Docker
To run all required services in containers, Dockerfiles and docker-compose files are provided. Since v0.7.0 Docker Secrets are being used and thus the services must be started via a stack deploy on a Docker Swarm. Starting via docker-compose is not supported anymore.

### Prerequisits
Prepare the following things:
- Docker Volumes
  - Api
    - (prd/dev)-ph-shoppinglist-api-logs
    - (prd/dev)-ph-shoppinglist-api-config
  - Frontend
    - (prd/dev)-ph-shoppinglist-frontend-config
  - Database
    - (prd/dev)-ph-shoppinglist-database
- Docker Secrets
  - (prd/dev)-ph-shoppinglist-db-username
  - (prd/dev)-ph-shoppinglist-db-password
  - (prd/dev)-ph-shoppinglist-db-root-pwd

### Api
- The appsettings file (*Api/ShoppingList.Api.WebApp/appsettings.\*.json*) will not be delivered with the docker image and must be placed inside the (prd/dev)-ph-shoppinglist-api-**config** volume. Specify the following things there:
  - The DB's address and port
  - The frontend's address as an allowed origin for CORS (e.g. https://localhost:5000)

### Frontend
- Configure the webserver address & the frontend's environment in shoppinglist.conf under *Frontend/Docker* and copy it into the root directory of the (prd/dev)-ph-shoppinglist-frontend-**config**.
- Set the api's address in the respective appsettings file (*Frontend/ShoppingList.Frontend.WebApp/wwwroot/appsettings.\*.json*) and copy it into a directory of your choice on your host.

### yml files
- Under *Docker/Compose/* are yml files for development and production. You have to replace the `{CONFIG_FOLDER_PATH}` placeholder with the absolute path of the directory where your frontend's appsettings file is
- Start the containers via e.g. `docker stack deploy --compose-file docker-compose-prd.yml prd-ph-shoppinglist`

And now you're done. Happy shopping!

## Optional Setup

### https
If you don't want to run the application behind a reverse proxy that handles the certificate for you, you can also configure the application for https.

#### Api
1. Create the docker volume (prd/dev)-ph-shoppinglist-api-**tls** and uncomment the line in the docker compose file where it's mapped as a volume.
2. Generate the certificate and copy the files (\<cert-name\>.crt & \<cert-key-name\>.key) into the root directory of the (prd/dev)-ph-shoppinglist-api-**tls** volume.
3. Replace the existing kestrel http endpoint in your `appsettings.{env}.json` with an https configuration like the following or [any other valid one](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel/endpoints?view=aspnetcore-7.0#replace-the-default-certificate-from-configuration). Just make sure the certificate's folder matches the one to which the tls volume is mapped (Default: ssl).
    ```
    "Kestrel": {
      "Endpoints": {
        "HttpsInlineCertAndKeyFile": {
          "Url": "https://localhost:5002",
          "Certificate": {
            "Path": "ssl/<cert-name>.crt",
            "KeyPath": "ssl/<cert-key-name>.key"
          }
        }
      }
    }
    ```

#### Frontend

1. Create the docker volume (prd/dev)-ph-shoppinglist-frontend-**tls** and uncomment the line in the docker compose file where it's mapped as a volume.
2. Generate the certificate and copy the files (\<cert-name\>.crt & \<cert-key-name\>.key) into the root directory of the (prd/dev)-ph-shoppinglist-frontend-**tls** volume.
3. Replace the `shoppinglist.conf` (under *Frontend/Docker*) with:
    ```
    server {
        listen 80 default_server;
        server_name <webserver-address>; # set your webserver address here (without port)
        return 301 https://$server_name$request_uri;
    }

    server {
        listen 443 ssl;
        server_name <webserver-address>; # set your webserver address here (without port)
        
        ssl_certificate /etc/nginx/ssl/<cert-name>.crt;
        ssl_certificate_key /etc/nginx/ssl/<cert-key-name>.key;

        add_header blazor-environment "Development"; # set this to Development or Production

        location / {
            root /usr/share/nginx/html/wwwroot;
            index index.html index.htm;
        }
    }
    ```

### Key Vault
Instead of providing the database credentials via docker secrets, it's also possible to retrieve them from a [HashiCorp Vault](https://www.vaultproject.io/). To do so, you need the following setup (this assumes that you already have a running Vault):

- Remove the api's two DB environment variables (PH_SL_DB_USERNAME_FILE & PH_SL_DB_PASSWORD_FILE) and both username/password docker secrets from the docker compose file
- Create new docker secrets that contain the username/password with which the api will authenticate agains the vault:
  - (prd/dev)-ph-shoppinglist-vault-api-username 
  - (prd/dev)-ph-shoppinglist-vault-api-password
- Import both secrets in the docker compose file and replace the api's two DB environment variables with
  - PH_SL_VAULT_USERNAME_FILE: /run/secrets/(prd/dev)-ph-shoppinglist-vault-api-username
  - PH_SL_VAULT_PASSWORD_FILE: /run/secrets/(prd/dev)-ph-shoppinglist-vault-api-password
- Set the vault's URI in the api's appsettings files (*Api/ShoppingList.Api.WebApp/appsettings.\*.json*)
- The default mount point ((prd/dev)-ph-shoppinglist) & secret name (database) are defined in the same appsettings file and can be changed at will. But the key names inside the secret must be "username" and "password" (all lowercase) and can not be changed. Define the username and password of the user with which you want to authenticate against the database.

## Local Development Setup
To get everything running at your dev machine, at least a running dev DB is necessary. However, it's recommended to start the whole dev stack in Docker. You'll then be able to start the api & frontend locally where the frontend connects to the api and the api to the dev database.

### API

#### Database connection
To mimic Docker Secrets, there are two variables in the *Api/ShoppingList.Api.WebApp/Properties/launchSettings.json*: PH_SL_DB_USERNAME_FILE & PH_SL_DB_PASSWORD_FILE. Create two files with only username and password respectively and specify their full absolute file path in mentioned variables. A normal .txt is enough. [If you want to use the Vault, create PH_SL_VAULT_USERNAME_FILE & PH_SL_VAULT_PASSWORD_FILE variables instead in the launchSettings.json file, remove the other two and specify the location of the files holding the key vault username & password. Then, set the Vault's URI in the *Api/ShoppingList.Api.WebApp/appsettings.Local.json*.]

Also, set the DB's address and port in your *Api/ShoppingList.Api.WebApp/appsettings.Local.json*.