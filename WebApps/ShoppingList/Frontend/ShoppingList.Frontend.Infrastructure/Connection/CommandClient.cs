using ShoppingList.Api.Client;
using ShoppingList.Frontend.Models.Shared.Requests;
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
            await client.PutItemInBasket(request.ShoppingListId, request.ItemId);
        }

        public async Task RemoveItemFromBasketAsync(RemoveItemFromBasketRequest request)
        {
            await client.RemoveItemFromBasket(request.ShoppingListId, request.ItemId);
        }

        public async Task ChangeItemQuantityOnShoppingListAsync(ChangeItemQuantityOnShoppingListRequest request)
        {
            await client.ChangeItemQuantityOnShoppingList(request.ShoppingListId, request.ItemId, request.Quantity);
        }

        public async Task FinishListAsync(FinishListRequest request)
        {
            await client.FinishList(request.ShoppingListId);
        }

        public async Task RemoveItemFromShoppingListAsync(RemoveItemFromShoppingListRequest request)
        {
            await client.RemoveItemFromShoppingList(request.ShoppingListId, request.ItemId);
        }
    }
}