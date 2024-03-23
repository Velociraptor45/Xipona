using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Modifications;

public class ItemCategoryModification
{
    public ItemCategoryModification(ItemCategoryId id, ItemCategoryName name)
    {
        Id = id;
        Name = name;
    }

    public ItemCategoryId Id { get; }
    public ItemCategoryName Name { get; }
}