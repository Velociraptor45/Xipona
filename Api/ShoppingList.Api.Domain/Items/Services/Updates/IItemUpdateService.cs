using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;

public interface IItemUpdateService
{
    Task UpdateItemWithTypesAsync(ItemWithTypesUpdate update);

    Task Update(ItemUpdate update);

    Task UpdateAsync(ItemId itemId, ItemTypeId? itemTypeId, StoreId storeId, Price price);
}