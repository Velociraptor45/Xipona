using ShoppingList.Api.Client;
using ShoppingList.Frontend.Models.Shared.Requests;
using System;
using System.Threading.Tasks;

namespace ShoppingList.Frontend.Infrastructure.Connection
{
    public class CommandClient : ICommandClient
    {
        private readonly IShoppingListApiClient client;

        public CommandClient(IShoppingListApiClient client)
        {
            this.client = client;
        }

        public async Task IsAliveAsync()
        {
            _ = await client.IsAlive();
        }

        public async Task PutItemInBasketAsync(PutItemInBasketRequest request)
        {
            Console.WriteLine("Try putting item in basket.");
            await client.PutItemInBasket(request.ShoppingListId, request.ItemId);
            Console.WriteLine("Item successfully put in basket.");
        }
    }
}