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
using ProjectHermes.Xipona.Api.ApplicationServices;
using ProjectHermes.Xipona.Api.Core;
using ProjectHermes.Xipona.Api.Core.Files;
using ProjectHermes.Xipona.Api.Domain;
using ProjectHermes.Xipona.Api.Endpoint;
using ProjectHermes.Xipona.Api.Endpoint.Middleware;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Repositories;
using ProjectHermes.Xipona.Api.Secrets;
using ProjectHermes.Xipona.Api.WebApp.Auth;
using ProjectHermes.Xipona.Api.WebApp.BackgroundServices;
using ProjectHermes.Xipona.Api.WebApp.Configs;
using ProjectHermes.Xipona.Api.WebApp.Extensions;
using ProjectHermes.Xipona.Api.WebApp.Serialization;
using Scalar.AspNetCore;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;

var builder = WebApplication.CreateBuilder();

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

builder.Services.AddCore();
builder.Services.AddDomain();
builder.Services.AddEndpointConverters();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddCors();

var authOptions = new AuthenticationOptions();
configuration.GetSection("Auth").Bind(authOptions);

builder.Services.AddOpenApi();

//builder.Services.AddSwaggerGen(opt =>
//{
//    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Xipona API", Version = "v1" });
//    opt.CustomSchemaIds(type => type.ToString());

//    if (!authOptions.Enabled)
//        return;

//    var securityScheme = new OpenApiSecurityScheme
//    {
//        Name = "Auth",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.OpenIdConnect,
//        OpenIdConnectUrl = new Uri(authOptions.OidcUrl),
//        Scheme = "bearer",
//        BearerFormat = "JWT",
//        Reference = new OpenApiReference
//        {
//            Id = "Bearer",
//            Type = ReferenceType.SecurityScheme
//        }
//    };

//    opt.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
//    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        { securityScheme, Array.Empty<string>() }
//    });
//});

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
app.MapScalarApiReference(); // TODO auth?

//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Xipona API v1");
//});

app.UseCors(policyBuilder =>
{
    var corsConfig = new CorsConfig();
    // throwing does atm not work https://github.com/dotnet/runtime/issues/98231
    app.Configuration.GetSection("Cors").Bind(corsConfig, opt => opt.ErrorOnUnknownConfiguration = true);

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