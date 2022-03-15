using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItems;

public class StoreItemSectionContractConverter :
    IToContractConverter<StoreItemSectionReadModel, StoreItemSectionContract>
{
    public StoreItemSectionContract ToContract(StoreItemSectionReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new StoreItemSectionContract(source.Id.Value, source.Name, source.SortingIndex);
    }
}