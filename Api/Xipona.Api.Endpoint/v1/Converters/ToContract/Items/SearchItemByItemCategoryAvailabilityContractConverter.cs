using Xipona.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Items.Services.Queries;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.Items;

public class SearchItemByItemCategoryAvailabilityContractConverter :
    IToContractConverter<ItemAvailabilityReadModel, SearchItemByItemCategoryAvailabilityContract>
{
    public SearchItemByItemCategoryAvailabilityContract ToContract(ItemAvailabilityReadModel source)
    {
        return new SearchItemByItemCategoryAvailabilityContract(
            source.Store.Id,
            source.Store.Name,
            source.Price);
    }
}