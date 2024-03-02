using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Items;

public class SearchItemResultContractConverter :
    IToContractConverter<SearchItemResultReadModel, SearchItemResultContract>
{
    public SearchItemResultContract ToContract(SearchItemResultReadModel source)
    {
        return new SearchItemResultContract(source.Id, source.ItemName, source.ManufacturerName?.Value);
    }
}