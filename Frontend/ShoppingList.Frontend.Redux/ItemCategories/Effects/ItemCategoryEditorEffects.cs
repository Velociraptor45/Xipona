using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Constants;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ItemCategories;
using RestEase;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.Effects;

public class ItemCategoryEditorEffects
{
    private readonly IApiClient _client;
    private readonly NavigationManager _navigationManager;
    private readonly IState<ItemCategoryState> _state;

    public ItemCategoryEditorEffects(IApiClient client, NavigationManager navigationManager,
        IState<ItemCategoryState> state)
    {
        _client = client;
        _navigationManager = navigationManager;
        _state = state;
    }

    [EffectMethod(typeof(LeaveItemCategoryEditorAction))]
    public Task HandleLeaveItemCategoryEditorAction(IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo(PageRoutes.ItemCategories);
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
        }

        dispatcher.Dispatch(new SaveItemCategoryFinishedAction());
        dispatcher.Dispatch(new LeaveItemCategoryEditorAction());
    }

    [EffectMethod(typeof(DeleteItemCategoryAction))]
    public async Task HandleDeleteItemCategoryAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new DeleteItemCategoryStartedAction());

        try
        {
            await _client.DeleteItemCategoryAsync(_state.Value.Editor.ItemCategory!.Id);
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
        dispatcher.Dispatch(new LeaveItemCategoryEditorAction());
    }
}