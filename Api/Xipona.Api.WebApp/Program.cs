using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using Xipona.Api.ApplicationServices;
using Xipona.Api.Core;
using Xipona.Api.Core.Files;
using Xipona.Api.Domain;
using Xipona.Api.Endpoint;
using Xipona.Api.Endpoint.Middleware;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Repositories;
using Xipona.Api.Secrets;
using Xipona.Api.WebApp.Auth;
using Xipona.Api.WebApp.BackgroundServices;
using Xipona.Api.WebApp.Configs;
using Xipona.Api.WebApp.Extensions;
using Xipona.Api.WebApp.Serialization;

var builder = WebApplication.CreateBuilder();
builder.Host.UseDefaultServiceProvider((_, opt) =>
{
    opt.ValidateOnBuild = true;
});
builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());

if (builder.Environment.IsEnvironment("Local"))
{
    builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
}

AddAppsettingsSourceTo(builder.Configuration.Sources);

var configuration = builder.Configuration;

var secretServices = new ServiceCollection();
secretServices.AddSingleton<IConfiguration>(configuration);
secretServices.AddTransient<IFileLoadingService, FileLoadingService>();
SecretStoreRegister.RegisterSecretStore(configuration, new FileLoadingService(), secretServices);
secretServices.AddTransient<ISecretLoadingService, SecretLoadingService>();

#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
var secretProvider = secretServices.BuildServiceProvider();
#pragma warning restore ASP0000

var secretLoadingService = secretProvider.GetRequiredService<ISecretLoadingService>();
var connectionStrings = await secretLoadingService.LoadConnectionStringsAsync();
builder.Services.AddSingleton(connectionStrings);

await builder.Services.AddOtelAsync(configuration, builder.Environment, secretLoadingService);

builder.Services.Configure<JsonOptions>(opt =>
    opt.SerializerOptions.TypeInfoResolverChain.Add(XiponaJsonSerializationContext.Default));

builder.Services.AddMemoryCache();
builder.Services.AddCore();
builder.Services.AddDomain();
builder.Services.AddEndpointConverters();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddCors();

var authOptions = new AuthenticationOptions();
configuration.GetSection("Auth").Bind(authOptions);

builder.Services.AddSingleton(authOptions);

builder.Services.AddOpenApi("v1", opt =>
{
    if (!authOptions.Enabled)
        return;

    opt.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddRepositories();
builder.Services.AddApplicationServices();
builder.Services.AddHostedService<DatabaseMigrationBackgroundService>();

SetupSecurity();

var app = builder.Build();

app.Lifetime.ApplicationStopping.Register(Diagnostics.DisposeInstance);

app.UseExceptionHandling();
if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
}

app.MapOpenApi();
app.MapScalarApiReference(opt => opt.Title = "Xipona API");

app.UseCors(policyBuilder =>
{
    var corsConfig = new CorsConfig();
    app.Configuration.Bind(corsConfig);

    policyBuilder
        .WithOrigins(corsConfig.AllowedOrigins)
        .WithMethods("GET", "PUT", "POST", "DELETE")
        .WithHeaders("Content-Type", "authorization");
});

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseDiagnosticsMiddleware();

app.RegisterUserEndpoints();
app.RegisterItemCategoryEndpoints();
app.RegisterItemEndpoints();
app.RegisterManufacturerEndpoints();
app.RegisterMonitoringEndpoints();
app.RegisterRecipeEndpoints();
app.RegisterShoppingListEndpoints();
app.RegisterRecipeTagEndpoints();
app.RegisterStoreEndpoints();

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

void SetupSecurity()
{
    if (!authOptions.Enabled)
    {
        builder.Services
            .AddAuthorizationBuilder()
            .AddPolicy("User", new AuthorizationPolicyBuilder().RequireAssertion(_ => true).Build());
        return;
    }

    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt =>
        {
            opt.Authority = authOptions.Authority;
            opt.Audience = authOptions.Audience;
            opt.TokenValidationParameters = new()
            {
                ValidTypes = authOptions.ValidTypes,
                NameClaimType = authOptions.NameClaimType,
                RoleClaimType = authOptions.RoleClaimType,
            };
        });
    builder.Services
        .AddAuthorizationBuilder()
        .AddPolicy("User", policy => policy.RequireRole(authOptions.UserRoleName));
}