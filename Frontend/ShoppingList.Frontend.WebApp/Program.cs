using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Client;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
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

            var uriBuilder = new UriBuilder("http", "", 0, "v1");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = uriBuilder.Uri }); //todo
            builder.Services.AddTransient<IShoppingListApiClient, ShoppingListApiClient>();
            builder.Services.AddTransient<IApiClient, ApiClient>();
            builder.Services.AddTransient<ICommandQueue, CommandQueue>();
            builder.Services.AddAntDesign();

            await builder.Build().RunAsync();
        }
    }
}