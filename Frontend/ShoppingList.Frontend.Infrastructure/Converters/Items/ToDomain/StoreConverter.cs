using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class StoreConverter : IToDomainConverter<StoreItemStoreContract, StoreItemStore>
    {
        private readonly IToDomainConverter<StoreItemSectionContract, StoreItemSection> sectionConverter;

        public StoreConverter(
            IToDomainConverter<StoreItemSectionContract, StoreItemSection> sectionConverter)
        {
            this.sectionConverter = sectionConverter;
        }

        public StoreItemStore ToDomain(StoreItemStoreContract source)
        {
            return new StoreItemStore(
                source.Id,
                source.Name,
                source.Sections.Select(s => sectionConverter.ToDomain(s)));
        }
    }
}