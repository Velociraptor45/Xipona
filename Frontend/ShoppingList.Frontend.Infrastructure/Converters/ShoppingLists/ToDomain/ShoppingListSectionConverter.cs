using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain
{
    public class ShoppingListSectionConverter : IToDomainConverter<ShoppingListSectionContract, ShoppingListSection>
    {
        private readonly IToDomainConverter<ShoppingListItemContract, ShoppingListItem> _itemConverter;

        public ShoppingListSectionConverter(
            IToDomainConverter<ShoppingListItemContract, ShoppingListItem> itemConverter)
        {
            _itemConverter = itemConverter;
        }

        public ShoppingListSection ToDomain(ShoppingListSectionContract source)
        {
            return new ShoppingListSection(
                source.Id,
                source.Name,
                source.SortingIndex,
                source.IsDefaultSection,
                source.Items.Select(_itemConverter.ToDomain));
        }
    }
}