using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Items;

public class ItemStoreContractConverter :
    IToContractConverter<ItemStoreReadModel, ItemStoreContract>
{
    private readonly IToContractConverter<ItemSectionReadModel, ItemSectionContract> _storeItemSectionContractConverter;

    public ItemStoreContractConverter(
        IToContractConverter<ItemSectionReadModel, ItemSectionContract> storeItemSectionContractConverter)
    {
        _storeItemSectionContractConverter = storeItemSectionContractConverter;
    }

    public ItemStoreContract ToContract(ItemStoreReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new ItemStoreContract(
            source.Id.Value,
            source.Name.Value,
            _storeItemSectionContractConverter.ToContract(source.Sections));
    }
}