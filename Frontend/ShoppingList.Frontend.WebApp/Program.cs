using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Client;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.WebApp.Service;
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

            builder.Services.AddTransient<IShoppingListApiClient, ShoppingListApiClient>();
            builder.Services.AddTransient<IShoppingListNotificationService, ShoppingListNotificationService>();
            builder.Services.AddTransient<IApiClient, ApiClient>();
            builder.Services.AddTransient<ICommandQueue, CommandQueue>();
            builder.Services.AddScoped<IItemPriceCalculationService, ItemPriceCalculationService>();
            builder.Services.AddAntDesign();

            await builder.Build().RunAsync();
        }

        private static void ConfigureHttpClient(WebAssemblyHostBuilder builder)
        {
            var uri = new Uri(builder.Configuration["Connection:Uri"]);
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = uri });
        }
    }
}