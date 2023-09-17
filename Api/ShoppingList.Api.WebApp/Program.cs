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
using ProjectHermes.ShoppingList.Api.Repositories.Common.Services;
using ProjectHermes.ShoppingList.Api.Vault;
using ProjectHermes.ShoppingList.Api.WebApp.BackgroundServices;
using ProjectHermes.ShoppingList.Api.WebApp.Configs;
using ProjectHermes.ShoppingList.Api.WebApp.Extensions;
using Serilog;
using System.IO;

var builder = WebApplication.CreateBuilder();

builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
builder.Logging.AddConsole();
builder.Logging.AddSerilog();

IConfiguration configuration = builder.Configuration;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var fileLoadingService = new FileLoadingService();
var vaultService = new VaultService(configuration, fileLoadingService);
var configurationLoadingService = new DatabaseConfigurationLoadingService(fileLoadingService, vaultService);
var connectionStrings = await configurationLoadingService.LoadAsync(configuration);
builder.Services.AddSingleton(connectionStrings);

builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
builder.Services.AddCore();
builder.Services.AddDomain();
builder.Services.AddEndpointControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
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

app.UseCors(policyBuilder =>
{
    var corsConfig = app.Configuration.GetSection("Cors").Get<CorsConfig>();

    policyBuilder
        .WithOrigins(corsConfig.AllowedOrigins)
        .WithMethods("GET", "PUT", "POST", "DELETE")
        .WithHeaders("Content-Type");
});

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();