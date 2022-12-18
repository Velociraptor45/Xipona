using Fluxor;
using Microsoft.AspNetCore.Components;
using ShoppingList.Frontend.Redux.Manufacturers.Actions;
using ShoppingList.Frontend.Redux.Manufacturers.States;
using ShoppingList.Frontend.Redux.Shared.Ports;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.Manufacturers;

namespace ShoppingList.Frontend.Redux.Manufacturers.Effects;

public class ManufacturerEffects
{
    private const string ManufacturerPageName = "manufacturers";

    private readonly IApiClient _client;
    private readonly NavigationManager _navigationManager;
    private readonly IState<ManufacturerState> _state;

    public ManufacturerEffects(IApiClient client, NavigationManager navigationManager, IState<ManufacturerState> state)
    {
        _client = client;
        _navigationManager = navigationManager;
        _state = state;
    }

    [EffectMethod]
    public async Task HandleSearchManufacturersAction(SearchManufacturersAction action, IDispatcher dispatcher)
    {
        if (string.IsNullOrWhiteSpace(action.SearchInput))
        {
            dispatcher.Dispatch(new SearchManufacturersFinishedAction(new List<ManufacturerSearchResult>()));
            return;
        }

        dispatcher.Dispatch(new SearchManufacturersStartedAction());

        var result = await _client.GetManufacturerSearchResultsAsync(action.SearchInput);

        var finishAction = new SearchManufacturersFinishedAction(result.ToList());
        dispatcher.Dispatch(finishAction);
    }

    [EffectMethod]
    public Task HandleEditManufacturerAction(EditManufacturerAction action, IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo($"{ManufacturerPageName}/{action.Id}");
        return Task.CompletedTask;
    }

    [EffectMethod(typeof(LeaveManufacturerEditorAction))]
    public Task HandleLeaveManufacturerEditorAction(IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo(ManufacturerPageName);
        return Task.CompletedTask;
    }

    [EffectMethod]
    public async Task HandleLoadManufacturerForEditingAction(LoadManufacturerForEditingAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new LoadManufacturerForEditingStartedAction());

        var manufacturer = await _client.GetManufacturerByIdAsync(action.Id);

        var finishAction =
            new LoadManufacturerForEditingFinishedAction(new EditedManufacturer(manufacturer.Id, manufacturer.Name));
        dispatcher.Dispatch(finishAction);
    }

    [EffectMethod(typeof(SaveManufacturerAction))]
    public async Task HandleSaveManufacturerAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SavingManufacturerStartedAction());

        var manufacturer = _state.Value.Editor;
        if (manufacturer.Manufacturer!.Id == Guid.Empty)
        {
            await _client.CreateManufacturerAsync(manufacturer.Manufacturer.Name);
        }
        else
        {
            await _client.ModifyManufacturerAsync(
                new ModifyManufacturerRequest(manufacturer.Manufacturer.Id, manufacturer.Manufacturer.Name));
            var updateAction = new UpdateSearchResultsAfterSaveAction(
                manufacturer.Manufacturer.Id,
                manufacturer.Manufacturer.Name);
            dispatcher.Dispatch(updateAction);
        }

        dispatcher.Dispatch(new SavingManufacturerFinishedAction());
        dispatcher.Dispatch(new LeaveManufacturerEditorAction());
    }

    [EffectMethod(typeof(DeleteManufacturerAction))]
    public async Task HandleDeleteManufacturerAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new DeletingManufacturerStartedAction());

        await _client.DeleteManufacturerAsync(_state.Value.Editor.Manufacturer!.Id);

        dispatcher.Dispatch(new DeletingManufacturerFinishedAction());
        dispatcher.Dispatch(new LeaveManufacturerEditorAction());
    }
}