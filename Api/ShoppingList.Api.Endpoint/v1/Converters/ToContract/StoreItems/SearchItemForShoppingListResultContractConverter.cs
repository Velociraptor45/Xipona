using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.SearchItemsForShoppingLists;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

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
            source.Price.Value,
            source.ItemCategory?.Name.Value ?? "",
            source.Manufacturer?.Name.Value ?? "",
            _storeItemSectionContractConverter.ToContract(source.DefaultSection));
    }
}