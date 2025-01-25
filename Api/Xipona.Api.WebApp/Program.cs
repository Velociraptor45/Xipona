using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProjectHermes.Xipona.Api.ApplicationServices;
using ProjectHermes.Xipona.Api.Core;
using ProjectHermes.Xipona.Api.Core.Files;
using ProjectHermes.Xipona.Api.Domain;
using ProjectHermes.Xipona.Api.Endpoint;
using ProjectHermes.Xipona.Api.Endpoint.Middleware;
using ProjectHermes.Xipona.Api.Repositories;
using ProjectHermes.Xipona.Api.Vault;
using ProjectHermes.Xipona.Api.WebApp.Auth;
using ProjectHermes.Xipona.Api.WebApp.BackgroundServices;
using ProjectHermes.Xipona.Api.WebApp.Configs;
using ProjectHermes.Xipona.Api.WebApp.Extensions;
using ProjectHermes.Xipona.Api.WebApp.Serialization;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder();

builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());

if (builder.Environment.IsEnvironment("Local"))
{
    builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
}

AddAppsettingsSourceTo(builder.Configuration.Sources);

var configuration = builder.Configuration;

var fileLoadingService = new FileLoadingService();
builder.Services.AddOtel(configuration, builder.Environment, fileLoadingService);

await Configure(configuration, fileLoadingService);

//var vaultService = new VaultService(configuration, fileLoadingService);
//var configurationLoadingService =
//    new DatabaseConfigurationLoadingService(fileLoadingService, vaultService, configuration);
//var connectionStrings = await configurationLoadingService.LoadAsync();
//builder.Services.AddSingleton(connectionStrings);

builder.Services
    .AddControllers(options => options.SuppressAsyncSuffixInActionNames = false)
    .AddJsonOptions(opt => opt.JsonSerializerOptions.TypeInfoResolverChain.Add(XiponaJsonSerializationContext.Default));
builder.Services.AddCore();
builder.Services.AddDomain();
builder.Services.AddEndpointControllers();

var authOptions = new AuthenticationOptions();
configuration.GetSection("Auth").Bind(authOptions);

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Xipona API", Version = "v1" });
    opt.CustomSchemaIds(type => type.ToString());

    if (!authOptions.Enabled)
        return;

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Auth",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.OpenIdConnect,
        OpenIdConnectUrl = new Uri(authOptions.OidcUrl),
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };

    opt.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

builder.Services.AddRepositories();
builder.Services.AddApplicationServices();
builder.Services.AddHostedService<DatabaseMigrationBackgroundService>();

SetupSecurity();

var app = builder.Build();

app.Lifetime.ApplicationStopping.Register(() =>
{
    Diagnostics.DisposeInstance();
});

app.UseExceptionHandling();
if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Xipona API v1");
});

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

app.UseMiddleware<DiagnosticsMiddleware>();

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

async Task Configure(IConfiguration configuration, IFileLoadingService fileLoadingService)
{
    //var services = new ServiceCollection();
    //services.AddTransient<IFileLoadingService, FileLoadingService>();
    //services.AddTransient<IVaultService, VaultService>();

    var secrets = new Secrets();
    configuration.Bind(secrets);

    if ((!string.IsNullOrWhiteSpace(secrets.VaultUsername) || !string.IsNullOrWhiteSpace(secrets.VaultUsernameFile))
        && (!string.IsNullOrWhiteSpace(secrets.VaultPassword) || !string.IsNullOrWhiteSpace(secrets.VaultPasswordFile)))
    {
        // use vault
        var vaultCredentials = new VaultCredentials
        {
            Username = !string.IsNullOrWhiteSpace(secrets.VaultUsername)
                ? secrets.VaultUsername
                : fileLoadingService.ReadFile(secrets.VaultUsernameFile),
            Password = !string.IsNullOrWhiteSpace(secrets.VaultPassword)
                ? secrets.VaultPassword
                : fileLoadingService.ReadFile(secrets.VaultPasswordFile)
        };

        var vaultConfig = new VaultConfig();
        configuration.GetSection("KeyVault").Bind(vaultConfig, opt => opt.ErrorOnUnknownConfiguration = true);

        var vaultService = new VaultService(vaultCredentials, vaultConfig);
        var x = await vaultService.LoadDatabaseCredentialsAsync();
    }
    else
    {
        // use env
    }

    //services.AddHttpClient("vault", client => client.BaseAddress = )
}


class Secrets
{
    [ConfigurationKeyName("PH_XIPONA_VAULT_USERNAME")]
    public string VaultUsername { get; set; }

    [ConfigurationKeyName("PH_XIPONA_VAULT_USERNAME_FILE")]
    public string VaultUsernameFile { get; set; }

    [ConfigurationKeyName("PH_XIPONA_VAULT_PASSWORD")]
    public string VaultPassword { get; set; }

    [ConfigurationKeyName("PH_XIPONA_VAULT_PASSWORD_FILE")]
    public string VaultPasswordFile { get; set; }

    //[JsonPropertyName("PH_XIPONA_DB_USERNAME")]
    //public string DbUsername { get; set; }

    //[JsonPropertyName("PH_XIPONA_DB_USERNAME_FILE")]
    //public string DbUsernameFile { get; set; }

    //[JsonPropertyName("PH_XIPONA_DB_PASSWORD")]
    //public string DbPassword { get; set; }

    //[JsonPropertyName("PH_XIPONA_DB_PASSWORD_FILE")]
    //public string DbPasswordFile { get; set; }
}
