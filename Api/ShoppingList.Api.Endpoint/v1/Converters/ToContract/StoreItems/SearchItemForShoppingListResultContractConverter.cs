using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.SearchItemsForShoppingLists;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Searches;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.StoreQueries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItems;

public class SearchItemForShoppingListResultContractConverter :
    IToContractConverter<SearchItemForShoppingResultReadModel, SearchItemForShoppingListResultContract>
{
    private readonly IToContractConverter<StoreSectionReadModel, StoreSectionContract> _storeItemSectionContractConverter;

    public SearchItemForShoppingListResultContractConverter(
        IToContractConverter<StoreSectionReadModel, StoreSectionContract> storeItemSectionContractConverter)
    {
        _storeItemSectionContractConverter = storeItemSectionContractConverter;
    }

    public SearchItemForShoppingListResultContract ToContract(SearchItemForShoppingResultReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new SearchItemForShoppingListResultContract(
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