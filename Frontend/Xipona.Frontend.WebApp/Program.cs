using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using Serilog.Core;
using Xipona.Api.Client;
using Xipona.Frontend.Infrastructure;
using Xipona.Frontend.Infrastructure.Connection;
using Xipona.Frontend.Redux;
using Xipona.Frontend.Redux.Shared.Configurations;
using Xipona.Frontend.Redux.Shared.Ports;
using Xipona.Frontend.WebApp.Auth;
using Xipona.Frontend.WebApp.Configs;
using Xipona.Frontend.WebApp.Services;
using Xipona.Frontend.WebApp.Services.Notification;
using Xipona.Frontend.WebApp.Services.Prices;

namespace Xipona.Frontend.WebApp;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        await LoadVariables(builder);
        
        var authConfig = new AuthConfig();
        builder.Configuration.Bind(authConfig);
        builder.Services.AddSingleton(authConfig);
        AddSecurity(builder, authConfig);

        ConfigureHttpClient(builder, authConfig);
        ConfigureLogging(builder);

        AddDependencies(builder);
        builder.Services.AddAntDesign();

        await builder.Build().RunAsync();
    }

    private static async Task LoadVariables(WebAssemblyHostBuilder builder)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        var responseMessage = await client.GetAsync("variables.json").ConfigureAwait(false);
        var stream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);

        builder.Configuration.AddJsonStream(stream);
    }

    private static void ConfigureHttpClient(WebAssemblyHostBuilder builder, AuthConfig authConfig)
    {
        var connectionConfig = new ConnectionConfig();
        builder.Configuration.Bind(connectionConfig);

        builder.Services.AddSingleton(connectionConfig);

        if (string.IsNullOrWhiteSpace(connectionConfig.ApiUri))
            throw new InvalidOperationException("The Api-Url is missing in the configuration");

        var uri = new Uri(connectionConfig.ApiUri);
        var httpClientBuilder = builder.Services.AddHttpClient("Api", client => client.BaseAddress = uri);

        if (authConfig.Enabled)
        {
            builder.Services.AddScoped<CustomAddressAuthorizationMessageHandler>();
            httpClientBuilder.AddHttpMessageHandler<CustomAddressAuthorizationMessageHandler>();
        }

        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Api"));
    }

    private static void ConfigureLogging(WebAssemblyHostBuilder builder)
    {
        var config = new CollectRemoteLogsConfig();
        builder.Configuration.Bind(config);

        if (!config.Enabled)
            return;

        var endpointUrl = config.HostUrl.EndsWith('/')
            ? $"{config.HostUrl}ingest"
            : $"{config.HostUrl}/ingest";

        var levelSwitch = new LoggingLevelSwitch();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(levelSwitch)
            .Enrich.WithProperty("Env", builder.HostEnvironment.Environment)
            .Enrich.WithProperty("InstanceId", Guid.NewGuid().ToString("D"))
            .WriteTo.BrowserHttp(endpointUrl: endpointUrl, controlLevelSwitch: levelSwitch)
            .CreateLogger();

        builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
    }

    private static void AddDependencies(WebAssemblyHostBuilder builder)
    {
        var shoppingListConfig = new ShoppingListConfiguration()
        {
            SearchDelayAfterInput = TimeSpan.FromMilliseconds(300),
            HideItemsDelay = TimeSpan.FromMilliseconds(1000)
        };
        builder.Services.AddSingleton(shoppingListConfig);

        var commandQueueConfig = new CommandQueueConfig()
        {
            ConnectionRetryInterval = TimeSpan.FromSeconds(4)
        };
        builder.Services.AddSingleton(commandQueueConfig);

        builder.Services.AddTransient<IXiponaApiClient, XiponaApiClient>();
        builder.Services.AddTransient<IShoppingListNotificationService, ShoppingListNotificationService>();
        builder.Services.AddTransient<IPriceLabelService, PriceLabelService>();
        builder.Services.AddTransient<IApiClient, ApiClient>();
        builder.Services.AddScoped<ICommandQueue, CommandQueue>();

        builder.Services.AddScoped<IItemPriceCalculationService, ItemPriceCalculationService>();

        builder.Services.AddSingleton(TimeProvider.System);

        builder.Services.AddInfrastructure();

        builder.Services.AddRedux();

        builder.Services.AddBlazoredLocalStorage();
    }

    private static void AddSecurity(WebAssemblyHostBuilder builder, AuthConfig authConfig)
    {
        if (!authConfig.Enabled)
        {
            builder.Services.AddOidcAuthentication(_ => { });
            builder.Services.AddAuthorizationCore(cfg =>
            {
                cfg.AddPolicy("User", policy => policy.RequireAssertion(_ => true));
            });
            return;
        }

        builder.Services.AddOidcAuthentication(opt =>
        {
            opt.ProviderOptions.Authority = authConfig.Authority;
            opt.ProviderOptions.MetadataUrl = $"{opt.ProviderOptions.Authority}/.well-known/openid-configuration";
            opt.ProviderOptions.ClientId = authConfig.ClientId;
            opt.ProviderOptions.ResponseType = authConfig.ResponseType;
            foreach (var scope in authConfig.DefaultScopes)
                opt.ProviderOptions.DefaultScopes.Add(scope);

            opt.UserOptions.NameClaim = authConfig.NameClaimIdentifier;
            opt.UserOptions.RoleClaim = authConfig.RoleClaimIdentifier;
            opt.UserOptions.ScopeClaim = authConfig.ScopeClaimIdentifier;

        }).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

        builder.Services.AddAuthorizationCore(cfg =>
        {
            cfg.AddPolicy("User", new AuthorizationPolicyBuilder()
                .RequireRole(authConfig.UserRoleName)
                .Build());
        });
    }

    private sealed class CollectRemoteLogsConfig
    {
        [ConfigurationKeyName("XIPONA_LOGS_ENABLED")]
        public bool Enabled { get; init; } = false;

        [ConfigurationKeyName("XIPONA_LOGS_HOST_URL")]
        public string HostUrl { get; init; } = string.Empty;
    }
}