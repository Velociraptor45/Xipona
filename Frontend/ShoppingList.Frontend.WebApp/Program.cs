using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Client;
using ProjectHermes.ShoppingList.Frontend.Infrastructure;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Models.Index.Services;
using ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Index.Services;
using ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Stores.Services;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Items;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            ConfigureHttpClient(builder);

            AddDependencies(builder);
            builder.Services.AddAntDesign();

            await builder.Build().RunAsync();
        }

        private static void ConfigureHttpClient(WebAssemblyHostBuilder builder)
        {
            var uri = new Uri(builder.Configuration["Connection:Uri"]);
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = uri });
        }

        private static void AddDependencies(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddTransient<IShoppingListApiClient, ShoppingListApiClient>();
            builder.Services.AddTransient<IShoppingListNotificationService, ShoppingListNotificationService>();
            builder.Services.AddTransient<IApiClient, ApiClient>();
            builder.Services.AddScoped<ICommandQueue, CommandQueue>();

            builder.Services.AddTransient<ITemporaryItemCreationService, TemporaryItemCreationService>();

            builder.Services.AddScoped<IItemPriceCalculationService, ItemPriceCalculationService>();

            builder.Services.AddTransient<IShoppingListCommunicationService, ShoppingListCommunicationService>();
            builder.Services.AddTransient<IItemsPageLoadingService, ItemsPageLoadingService>();
            builder.Services.AddTransient<IStoresPageCommunicationService, StoresPageCommunicationService>();

            builder.Services.AddInfrastructure();
        }
    }
}