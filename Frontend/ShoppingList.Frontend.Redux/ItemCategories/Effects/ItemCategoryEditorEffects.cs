using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Constants;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ItemCategories;
using RestEase;
using Timer = System.Timers.Timer;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.Effects;

public class ItemCategoryEditorEffects
{
    private readonly IApiClient _client;
    private readonly NavigationManager _navigationManager;
    private readonly IState<ItemCategoryState> _state;
    private readonly IShoppingListNotificationService _notificationService;

    private Timer? _leaveEditorTimer;

    public ItemCategoryEditorEffects(IApiClient client, NavigationManager navigationManager,
        IState<ItemCategoryState> state, IShoppingListNotificationService notificationService)
    {
        _client = client;
        _navigationManager = navigationManager;
        _state = state;
        _notificationService = notificationService;
    }

    [EffectMethod]
    public Task HandleLeaveItemCategoryEditorAction(LeaveItemCategoryEditorAction action, IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo(PageRoutes.ItemCategories);

        if(action.TriggeredBySave)
            dispatcher.Dispatch(new SearchItemCategoriesAction());

        return Task.CompletedTask;
    }

    [EffectMethod]
    public async Task HandleLoadItemCategoryForEditingAction(LoadItemCategoryForEditingAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new LoadItemCategoryForEditingStartedAction());

        EditedItemCategory itemCategory;

        try
        {
            itemCategory = await _client.GetItemCategoryByIdAsync(action.ItemCategoryId);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Loading item category failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Loading item category failed", e.Message));
            return;
        }

        var finishAction = new LoadItemCategoryForEditingFinishedAction(itemCategory);
        dispatcher.Dispatch(finishAction);
    }

    [EffectMethod(typeof(SaveItemCategoryAction))]
    public async Task HandleSaveItemCategoryAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SaveItemCategoryStartedAction());

        var editor = _state.Value.Editor;
        if (editor.ItemCategory!.Id == Guid.Empty)
        {
            try
            {
                await _client.CreateItemCategoryAsync(editor.ItemCategory.Name);
            }
            catch (ApiException e)
            {
                dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Creating item category failed", e));
                dispatcher.Dispatch(new SaveItemCategoryFinishedAction());
                return;
            }
            catch (HttpRequestException e)
            {
                dispatcher.Dispatch(new DisplayErrorNotificationAction("Creating item category failed", e.Message));
                dispatcher.Dispatch(new SaveItemCategoryFinishedAction());
                return;
            }
            _notificationService.NotifySuccess($"Successfully created item category {editor.ItemCategory.Name}");
        }
        else
        {
            try
            {
                await _client.ModifyItemCategoryAsync(
                        new ModifyItemCategoryRequest(editor.ItemCategory.Id, editor.ItemCategory.Name));
            }
            catch (ApiException e)
            {
                dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Saving item category failed", e));
                dispatcher.Dispatch(new SaveItemCategoryFinishedAction());
                return;
            }
            catch (HttpRequestException e)
            {
                dispatcher.Dispatch(new DisplayErrorNotificationAction("Saving item category failed", e.Message));
                dispatcher.Dispatch(new SaveItemCategoryFinishedAction());
                return;
            }

            var updateAction = new UpdateItemCategorySearchResultsAfterSaveAction(
                editor.ItemCategory.Id,
                editor.ItemCategory.Name);
            dispatcher.Dispatch(updateAction);
            _notificationService.NotifySuccess($"Successfully modified item category {editor.ItemCategory.Name}");
        }

        dispatcher.Dispatch(new SaveItemCategoryFinishedAction());
        dispatcher.Dispatch(new LeaveItemCategoryEditorAction(true));
    }

    [EffectMethod(typeof(DeleteItemCategoryAction))]
    public async Task HandleDeleteItemCategoryAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new DeleteItemCategoryStartedAction());

        var itemCategory = _state.Value.Editor.ItemCategory!;
        try
        {
            await _client.DeleteItemCategoryAsync(itemCategory.Id);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Deleting item category failed", e));
            dispatcher.Dispatch(new DeleteItemCategoryFinishedAction());
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Deleting item category failed", e.Message));
            dispatcher.Dispatch(new DeleteItemCategoryFinishedAction());
            return;
        }

        dispatcher.Dispatch(new DeleteItemCategoryFinishedAction());
        dispatcher.Dispatch(new CloseDeleteItemCategoryDialogAction(true));
        _notificationService.NotifySuccess($"Successfully deleted item category {itemCategory.Name}");
    }

    [EffectMethod]
    public Task HandleCloseDeleteItemCategoryDialogAction(CloseDeleteItemCategoryDialogAction action, IDispatcher dispatcher)
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
        _leaveEditorTimer.Elapsed += (_, _) => dispatcher.Dispatch(new LeaveItemCategoryEditorAction(true));
        _leaveEditorTimer.Start();

        return Task.CompletedTask;
    }
}