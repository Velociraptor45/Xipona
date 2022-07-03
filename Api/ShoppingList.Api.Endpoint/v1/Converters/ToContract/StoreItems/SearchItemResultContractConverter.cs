using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItems;

public class SearchItemResultContractConverter :
    IToContractConverter<SearchItemResultReadModel, SearchItemResultContract>
{
    public SearchItemResultContract ToContract(SearchItemResultReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new SearchItemResultContract(source.Id.Value, source.ItemName.Value);
    }
}