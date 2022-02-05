using System;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Stores;

public class StoreCreationInfoConverter : IToDomainConverter<CreateStoreContract, StoreCreationInfo>
{
    private readonly IToDomainConverter<StoreSectionContract, IStoreSection> storeSectionConverter;

    public StoreCreationInfoConverter(
        IToDomainConverter<StoreSectionContract, IStoreSection> storeSectionConverter)
    {
        this.storeSectionConverter = storeSectionConverter;
    }

    public StoreCreationInfo ToDomain(CreateStoreContract source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new StoreCreationInfo(
            StoreId.New,
            source.Name,
            storeSectionConverter.ToDomain(source.Sections));
    }
}