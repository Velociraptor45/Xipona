using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Client;
using ProjectHermes.ShoppingList.Frontend.Infrastructure;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Redux;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Configurations;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.WebApp.Auth;
using ProjectHermes.ShoppingList.Frontend.WebApp.Configs;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification;
using Serilog;
using Serilog.Core;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            var authConfig = builder.Configuration.GetSection("Auth").Get<AuthConfig>();
            AddSecurity(builder, authConfig);

            ConfigureHttpClient(builder, authConfig);
            ConfigureLogging(builder);

            AddDependencies(builder);
            builder.Services.AddAntDesign();

            await builder.Build().RunAsync();
        }

        private static void ConfigureHttpClient(WebAssemblyHostBuilder builder, AuthConfig authConfig)
        {
            var connectionConfig = builder.Configuration.GetSection("Connection").Get<ConnectionConfig>();

            builder.Services.AddSingleton(connectionConfig);

            if (string.IsNullOrWhiteSpace(connectionConfig.ApiUri))
                throw new InvalidOperationException($"The Connection:{nameof(ConnectionConfig.ApiUri)} section in the appsettings is missing");

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
            var config = builder.Configuration.GetSection("CollectRemoteLogs").Get<CollectRemoteLogsConfig>();
            if (!config.Enabled)
                return;

            var endpointUrl = config.HostUri.EndsWith("/")
                ? $"{config.HostUri}ingest"
                : $"{config.HostUri}/ingest";

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

            builder.Services.AddTransient<IShoppingListApiClient, ShoppingListApiClient>();
            builder.Services.AddTransient<IShoppingListNotificationService, ShoppingListNotificationService>();
            builder.Services.AddTransient<IApiClient, ApiClient>();
            builder.Services.AddScoped<ICommandQueue, CommandQueue>();

            builder.Services.AddScoped<IItemPriceCalculationService, ItemPriceCalculationService>();

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
                builder.Configuration.Bind("Auth:Provider", opt.ProviderOptions);
                builder.Configuration.Bind("Auth:User", opt.UserOptions);
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
            public bool Enabled { get; init; }
            public string HostUri { get; init; } = string.Empty;
        }
    }
}