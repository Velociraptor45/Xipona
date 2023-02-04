using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Stores;

public class StoreForItemContractConverter : IToContractConverter<IStore, StoreForItemContract>
{
    private readonly IToContractConverter<ISection, SectionForItemContract> _sectionConverter;

    public StoreForItemContractConverter(
        IToContractConverter<ISection, SectionForItemContract> sectionConverter)
    {
        _sectionConverter = sectionConverter;
    }

    public StoreForItemContract ToContract(IStore source)
    {
        return new StoreForItemContract(
            source.Id,
            source.Name,
            source.Sections.Select(_sectionConverter.ToContract));
    }
}