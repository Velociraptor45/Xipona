using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Queries.ItemCategoryById;

public class ItemCategoryByIdQuery : IQuery<IItemCategory>
{
    public ItemCategoryByIdQuery(ItemCategoryId itemCategoryId)
    {
        ItemCategoryId = itemCategoryId;
    }

    public ItemCategoryId ItemCategoryId { get; }
}