namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models.Factories;

public interface IItemCategoryFactory
{
    IItemCategory Create(ItemCategoryId id, string name, bool isDeleted);
}