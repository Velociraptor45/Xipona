using Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.ShoppingLists.Services.Queries;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

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
            source.Id,
            _shoppingListStoreContractConverter.ToContract(source.Store),
            _shoppingListSectionContractConverter.ToContract(source.Sections),
            source.CompletionDate,
            source.ListDiscounts.Select(d => new ShoppingListDiscountContract(d.Id, d.Price, d.Percentage)));
    }
}