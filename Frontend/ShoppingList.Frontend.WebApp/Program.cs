using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Client;
using ProjectHermes.ShoppingList.Frontend.Infrastructure;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Index.Services;
using ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Stores.Services;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification;
using ShoppingList.Frontend.Redux;
using ShoppingList.Frontend.Redux.Shared.Ports;
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

            ConfigureHttpClient(builder);

            AddDependencies(builder);
            builder.Services.AddAntDesign();

            await builder.Build().RunAsync();
        }

        private static void ConfigureHttpClient(WebAssemblyHostBuilder builder)
        {
            var uriString = builder.Configuration["Connection:ApiUri"];

            if (uriString is null)
                throw new InvalidOperationException("The Connection:Uri section in the appsettings is missing");

            var uri = new Uri(uriString);
            builder.Services.AddScoped(_ => new HttpClient { BaseAddress = uri });
        }

        private static void AddDependencies(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddTransient<IShoppingListApiClient, ShoppingListApiClient>();
            builder.Services.AddTransient<IShoppingListNotificationService, ShoppingListNotificationService>();
            builder.Services.AddTransient<IApiClient, ApiClient>();
            builder.Services.AddScoped<ICommandQueue, CommandQueue>();

            builder.Services.AddScoped<IItemPriceCalculationService, ItemPriceCalculationService>();

            builder.Services.AddTransient<IStoresApiService, StoresApiService>();

            builder.Services.AddInfrastructure();

            builder.Services.AddRedux();
        }
    }
}