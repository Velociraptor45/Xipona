namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemUpdate;

public interface IItemUpdateService
{
    Task UpdateItemWithTypesAsync(ItemWithTypesUpdate update);
}