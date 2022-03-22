namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Modifications;

public interface IItemModificationService
{
    Task ModifyItemWithTypesAsync(ItemWithTypesModification modification);

    Task Modify(ItemModification modification);
}