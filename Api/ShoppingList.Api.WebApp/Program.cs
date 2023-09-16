using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.ApplicationServices;
using ProjectHermes.ShoppingList.Api.Core;
using ProjectHermes.ShoppingList.Api.Core.Files;
using ProjectHermes.ShoppingList.Api.Domain;
using ProjectHermes.ShoppingList.Api.Endpoint;
using ProjectHermes.ShoppingList.Api.Repositories;
using ProjectHermes.ShoppingList.Api.Repositories.Common.Services;
using ProjectHermes.ShoppingList.Api.Vault;
using ProjectHermes.ShoppingList.Api.WebApp.BackgroundServices;
using ProjectHermes.ShoppingList.Api.WebApp.Extensions;
using Serilog;
using System.Collections.Generic;
using System.IO;

var builder = WebApplication.CreateBuilder();

AddAppsettingsSourceTo(builder.Configuration.Sources);

builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
builder.Logging.AddConsole();
builder.Logging.AddSerilog();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var fileLoadingService = new FileLoadingService();
var vaultService = new VaultService(builder.Configuration, fileLoadingService);
var configurationLoadingService = new DatabaseConfigurationLoadingService(fileLoadingService, vaultService);
var connectionStrings = await configurationLoadingService.LoadAsync(builder.Configuration);
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
            b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); //todo #349
        });
});
builder.Services.AddRepositories();
builder.Services.AddApplicationServices();
builder.Services.AddHostedService<DatabaseMigrationBackgroundService>();

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

static void AddAppsettingsSourceTo(IList<IConfigurationSource> sources)
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var basePath = env == "Local"
        ? Directory.GetCurrentDirectory()
        : Path.Combine(Directory.GetCurrentDirectory(), "config");
    var jsonSource = new JsonConfigurationSource
    {
        FileProvider = new PhysicalFileProvider(basePath),
        Path = $"appsettings.{env}.json",
        Optional = false,
        ReloadOnChange = true
    };
    sources.Add(jsonSource);
}