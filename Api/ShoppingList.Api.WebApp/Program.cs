using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.ApplicationServices;
using ProjectHermes.ShoppingList.Api.Core;
using ProjectHermes.ShoppingList.Api.Core.Files;
using ProjectHermes.ShoppingList.Api.Domain;
using ProjectHermes.ShoppingList.Api.Endpoint;
using ProjectHermes.ShoppingList.Api.Repositories;
using ProjectHermes.ShoppingList.Api.Vault;
using ProjectHermes.ShoppingList.Api.WebApp.Extensions;
using ProjectHermes.ShoppingList.Api.WebApp.Services;
using Serilog;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

IConfiguration configuration = GetConfiguration();

var builder = WebApplication.CreateBuilder();

builder.WebHost.UseKestrel(options =>
{
    options.AddServerHeader = false;
    IPAddress ipAddress = IPAddress.Parse(configuration["Kestrel:IP-Address"]!);
    int port = int.Parse(configuration["Kestrel:Port"]!);
    options.Listen(ipAddress, port, listenOptions =>
    {
        listenOptions.UseHttps(GetCertificate(configuration));
    });
});
builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
builder.Logging.AddConsole();
builder.Logging.AddSerilog();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var fileLoadingService = new FileLoadingService();
var vaultService = new VaultService(configuration, fileLoadingService);
var configurationLoadingService = new ConfigurationLoadingService(fileLoadingService, vaultService);
var connectionStrings = await configurationLoadingService.LoadAsync();
builder.Services.AddSingleton(connectionStrings);

builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
builder.Services.AddCore();
builder.Services.AddDomain();
builder.Services.AddEndpointControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("temporary",
        b =>
        {
            b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); //todo
        });
});
builder.Services.AddRepositories();
builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseExceptionHandling();
if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingList API v1");
});

app.UseRouting();
app.UseCors("temporary");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

static IConfiguration GetConfiguration()
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var basePath = env == "Local"
        ? Directory.GetCurrentDirectory()
        : Path.Combine(Directory.GetCurrentDirectory(), "config");

    return new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddEnvironmentVariables()
        .AddJsonFile($"appsettings.{env}.json",
            optional: true, reloadOnChange: true)
        .Build();
}

static X509Certificate2 GetCertificate(IConfiguration configuration)
{
    CertificateLoadingService loadingService = new CertificateLoadingService();
    string crtFilePath = configuration["Certificate:CrtFilePath"];
    string privateKeyFilePath = configuration["Certificate:PrivateKeyFilePath"];

    return loadingService.GetCertificate(crtFilePath, privateKeyFilePath);
}