using Xipona.Api.Domain.ItemCategories.Models;

namespace Xipona.Api.Domain.ItemCategories.Services.Queries;

public class ItemCategorySearchResultReadModel
{
    public ItemCategorySearchResultReadModel(ItemCategoryId id, ItemCategoryName name)
    {
        Id = id;
        Name = name;
    }

    public ItemCategoryId Id { get; }
    public ItemCategoryName Name { get; }
}