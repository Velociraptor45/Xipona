using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Api.Client;
using ShoppingList.Frontend.Infrastructure.Connection;
using ShoppingList.Frontend.Models.Ports;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShoppingList.Frontend.WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var uriBuilder = new UriBuilder("http", "", 0, "v1");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = uriBuilder.Uri }); //todo
            builder.Services.AddTransient<IShoppingListApiClient, ShoppingListApiClient>();
            builder.Services.AddTransient<IOfflineClient, OfflineClient>();
            builder.Services.AddAntDesign();

            await builder.Build().RunAsync();
        }
    }
}