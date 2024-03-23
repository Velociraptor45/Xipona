using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Queries;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

public class ShoppingListSectionContractConverter :
    IToContractConverter<ShoppingListSectionReadModel, ShoppingListSectionContract>
{
    private readonly IToContractConverter<ShoppingListItemReadModel, ShoppingListItemContract> _shoppingListItemContractConverter;

    public ShoppingListSectionContractConverter(
        IToContractConverter<ShoppingListItemReadModel, ShoppingListItemContract> shoppingListItemContractConverter)
    {
        _shoppingListItemContractConverter = shoppingListItemContractConverter;
    }

    public ShoppingListSectionContract ToContract(ShoppingListSectionReadModel source)
    {
        return new ShoppingListSectionContract(
            source.Id,
            source.Name,
            source.SortingIndex,
            source.IsDefaultSection,
            _shoppingListItemContractConverter.ToContract(source.ItemReadModels));
    }
}