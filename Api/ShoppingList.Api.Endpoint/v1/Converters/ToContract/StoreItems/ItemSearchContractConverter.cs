using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.SearchItemForShoppingLists;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItems;

public class ItemSearchContractConverter :
    IToContractConverter<ItemForShoppingListSearchReadModel, ItemForShoppingListSearchContract>
{
    private readonly IToContractConverter<StoreSectionReadModel, StoreSectionContract> _storeItemSectionContractConverter;

    public ItemSearchContractConverter(
        IToContractConverter<StoreSectionReadModel, StoreSectionContract> storeItemSectionContractConverter)
    {
        _storeItemSectionContractConverter = storeItemSectionContractConverter;
    }

    public ItemForShoppingListSearchContract ToContract(ItemForShoppingListSearchReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new ItemForShoppingListSearchContract(
            source.Id.Value,
            source.TypeId?.Value,
            source.Name,
            source.DefaultQuantity,
            source.Price,
            source.ItemCategory?.Name ?? "",
            source.Manufacturer?.Name ?? "",
            _storeItemSectionContractConverter.ToContract(source.DefaultSection));
    }
}