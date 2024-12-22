using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor.SideDishes;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;
using RestEase;
using Timer = System.Timers.Timer;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.Effects;
public sealed class SideDishSelectorEffects : IAsyncDisposable
{
    private readonly IApiClient _client;
    private readonly IState<RecipeState> _state;

    private Timer? _startSearchTimer;

    public SideDishSelectorEffects(IApiClient client, IState<RecipeState> state)
    {
        _client = client;
        _state = state;
    }

    [EffectMethod]
    public Task HandleSideDishInputChangedAction(SideDishInputChangedAction action, IDispatcher dispatcher)
    {
        if (_startSearchTimer is not null)
        {
            _startSearchTimer.Stop();
            _startSearchTimer.Dispose();
        }

        if (string.IsNullOrWhiteSpace(action.Input))
        {
            dispatcher.Dispatch(new SearchSideDishesFinishedAction(new List<RecipeSearchResult>()));
            return Task.CompletedTask;
        }

        _startSearchTimer = new(300d);
        _startSearchTimer.AutoReset = false;
        _startSearchTimer.Elapsed += (_, _) => dispatcher.Dispatch(new SearchSideDishesAction());
        _startSearchTimer.Start();

        return Task.CompletedTask;
    }

    [EffectMethod]
    public async Task HandleSearchSideDishesAction(SearchSideDishesAction action, IDispatcher dispatcher)
    {
        if (_state.Value.Editor.Recipe is null)
            return;

        IEnumerable<RecipeSearchResult> results;
        try
        {
            results = await _client.SearchRecipesByNameAsync(_state.Value.Editor.SideDishSelector.Input);
        }
        catch (ApiException e)
        {
            dispatcher.Dispatch(new DisplayApiExceptionNotificationAction("Searching for item categories failed", e));
            return;
        }
        catch (HttpRequestException e)
        {
            dispatcher.Dispatch(new DisplayErrorNotificationAction("Searching for item categories failed", e.Message));
            return;
        }

        dispatcher.Dispatch(new SearchSideDishesFinishedAction(results.ToList()));
    }


    public ValueTask DisposeAsync()
    {
        _startSearchTimer?.Dispose();
        return ValueTask.CompletedTask;
    }
}
