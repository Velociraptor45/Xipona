using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Stores;

public class StoreForShoppingContractConverter : IToContractConverter<IStore, StoreForShoppingContract>
{
    private readonly IToContractConverter<ISection, SectionForShoppingContract> _sectionConverter;

    public StoreForShoppingContractConverter(
        IToContractConverter<ISection, SectionForShoppingContract> sectionConverter)
    {
        _sectionConverter = sectionConverter;
    }

    public StoreForShoppingContract ToContract(IStore source)
    {
        var sectionsToConvert = source.Sections.Where(s => !s.IsDeleted);
        return new StoreForShoppingContract(
            source.Id,
            source.Name,
            sectionsToConvert.Select(_sectionConverter.ToContract));
    }
}