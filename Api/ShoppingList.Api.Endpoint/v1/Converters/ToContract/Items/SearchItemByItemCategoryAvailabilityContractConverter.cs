using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Items;

public class SearchItemByItemCategoryAvailabilityContractConverter :
    IToContractConverter<ItemAvailabilityReadModel, SearchItemByItemCategoryAvailabilityContract>
{
    public SearchItemByItemCategoryAvailabilityContract ToContract(ItemAvailabilityReadModel source)
    {
        return new SearchItemByItemCategoryAvailabilityContract(
            source.Store.Id.Value,
            source.Store.Name,
            source.Price.Value);
    }
}