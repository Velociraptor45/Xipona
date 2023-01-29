using Fluxor;
using Microsoft.AspNetCore.Components;
using ShoppingList.Frontend.Redux.ItemCategories.Actions;
using ShoppingList.Frontend.Redux.ItemCategories.States;
using ShoppingList.Frontend.Redux.Shared.Constants;
using ShoppingList.Frontend.Redux.Shared.Ports;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.ItemCategories;

namespace ShoppingList.Frontend.Redux.ItemCategories.Effects;

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

        var itemCategory = await _client.GetItemCategoryByIdAsync(action.ItemCategoryId);

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
            await _client.CreateItemCategoryAsync(editor.ItemCategory.Name);
        }
        else
        {
            await _client.ModifyItemCategoryAsync(
                new ModifyItemCategoryRequest(editor.ItemCategory.Id, editor.ItemCategory.Name));
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

        await _client.DeleteItemCategoryAsync(_state.Value.Editor.ItemCategory!.Id);

        dispatcher.Dispatch(new DeleteItemCategoryFinishedAction());
        dispatcher.Dispatch(new LeaveItemCategoryEditorAction());
    }
}