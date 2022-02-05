using System.Threading.Tasks;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemCreation;

public interface IItemCreationService
{
    Task CreateItemWithTypesAsync(IStoreItem item);
}