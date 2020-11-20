using ShoppingList.Frontend.Models.Shared.Requests;
using System.Threading.Tasks;

namespace ShoppingList.Frontend.Infrastructure.Connection
{
    public class CommandClient : ICommandClient
    {
        public Task IsAliveAsync()
        {
            return Task.CompletedTask;
        }

        public Task PutItemInBasketAsync(PutItemInBasketRequest request)
        {
            return Task.CompletedTask;
        }
    }
}