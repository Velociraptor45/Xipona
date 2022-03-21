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
    public ItemCategoryName Name { get; }
    public bool IsDeleted { get; set; }

    public void Delete()
    {
        IsDeleted = true;
    }
}