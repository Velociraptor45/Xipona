using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

public class ItemCategory : IItemCategory
{
    public ItemCategory(ItemCategoryId id, ItemCategoryName name, bool isDeleted)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
    }

    public ItemCategoryId Id { get; }
    public ItemCategoryName Name { get; private set; }
    public bool IsDeleted { get; set; }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void Modify(ItemCategoryModification modification)
    {
        Name = modification.Name;
    }
}