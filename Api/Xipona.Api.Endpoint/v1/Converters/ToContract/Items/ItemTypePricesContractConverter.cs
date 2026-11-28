using Xipona.Api.Contracts.Items.Queries.GetItemTypePrices;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Items.Services.Queries;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.Items;

public class ItemTypePricesContractConverter : IToContractConverter<ItemTypePricesReadModel, ItemTypePricesContract>
{
    public ItemTypePricesContract ToContract(ItemTypePricesReadModel source)
    {
        var prices = source.Prices.Select(p => new ItemTypePriceContract(p.Id, p.Price, p.Name));
        return new ItemTypePricesContract(source.ItemId, source.StoreId, prices);
    }
}