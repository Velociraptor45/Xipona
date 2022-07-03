using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Stores;

public class StoreContractConverter : IToContractConverter<IStore, StoreContract>
{
    private readonly IToContractConverter<IStoreSection, StoreSectionContract> _sectionConverter;

    public StoreContractConverter(
        IToContractConverter<IStoreSection, StoreSectionContract> sectionConverter)
    {
        _sectionConverter = sectionConverter;
    }

    public StoreContract ToContract(IStore source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var sections = _sectionConverter.ToContract(source.Sections);
        return new StoreContract(source.Id.Value, source.Name.Value, sections);
    }
}