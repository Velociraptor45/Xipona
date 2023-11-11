# Api folder structure

The api is organized in an onion structure. This means that it consists of many layers on top of each other where the outer layers can access the inner ones but not the other way round.
This leads to a fixed dependency direction that gives confidence while developing.
In the following sections the individual layers (from outmost to innermost) and their purposes are explained.

## Execution
Execution contains all code that is required to run the API as an application, like a web server or the configuration

## Endpoints
Endpoints defines all kinds of interfaces the application has to the outside. In this case here, it's exclusively REST-endpoints.

## Client
Client contains the client for the endpoints that is passed out to any frontends for the API in order to make calling it easier.

## Contracts
Contracts contains all the object structures that can be passed into or come as a response from the endpoints.

## Application Services
Application Services is the broker beteween the domain and the endpoints.
Every request has to go through it in order to reach the domain and it contains all the technical necessities like transaction handling, scope manangement or retries.

## Integration
Integration is responsible for connecting external ressources and providing an abstraction for them to the domain. Examples can be databases, 3rd party libraries or message queues.

## Domain
Domain is the heart of the application. Here resides all of the business logic and nothing else.

## Core
Core provides basic functionalities that are needed throughout the whole application, like extension methods, certain interfaces or services.