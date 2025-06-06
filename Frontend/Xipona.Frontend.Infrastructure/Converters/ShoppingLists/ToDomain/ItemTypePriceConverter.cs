using Xipona.Api.Contracts.Items.Queries.GetItemTypePrices;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain;

public class ItemTypePriceConverter : IToDomainConverter<ItemTypePriceContract, ItemTypePrice>
{
    public ItemTypePrice ToDomain(ItemTypePriceContract source)
    {
        return new ItemTypePrice(source.TypeId, source.Name, source.Price);
    }
}