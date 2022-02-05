using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Stores;

public class StoreUpdateConverter : IToDomainConverter<UpdateStoreContract, StoreUpdate>
{
    private readonly IToDomainConverter<StoreSectionContract, IStoreSection> _storeSectionConverter;

    public StoreUpdateConverter(IToDomainConverter<StoreSectionContract, IStoreSection> storeSectionConverter)
    {
        _storeSectionConverter = storeSectionConverter;
    }

    public StoreUpdate ToDomain(UpdateStoreContract source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new StoreUpdate(
            new StoreId(source.Id),
            source.Name,
            _storeSectionConverter.ToDomain(source.Sections));
    }
}