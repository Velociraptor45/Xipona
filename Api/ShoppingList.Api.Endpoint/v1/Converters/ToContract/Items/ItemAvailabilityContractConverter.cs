using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Items;

public class ItemAvailabilityContractConverter :
    IToContractConverter<ItemAvailabilityReadModel, ItemAvailabilityContract>
{
    private readonly IToContractConverter<ItemStoreReadModel, ItemStoreContract> _itemStoreContractConverter;
    private readonly IToContractConverter<ItemSectionReadModel, ItemSectionContract> _sectionContractConverter;

    public ItemAvailabilityContractConverter(
        IToContractConverter<ItemStoreReadModel, ItemStoreContract> itemStoreContractConverter,
        IToContractConverter<ItemSectionReadModel, ItemSectionContract> sectionContractConverter)
    {
        _itemStoreContractConverter = itemStoreContractConverter;
        _sectionContractConverter = sectionContractConverter;
    }

    public ItemAvailabilityContract ToContract(ItemAvailabilityReadModel source)
    {
        return new ItemAvailabilityContract(
            _itemStoreContractConverter.ToContract(source.Store),
            source.Price,
            _sectionContractConverter.ToContract(source.DefaultSection));
    }
}