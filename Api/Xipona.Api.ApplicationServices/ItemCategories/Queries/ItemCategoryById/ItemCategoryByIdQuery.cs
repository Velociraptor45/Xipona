using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.ItemCategories.Models;

namespace Xipona.Api.ApplicationServices.ItemCategories.Queries.ItemCategoryById;

public class ItemCategoryByIdQuery : IQuery<IItemCategory>
{
    public ItemCategoryByIdQuery(ItemCategoryId itemCategoryId)
    {
        ItemCategoryId = itemCategoryId;
    }

    public ItemCategoryId ItemCategoryId { get; }
}