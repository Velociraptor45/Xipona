using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Queries.ItemCategoryById;

public class ItemCategoryByIdQuery : IQuery<IItemCategory>
{
    public ItemCategoryByIdQuery(ItemCategoryId itemCategoryId)
    {
        ItemCategoryId = itemCategoryId;
    }

    public ItemCategoryId ItemCategoryId { get; }
}