using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;

public class ItemCategoryReadModel
{
    public ItemCategoryReadModel(ItemCategoryId id, ItemCategoryName name, bool isDeleted)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
    }

    public ItemCategoryReadModel(IItemCategory itemCategory)
    {
        Id = itemCategory.Id;
        Name = itemCategory.Name;
        IsDeleted = itemCategory.IsDeleted;
    }

    public ItemCategoryId Id { get; }
    public ItemCategoryName Name { get; }
    public bool IsDeleted { get; }
}