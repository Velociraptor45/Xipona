using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Queries;

public class ShoppingListQueryService : IShoppingListQueryService
{
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly IShoppingListReadModelConversionService _shoppingListReadModelConversionService;
    private readonly CancellationToken _cancellationToken;

    public ShoppingListQueryService(
        IShoppingListRepository shoppingListRepository,
        Func<CancellationToken, IShoppingListReadModelConversionService> shoppingListReadModelConversionServiceDelegate,
        CancellationToken cancellationToken)
    {
        _shoppingListRepository = shoppingListRepository;
        _shoppingListReadModelConversionService = shoppingListReadModelConversionServiceDelegate(cancellationToken);
        _cancellationToken = cancellationToken;
    }

    public async Task<ShoppingListReadModel> GetActiveAsync(StoreId storeId)
    {
        var shoppingList = await _shoppingListRepository.FindActiveByAsync(storeId, _cancellationToken);
        if (shoppingList == null)
            throw new DomainException(new ShoppingListNotFoundReason(storeId));

        _cancellationToken.ThrowIfCancellationRequested();

        return await _shoppingListReadModelConversionService.ConvertAsync(shoppingList, _cancellationToken);
    }
}