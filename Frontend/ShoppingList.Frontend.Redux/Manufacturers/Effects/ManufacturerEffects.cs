using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Constants;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Manufacturers;
using RestEase;
using Timer = System.Timers.Timer;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.Effects;

public class ManufacturerEffects
{
    private readonly IApiClient _client;
    private readonly NavigationManager _navigationManager;
    private readonly IState<ManufacturerState> _state;
    private readonly IShoppingListNotificationService _notificationService;

    private Timer? _leaveEditorTimer;

    public ManufacturerEffects(IApiClient client, NavigationManager navigationManager, IState<ManufacturerState> state,
        IShoppingListNotificationService notificationService)
    {
        _client = client;
        _navigationManager = navigationManager;
        _state = state;
        _notificationService = notificationService;
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

        IEnumerable<ManufacturerSearchResult> result;
        try
        {
            result = await _client.GetManufacturerSearchResultsAsync(action.SearchInput);
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

        var finishAction = new SearchManufacturersFinishedAction(result.ToList());
        dispatcher.Dispatch(finishAction);
    }

    [EffectMethod]
    public Task HandleEditManufacturerAction(EditManufacturerAction action, IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo($"{PageRoutes.Manufacturers}/{action.Id}");
        return Task.CompletedTask;
    }

    [EffectMethod(typeof(LeaveManufacturerEditorAction))]
    public Task HandleLeaveManufacturerEditorAction(IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo(PageRoutes.Manufacturers);
        return Task.CompletedTask;
    }

    [EffectMethod]
    public async Task HandleLoadManufacturerForEditingAction(LoadManufacturerForEditingAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new LoadManufacturerForEditingStartedAction());

        EditedManufacturer manufacturer;
        try
        {
            manufacturer = await _client.GetManufacturerByIdAsync(action.Id);
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

        var finishAction = new LoadManufacturerForEditingFinishedAction(manufacturer);
        dispatcher.Dispatch(finishAction);
    }

    [EffectMethod(typeof(SaveManufacturerAction))]
    public async Task HandleSaveManufacturerAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SavingManufacturerStartedAction());

        var editor = _state.Value.Editor;
        if (editor.Manufacturer!.Id == Guid.Empty)
        {
            try
            {
                await _client.CreateManufacturerAsync(editor.Manufacturer.Name);
            }
            catch (ApiException e)
            {
                dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Creating manufacturer failed", e));
                dispatcher.Dispatch(new SavingManufacturerFinishedAction());
                return;
            }
            catch (HttpRequestException e)
            {
                dispatcher.Dispatch(new DisplayErrorNotificationAction("Creating manufacturer failed", e.Message));
                dispatcher.Dispatch(new SavingManufacturerFinishedAction());
                return;
            }
            _notificationService.NotifySuccess($"Successfully created manufacturer {editor.Manufacturer.Name}");
        }
        else
        {
            try
            {
                await _client.ModifyManufacturerAsync(
                        new ModifyManufacturerRequest(editor.Manufacturer.Id, editor.Manufacturer.Name));
            }
            catch (ApiException e)
            {
                dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Saving manufacturer failed", e));
                dispatcher.Dispatch(new SavingManufacturerFinishedAction());
                return;
            }
            catch (HttpRequestException e)
            {
                dispatcher.Dispatch(new DisplayErrorNotificationAction("Saving manufacturer failed", e.Message));
                dispatcher.Dispatch(new SavingManufacturerFinishedAction());
                return;
            }

            var updateAction = new UpdateSearchResultsAfterSaveAction(
                editor.Manufacturer.Id,
                editor.Manufacturer.Name);
            dispatcher.Dispatch(updateAction);
            _notificationService.NotifySuccess($"Successfully modified manufacturer {editor.Manufacturer.Name}");
        }

        dispatcher.Dispatch(new SavingManufacturerFinishedAction());
        dispatcher.Dispatch(new LeaveManufacturerEditorAction());
    }

    [EffectMethod(typeof(DeleteManufacturerAction))]
    public async Task HandleDeleteManufacturerAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new DeletingManufacturerStartedAction());

        var manufacturer = _state.Value.Editor.Manufacturer!;
        try
        {
            await _client.DeleteManufacturerAsync(manufacturer.Id);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Deleting manufacturer failed", e));
            dispatcher.Dispatch(new DeletingManufacturerFinishedAction());
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Deleting manufacturer failed", e.Message));
            dispatcher.Dispatch(new DeletingManufacturerFinishedAction());
            return;
        }

        dispatcher.Dispatch(new DeletingManufacturerFinishedAction());
        dispatcher.Dispatch(new CloseDeleteManufacturerDialogAction(true));
        _notificationService.NotifySuccess($"Successfully deleted manufacturer {manufacturer.Name}");
    }

    [EffectMethod]
    public Task HandleCloseDeleteManufacturerDialogAction(CloseDeleteManufacturerDialogAction action, IDispatcher dispatcher)
    {
        if (!action.LeaveEditor)
            return Task.CompletedTask;

        if (_leaveEditorTimer is not null)
        {
            _leaveEditorTimer.Stop();
            _leaveEditorTimer.Dispose();
        }

        _leaveEditorTimer = new Timer(Delays.LeaveEditorAfterDelete);
        _leaveEditorTimer.AutoReset = false;
        _leaveEditorTimer.Elapsed += (_, _) => dispatcher.Dispatch(new LeaveManufacturerEditorAction());
        _leaveEditorTimer.Start();

        return Task.CompletedTask;
    }
}