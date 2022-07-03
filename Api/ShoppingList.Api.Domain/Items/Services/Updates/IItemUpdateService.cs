namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;

public interface IItemUpdateService
{
    Task UpdateItemWithTypesAsync(ItemWithTypesUpdate update);

    Task Update(ItemUpdate update);
}