using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class SectionConverter : IToDomainConverter<StoreItemSectionContract, ItemSection>
    {
        public ItemSection ToDomain(StoreItemSectionContract source)
        {
            return new ItemSection(
                source.Id,
                source.Name,
                source.SortingIndex);
        }
    }
}