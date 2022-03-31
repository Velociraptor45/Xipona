using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Stores;

public class StoreSectionContractConverter :
    IToContractConverter<StoreSectionReadModel, StoreSectionContract>,
    IToContractConverter<IStoreSection, StoreSectionContract>
{
    public StoreSectionContract ToContract(StoreSectionReadModel source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new StoreSectionContract(
            source.Id.Value,
            source.Name.Value,
            source.SortingIndex,
            source.IsDefaultSection);
    }

    public StoreSectionContract ToContract(IStoreSection source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new StoreSectionContract(
            source.Id.Value,
            source.Name.Value,
            source.SortingIndex,
            source.IsDefaultSection);
    }
}