using ProjectHermes.Xipona.Api.Contracts.Items.Queries.GetItemTypePrices;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain;

public class ItemTypePriceConverter : IToDomainConverter<ItemTypePriceContract, ItemTypePrice>
{
    public ItemTypePrice ToDomain(ItemTypePriceContract source)
    {
        return new ItemTypePrice(source.TypeId, source.Name, source.Price);
    }
}