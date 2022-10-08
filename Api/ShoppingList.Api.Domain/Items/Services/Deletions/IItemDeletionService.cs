using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Deletions;

public interface IItemDeletionService
{
    Task DeleteAsync(ItemId itemId);
}