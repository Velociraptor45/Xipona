using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class ItemCategoryConverter : IToDomainConverter<ItemCategoryContract, ItemCategory>
    {
        public ItemCategory ToDomain(ItemCategoryContract source)
        {
            return new ItemCategory(source.Id, source.Name);
        }
    }
}