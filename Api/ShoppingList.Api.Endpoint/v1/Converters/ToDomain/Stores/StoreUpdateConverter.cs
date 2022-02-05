using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Stores;

public class StoreUpdateConverter : IToDomainConverter<UpdateStoreContract, StoreUpdate>
{
    private readonly IToDomainConverter<StoreSectionContract, IStoreSection> storeSectionConverter;

    public StoreUpdateConverter(IToDomainConverter<StoreSectionContract, IStoreSection> storeSectionConverter)
    {
        this.storeSectionConverter = storeSectionConverter;
    }

    public StoreUpdate ToDomain(UpdateStoreContract source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new StoreUpdate(
            new StoreId(source.Id),
            source.Name,
            storeSectionConverter.ToDomain(source.Sections));
    }
}