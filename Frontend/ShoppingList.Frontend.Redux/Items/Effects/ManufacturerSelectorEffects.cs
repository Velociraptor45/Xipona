using Fluxor;
using ShoppingList.Frontend.Redux.Items.Actions.Editor.ManufacturerSelectors;
using ShoppingList.Frontend.Redux.Items.States;
using ShoppingList.Frontend.Redux.Manufacturers.States;
using ShoppingList.Frontend.Redux.Shared.Ports;
using Timer = System.Timers.Timer;

namespace ShoppingList.Frontend.Redux.Items.Effects;

public sealed class ManufacturerSelectorEffects
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

        var manufacturer = await _client.GetManufacturerByIdAsync(_state.Value.Editor.Item!.ManufacturerId!.Value);
        var result = new ManufacturerSearchResult(manufacturer.Id, manufacturer.Name);
        dispatcher.Dispatch(new LoadInitialManufacturerFinishedAction(result));
    }

    [EffectMethod]
    public async Task HandleCreateNewManufacturerAction(CreateNewManufacturerAction action, IDispatcher dispatcher)
    {
        var result = await _client.CreateManufacturerAsync(_state.Value.Editor.ManufacturerSelector.Input); //todo change return type #298
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
    public Task HandleManufacturerDropdownClosedAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new ManufacturerInputChangedAction(string.Empty));
        return Task.CompletedTask;
    }

    [EffectMethod(typeof(SearchManufacturerAction))]
    public async Task HandleSearchManufacturerAction(IDispatcher dispatcher)
    {
        var input = _state.Value.Editor.ManufacturerSelector.Input;
        if (string.IsNullOrWhiteSpace(input))
            return;

        var result = await _client.GetManufacturerSearchResultsAsync(input);
        dispatcher.Dispatch(new SearchManufacturerFinishedAction(result.ToList()));
    }

    [EffectMethod(typeof(ClearManufacturerAction))]
    public Task HandleClearManufacturerAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new ManufacturerInputChangedAction(string.Empty));
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _startSearchTimer?.Dispose();
    }
}