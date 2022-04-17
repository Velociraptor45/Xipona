using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class StoreConverter : IToDomainConverter<StoreItemStoreContract, ItemStore>
    {
        private readonly IToDomainConverter<StoreItemSectionContract, ItemSection> sectionConverter;

        public StoreConverter(
            IToDomainConverter<StoreItemSectionContract, ItemSection> sectionConverter)
        {
            this.sectionConverter = sectionConverter;
        }

        public ItemStore ToDomain(StoreItemStoreContract source)
        {
            return new ItemStore(
                source.Id,
                source.Name,
                source.Sections.Select(s => sectionConverter.ToDomain(s)));
        }
    }
}