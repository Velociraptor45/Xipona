using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ItemCategories.ToDomain
{
    public class EditedItemCategoryConverter : IToDomainConverter<ItemCategoryContract, EditedItemCategory>
    {
        public EditedItemCategory ToDomain(ItemCategoryContract source)
        {
            return new EditedItemCategory(source.Id, source.Name);
        }
    }
}