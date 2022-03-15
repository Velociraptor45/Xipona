using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Stores;

public class StoreSectionContractConverter : IToContractConverter<StoreSectionReadModel, StoreSectionContract>
{
    public StoreSectionContract ToContract(StoreSectionReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new StoreSectionContract(
            source.Id.Value,
            source.Name,
            source.SortingIndex,
            source.IsDefaultSection);
    }
}