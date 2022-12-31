using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class StoreConverter : IToDomainConverter<ItemStoreContract, ItemStore> // todo remove #298
    {
        private readonly IToDomainConverter<ItemSectionContract, ItemSection> _sectionConverter;

        public StoreConverter(
            IToDomainConverter<ItemSectionContract, ItemSection> sectionConverter)
        {
            _sectionConverter = sectionConverter;
        }

        public ItemStore ToDomain(ItemStoreContract source)
        {
            return new ItemStore(
                source.Id,
                source.Name,
                source.Sections.Select(s => _sectionConverter.ToDomain(s)));
        }
    }
}