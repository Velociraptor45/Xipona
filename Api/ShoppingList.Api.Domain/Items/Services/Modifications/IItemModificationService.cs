namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;

public interface IItemModificationService
{
    Task ModifyItemWithTypesAsync(ItemWithTypesModification modification);

    Task Modify(ItemModification modification);
}