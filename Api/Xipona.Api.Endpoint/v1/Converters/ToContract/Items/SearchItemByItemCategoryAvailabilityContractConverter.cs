using ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Items;

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