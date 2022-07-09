using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Items;

public class ItemAvailabilityContractConverter :
    IToContractConverter<ItemAvailabilityReadModel, ItemAvailabilityContract>
{
    private readonly IToContractConverter<ItemStoreReadModel, ItemStoreContract> _itemStoreContractConverter;
    private readonly IToContractConverter<ItemSectionReadModel, ItemSectionContract> _storeSectionContractConverter;

    public ItemAvailabilityContractConverter(
        IToContractConverter<ItemStoreReadModel, ItemStoreContract> itemStoreContractConverter,
        IToContractConverter<ItemSectionReadModel, ItemSectionContract> storeSectionContractConverter)
    {
        _itemStoreContractConverter = itemStoreContractConverter;
        _storeSectionContractConverter = storeSectionContractConverter;
    }

    public ItemAvailabilityContract ToContract(ItemAvailabilityReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new ItemAvailabilityContract(
            _itemStoreContractConverter.ToContract(source.Store),
            source.Price.Value,
            _storeSectionContractConverter.ToContract(source.DefaultSection));
    }
}