# Xipona Api Client

This package provides a .net client for accessing the [Xipona](https://github.com/Velociraptor45/Xipona) API. It contains all current API endpoints and their incoming & outgoing contract structures.

In order to use it in your project, instantiate a new `XiponaApiClient` or add the `IXiponaApiClient` to your DI:
```cs
services.AddTransient<IXiponaApiClient, XiponaApiClient>()
```