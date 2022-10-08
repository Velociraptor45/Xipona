using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Items;

public class ItemStoreContractConverter :
    IToContractConverter<ItemStoreReadModel, ItemStoreContract>
{
    private readonly IToContractConverter<ItemSectionReadModel, ItemSectionContract> _itemSectionContractConverter;

    public ItemStoreContractConverter(
        IToContractConverter<ItemSectionReadModel, ItemSectionContract> itemSectionContractConverter)
    {
        _itemSectionContractConverter = itemSectionContractConverter;
    }

    public ItemStoreContract ToContract(ItemStoreReadModel source)
    {
        return new ItemStoreContract(
            source.Id.Value,
            source.Name.Value,
            _itemSectionContractConverter.ToContract(source.Sections));
    }
}