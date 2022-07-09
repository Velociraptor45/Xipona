using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class SectionConverter : IToDomainConverter<ItemSectionContract, ItemSection>
    {
        public ItemSection ToDomain(ItemSectionContract source)
        {
            return new ItemSection(
                source.Id,
                source.Name,
                source.SortingIndex);
        }
    }
}