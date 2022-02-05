using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId;

public class ActiveShoppingListByStoreIdQueryHandler
    : IQueryHandler<ActiveShoppingListByStoreIdQuery, ShoppingListReadModel>
{
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly IShoppingListReadModelConversionService _shoppingListReadModelConversionService;

    public ActiveShoppingListByStoreIdQueryHandler(IShoppingListRepository shoppingListRepository,
        IShoppingListReadModelConversionService shoppingListReadModelConversionService)
    {
        _shoppingListRepository = shoppingListRepository;
        _shoppingListReadModelConversionService = shoppingListReadModelConversionService;
    }

    public async Task<ShoppingListReadModel> HandleAsync(ActiveShoppingListByStoreIdQuery query,
        CancellationToken cancellationToken)
    {
        var shoppingList = await _shoppingListRepository.FindActiveByAsync(query.StoreId, cancellationToken);
        if (shoppingList == null)
            throw new DomainException(new ShoppingListNotFoundReason(query.StoreId));

        cancellationToken.ThrowIfCancellationRequested();

        return await _shoppingListReadModelConversionService.ConvertAsync(shoppingList, cancellationToken);
    }
}