using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemDeletions;

public interface IItemDeletionService
{
    Task DeleteAsync(ItemId itemId);
}