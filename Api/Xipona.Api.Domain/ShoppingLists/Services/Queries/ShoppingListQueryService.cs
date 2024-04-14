using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Queries;

public class ShoppingListQueryService : IShoppingListQueryService
{
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly IShoppingListReadModelConversionService _shoppingListReadModelConversionService;

    public ShoppingListQueryService(IShoppingListRepository shoppingListRepository,
        IShoppingListReadModelConversionService shoppingListReadModelConversionService)
    {
        _shoppingListRepository = shoppingListRepository;
        _shoppingListReadModelConversionService = shoppingListReadModelConversionService;
    }

    public async Task<ShoppingListReadModel> GetActiveAsync(StoreId storeId)
    {
        var shoppingList = await _shoppingListRepository.FindActiveByAsync(storeId);
        if (shoppingList == null)
            throw new DomainException(new ShoppingListNotFoundReason(storeId));

        return await _shoppingListReadModelConversionService.ConvertAsync(shoppingList);
    }
}