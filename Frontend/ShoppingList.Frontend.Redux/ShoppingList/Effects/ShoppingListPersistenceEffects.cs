using Blazored.LocalStorage;
using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.Items;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.Persistence;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using System.Text;
using System.Text.Json;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Effects;

public class ShoppingListPersistenceEffects
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    private readonly IState<ShoppingListState> _state;
    private readonly ILocalStorageService _localStorageService;

    public ShoppingListPersistenceEffects(IState<ShoppingListState> state, ILocalStorageService localStorageService)
    {
        _state = state;
        _localStorageService = localStorageService;
    }

    [EffectMethod(typeof(PutItemInBasketAction))]
    public async Task HandlePutItemInBasketAction(IDispatcher dispatcher)
    {
        await SaveListToLocalStorage();
    }

    [EffectMethod(typeof(RemoveItemFromBasketAction))]
    public async Task HandleRemoveItemFromBasketAction(IDispatcher dispatcher)
    {
        await SaveListToLocalStorage();
    }

    [EffectMethod(typeof(LoadShoppingListFinishedAction))]
    public async Task HandleLoadShoppingListFinishedAction(IDispatcher dispatcher)
    {
        await SaveListToLocalStorage();
    }

    [EffectMethod]
    public async Task HandleLoadShoppingListFromLocalStorageAction(LoadShoppingListFromLocalStorageAction action,
        IDispatcher dispatcher)
    {
        var list = await LoadListFromLocalStorage(action.StoreId);

        if (list is null)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading shopping list failed",
                "Retrieving shopping list from server and local storage failed"));
            return;
        }

        dispatcher.Dispatch(new LoadShoppingListFinishedAction(list));
    }

    private async Task SaveListToLocalStorage()
    {
        var list = _state.Value.ShoppingList!;
        var storeId = _state.Value.SelectedStoreId;
        var listSerialized = JsonSerializer.Serialize(list, _options);
        var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(listSerialized));

        var key = $"list-{storeId:D}";

        await _localStorageService.SetItemAsStringAsync(key, base64);
    }

    private async Task<ShoppingListModel?> LoadListFromLocalStorage(Guid storeId)
    {
        var key = $"list-{storeId:D}";

        if (!await _localStorageService.ContainKeyAsync(key))
            return null;

        var base64 = await _localStorageService.GetItemAsStringAsync(key);

        var serialized = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        return JsonSerializer.Deserialize<ShoppingListModel>(serialized, _options)!;
    }
}