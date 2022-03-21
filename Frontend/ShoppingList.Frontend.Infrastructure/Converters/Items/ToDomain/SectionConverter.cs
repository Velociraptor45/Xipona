using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class SectionConverter : IToDomainConverter<StoreItemSectionContract, StoreItemSection>
    {
        public StoreItemSection ToDomain(StoreItemSectionContract source)
        {
            return new StoreItemSection(
                source.Id,
                source.Name,
                source.SortingIndex);
        }
    }
}