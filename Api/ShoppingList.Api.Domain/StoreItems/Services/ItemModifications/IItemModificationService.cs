namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModifications;

public interface IItemModificationService
{
    Task ModifyItemWithTypesAsync(ItemWithTypesModification modification);
    Task Modify(ItemModification modification);
}