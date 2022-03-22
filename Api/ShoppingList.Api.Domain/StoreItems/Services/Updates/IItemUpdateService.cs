namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Updates;

public interface IItemUpdateService
{
    Task UpdateItemWithTypesAsync(ItemWithTypesUpdate update);

    Task Update(ItemUpdate update);
}