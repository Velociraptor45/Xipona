using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Deletions;

public class ShoppingListDeletionService : IShoppingListDeletionService
{
    private readonly ILogger<ShoppingListDeletionService> _logger;
    private readonly IShoppingListRepository _shoppingListRepository;

    public ShoppingListDeletionService(IShoppingListRepository shoppingListRepository,
        ILogger<ShoppingListDeletionService> logger)
    {
        _logger = logger;
        _shoppingListRepository = shoppingListRepository;
    }

    public async Task HardDeleteForStoreAsync(StoreId storeId)
    {
        var shoppingList = await _shoppingListRepository.FindActiveByAsync(storeId);

        if (shoppingList is null)
        {
            _logger.LogWarning(() => "No active shopping list found for store '{StoreId}' - aborting hard delete", storeId.Value);
            return;
        }

        await _shoppingListRepository.DeleteAsync(shoppingList.Id);
        _logger.LogInformation(() => "Deleted shopping list '{ShoppingListId}' after store '{StoreId}' was deleted", shoppingList.Id.Value, storeId.Value);
    }
}