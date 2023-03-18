using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor.ManufacturerSelectors;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using RestEase;
using Timer = System.Timers.Timer;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Effects;

public sealed class ManufacturerSelectorEffects : IDisposable
{
    private readonly IApiClient _client;
    private readonly IState<ItemState> _state;

    private Timer? _startSearchTimer;

    public ManufacturerSelectorEffects(IApiClient client, IState<ItemState> state)
    {
        _client = client;
        _state = state;
    }

    [EffectMethod(typeof(LoadInitialManufacturerAction))]
    public async Task HandleLoadInitialManufacturerAction(IDispatcher dispatcher)
    {
        if (_state.Value.Editor.Item?.ManufacturerId is null)
            return;

        EditedManufacturer manufacturer;
        try
        {
            manufacturer = await _client.GetManufacturerByIdAsync(_state.Value.Editor.Item!.ManufacturerId!.Value);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading manufacturer failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading manufacturer failed", e.Message));
            return;
        }

        var result = new ManufacturerSearchResult(manufacturer.Id, manufacturer.Name);
        dispatcher.Dispatch(new LoadInitialManufacturerFinishedAction(result));
    }

    [EffectMethod]
    public async Task HandleCreateNewManufacturerAction(CreateNewManufacturerAction action, IDispatcher dispatcher)
    {
        EditedManufacturer result;
        try
        {
            result = await _client.CreateManufacturerAsync(_state.Value.Editor.ManufacturerSelector.Input);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Creating manufacturer failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Creating manufacturer failed", e.Message));
            return;
        }

        dispatcher.Dispatch(
            new CreateNewManufacturerFinishedAction(new ManufacturerSearchResult(result.Id, result.Name)));
    }

    [EffectMethod]
    public Task HandleManufacturerInputChangedAction(ManufacturerInputChangedAction action, IDispatcher dispatcher)
    {
        if (_startSearchTimer is not null)
        {
            _startSearchTimer.Stop();
            _startSearchTimer.Dispose();
        }

        if (string.IsNullOrWhiteSpace(action.Input))
        {
            dispatcher.Dispatch(new SearchManufacturerFinishedAction(new List<ManufacturerSearchResult>()));
            return Task.CompletedTask;
        }

        _startSearchTimer = new(300d);
        _startSearchTimer.AutoReset = false;
        _startSearchTimer.Elapsed += (_, _) => dispatcher.Dispatch(new SearchManufacturerAction());
        _startSearchTimer.Start();

        return Task.CompletedTask;
    }

    [EffectMethod(typeof(ManufacturerDropdownClosedAction))]
    public static Task HandleManufacturerDropdownClosedAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new ClearManufacturerAction());
        return Task.CompletedTask;
    }

    [EffectMethod(typeof(SearchManufacturerAction))]
    public async Task HandleSearchManufacturerAction(IDispatcher dispatcher)
    {
        var input = _state.Value.Editor.ManufacturerSelector.Input;
        if (string.IsNullOrWhiteSpace(input))
            return;

        IEnumerable<ManufacturerSearchResult> result;
        try
        {
            result = await _client.GetManufacturerSearchResultsAsync(input);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Searching for manufacturers failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Searching for manufacturers failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new SearchManufacturerFinishedAction(result.ToList()));
    }

    [EffectMethod(typeof(ClearManufacturerAction))]
    public static Task HandleClearManufacturerAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new ManufacturerInputChangedAction(string.Empty));
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _startSearchTimer?.Dispose();
    }
}