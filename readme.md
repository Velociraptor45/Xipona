# Project Hermes - Shopping List

[![Docker Image Version (latest semver)](https://img.shields.io/docker/v/velocir4ptor/ph-shoppinglist-api?color=blue&label=docker%20image%20api&sort=semver)](https://hub.docker.com/repository/docker/velocir4ptor/ph-shoppinglist-api)
[![Docker Image Version (latest semver)](https://img.shields.io/docker/v/velocir4ptor/ph-shoppinglist-frontend?color=blue&label=docker%20image%20frontend&sort=semver)](https://hub.docker.com/repository/docker/velocir4ptor/ph-shoppinglist-frontend)

## Project Description

## Components

### API
The API contains all the application's business logic.

### Frontend
The frontend can show and edit active shopping lists, items & shops. Thanks to retry and queuing logic, it is possible to use the front page while shopping even if there's currently no internet connection. When the connection is re-established, the client syncs with the API.

## Docker
To run all required services in containers, Dockerfiles and docker-compose files are provided. Since v0.7.0 Docker Secrets are being used and thus the services must be started via a stack deploy on a Docker Swarm. Starting via docker-compose is not supported anymore.

In general, all above descriptions apply, plus some additions specified in the following sections.
The ProjectHermes ShoppingList comes with the option to connect to to a Hashicorp KeyVault. The setup for it is, however, a bit complicated. That's why there's an alternative option to provide the db connection string to the api via a Docker Secret.

### Prerequisits
Prepare the following things:
- Certificates
  - Vault (only if you want to use the key vault)
    - vault-cert.pem (the certificate)
    - vault-key.pem (the certificate key)
- Docker Volumes
  - Api
    - (prd/dev)-ph-shoppinglist-api-logs
    - (prd/dev)-ph-shoppinglist-api-config
  - Frontend
    - (prd/dev)-ph-shoppinglist-frontend-config
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
- Docker Secrets (no matter if you're using the key vault or not)
  - (prd/dev)-ph-shoppinglist-db-root-pwd

### Vault
If you don't want to use the key vault, you can skip this section.

- Move the vault.hcl file from *Docker/Files/* into the root directory of the (prd/dev)-ph-shoppinglist-vault-**config** volume
- Move the vault certificate files (vault-cert.pem & vault-key.pem) into the root directory of the (prd/dev)-ph-shoppinglist-vault-**tls** volume
- Make sure your root certificat has the file ending .crt and move it to */usr/local/share/ca-certificates/*. This will allow the api container at startup to trust your root certificate and call the key vault over https.
- Set the vault address in the api's appsettings files (*Api/ShoppingList.Api.WebApp/appsettings.\*.json*)

### Api
- The appsettings file (*Api/ShoppingList.Api.WebApp/appsettings.\*.json*) will not be delivered with the docker image and must be placed inside the (prd/dev)-ph-shoppinglist-api-**config** volume.

### Frontend
- Configure the webserver address & the frontend's environment in shoppinglist.conf under *Frontend/Docker* and copy it into the root directory of the (prd/dev)-ph-shoppinglist-frontend-**config**.
- Copy the appsettings file (*Frontend/ShoppingList.Frontend.WebApp/wwwroot/appsettings.\*.json*) into a directory of your choice on your host and set the api's address in the files.

### yml files
- Under *Docker/Compose/* are yml files for development and production. You have to replace the `{CONFIG_FOLDER_PATH}` placeholder with the absolute path of the directory where your frontend's appsettings-files are
- Start the containers via e.g. `docker stack deploy --compose-file docker-compose-prd.yml prd-ph-shoppinglist`. If you use the key vault, your Api container will probably fail because the key vault hasn't been initialized yet. This will be done in the [General Setup](#general-setup) section.

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

## Local Development Setup
To get the everything running at your dev machine, at least a running dev DB is necessary. However, it's recommended to start the whole dev stack in Docker. You'll then be able to start the api & frontend locally where the frontend connects to the api and the api to the dev database.

### API

#### Database connection
To mimic Docker Secrets, there are two variables in the *Api/ShoppingList.Api.WebApp/Properties/launchSettings.json*: PH_SL_VAULT_USERNAME_FILE & PH_SL_VAULT_PASSWORD_FILE. If you want to use the key vault, create two files with username and password to your dev key vault, respectively and specify their full absolute file path here. A normal .txt is enough. Fill the files with the same values you chose for the respective secrets during the [Prerequisits](#prerequisits) step.

If you don't want to use the key vault, create a PH_SL_DB_CONNECTION_STRING_FILE variable in the launchSettings.json file and remove the other two and specify the location of the file holding the connection string there (after you created the file). An example for the connection string can be found in the [General Setup](#general-setup) section.

## General Setup
Before performing the general setup, make sure you did the the Docker setup before. This step requires a running DB container and a running Vault container (if you want to use the key vault).

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
The API follows the **code first** approach, which means that you have to deploy the provided migrations to your database. Under *Api/Scripts/* are two shell scripts that deploy the migrations to the respective database. Before you execute them, open the `set-env-variables-(prd/dev).sh` file and check if the PH_SL_VAULT_USERNAME_FILE and PH_SL_VAULT_PASSWORD_FILE paths are set correctly (if you're using the key vault) or the PH_SL_DB_CONNECTION_STRING_FILE is set correctly (if you're not using the key vault). Execute the respective `set-env-variables-(prd/dev).sh` before the `database-update-(prd/dev).sh`.

And now you're done. Happy shopping!