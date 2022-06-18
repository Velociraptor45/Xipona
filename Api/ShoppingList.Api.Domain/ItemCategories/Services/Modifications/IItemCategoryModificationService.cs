namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Modifications;

public interface IItemCategoryModificationService
{
    Task ModifyAsync(ItemCategoryModification modification);
}