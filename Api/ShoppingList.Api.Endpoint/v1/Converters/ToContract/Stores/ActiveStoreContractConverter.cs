using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Stores;

public class ActiveStoreContractConverter : IToContractConverter<StoreReadModel, ActiveStoreContract>
{
    private readonly IToContractConverter<SectionReadModel, SectionContract> _sectionContractConverter;

    public ActiveStoreContractConverter(
        IToContractConverter<SectionReadModel, SectionContract> sectionContractConverter)
    {
        _sectionContractConverter = sectionContractConverter;
    }

    public ActiveStoreContract ToContract(StoreReadModel source)
    {
        return new ActiveStoreContract(
            source.Id.Value,
            source.Name.Value,
            source.ItemCount,
            _sectionContractConverter.ToContract(source.Sections));
    }
}