using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain
{
    public class ShoppingListRootConverter : IToDomainConverter<ShoppingListContract, ShoppingListRoot>
    {
        private readonly IToDomainConverter<ShoppingListSectionContract, ShoppingListSection> _sectionConverter;
        private readonly IToDomainConverter<ShoppingListStoreContract, Store> _storeConverter;

        public ShoppingListRootConverter(
            IToDomainConverter<ShoppingListSectionContract, ShoppingListSection> sectionConverter,
            IToDomainConverter<ShoppingListStoreContract, Store> storeConverter)
        {
            _sectionConverter = sectionConverter;
            _storeConverter = storeConverter;
        }

        public ShoppingListRoot ToDomain(ShoppingListContract source)
        {
            return new ShoppingListRoot(
                source.Id,
                source.CompletionDate,
                _storeConverter.ToDomain(source.Store),
                source.Sections.Select(_sectionConverter.ToDomain));
        }
    }
}