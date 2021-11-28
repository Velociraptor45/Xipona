using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemCreation
{
    public interface IItemCreationService
    {
        Task CreateItemWithTypesAsync(IStoreItem item);
    }
}