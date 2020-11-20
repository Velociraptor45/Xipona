using ShoppingList.Frontend.Models.Shared.Requests;
using System.Threading.Tasks;

namespace ShoppingList.Frontend.Infrastructure.Connection
{
    public interface ICommandClient
    {
        Task IsAliveAsync();

        Task PutItemInBasketAsync(PutItemInBasketRequest request);
    }
}