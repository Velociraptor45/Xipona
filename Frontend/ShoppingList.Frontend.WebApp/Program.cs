using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Client;
using ProjectHermes.ShoppingList.Frontend.Infrastructure;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Index.Services;
using ProjectHermes.ShoppingList.Frontend.WebApp.Pages.ItemCategories.Models;
using ProjectHermes.ShoppingList.Frontend.WebApp.Pages.ItemCategories.Services;
using ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Services;
using ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Services.ItemEditor;
using ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Manufacturers.Models;
using ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Manufacturers.Services;
using ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Stores.Services;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification;
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

            builder.Services.AddTransient<IShoppingListApiService, ShoppingListApiService>();
            builder.Services.AddTransient<IItemsApiService, ItemsApiService>();
            builder.Services.AddTransient<IStoresApiService, StoresApiService>();

            builder.Services.AddTransient<IItemEditorApiService, ItemEditorApiService>();

            var manufacturerState = new ManufacturersState();
            builder.Services.AddSingleton(manufacturerState);
            builder.Services.AddTransient<IManufacturerApiService, ManufacturerApiService>();

            var itemCategoryState = new ItemCategoriesState();
            builder.Services.AddSingleton(itemCategoryState);
            builder.Services.AddTransient<IItemCategoryApiService, ItemCategoryApiService>();

            builder.Services.AddInfrastructure();
        }
    }
}