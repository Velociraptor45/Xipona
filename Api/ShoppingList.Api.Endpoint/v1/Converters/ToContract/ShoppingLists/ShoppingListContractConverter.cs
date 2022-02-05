using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

public class ShoppingListContractConverter : IToContractConverter<ShoppingListReadModel, ShoppingListContract>
{
    private readonly IToContractConverter<ShoppingListSectionReadModel, ShoppingListSectionContract> _shoppingListSectionContractConverter;
    private readonly IToContractConverter<ShoppingListStoreReadModel, ShoppingListStoreContract> _shoppingListStoreContractConverter;

    public ShoppingListContractConverter(
        IToContractConverter<ShoppingListSectionReadModel, ShoppingListSectionContract> shoppingListSectionContractConverter,
        IToContractConverter<ShoppingListStoreReadModel, ShoppingListStoreContract> shoppingListStoreContractConverter)
    {
        _shoppingListSectionContractConverter = shoppingListSectionContractConverter;
        _shoppingListStoreContractConverter = shoppingListStoreContractConverter;
    }

    public ShoppingListContract ToContract(ShoppingListReadModel source)
    {
        return new ShoppingListContract(
            source.Id.Value,
            _shoppingListStoreContractConverter.ToContract(source.Store),
            _shoppingListSectionContractConverter.ToContract(source.Sections),
            source.CompletionDate);
    }
}