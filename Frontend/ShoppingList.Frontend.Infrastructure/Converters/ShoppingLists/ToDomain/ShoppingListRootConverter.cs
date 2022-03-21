using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain
{
    public class ShoppingListRootConverter : IToDomainConverter<ShoppingListContract, ShoppingListRoot>
    {
        private readonly IToDomainConverter<ShoppingListSectionContract, ShoppingListSection> sectionConverter;
        private readonly IToDomainConverter<ShoppingListStoreContract, Store> storeConverter;

        public ShoppingListRootConverter(
            IToDomainConverter<ShoppingListSectionContract, ShoppingListSection> sectionConverter,
            IToDomainConverter<ShoppingListStoreContract, Store> storeConverter)
        {
            this.sectionConverter = sectionConverter;
            this.storeConverter = storeConverter;
        }

        public ShoppingListRoot ToDomain(ShoppingListContract source)
        {
            return new ShoppingListRoot(
                source.Id,
                source.CompletionDate,
                storeConverter.ToDomain(source.Store),
                source.Sections.Select(sectionConverter.ToDomain));
        }
    }
}