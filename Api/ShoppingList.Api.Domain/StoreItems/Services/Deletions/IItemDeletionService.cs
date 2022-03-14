using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Deletions;

public interface IItemDeletionService
{
    Task DeleteAsync(ItemId itemId);
}