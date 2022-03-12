namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemUpdates;

public interface IItemUpdateService
{
    Task UpdateItemWithTypesAsync(ItemWithTypesUpdate update);

    Task Update(ItemUpdate update);
}