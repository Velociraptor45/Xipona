using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.WebApp.Store.Manufacturers.Actions;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Store.Manufacturers.Effects;
public class ManufacturerEffects
{
    private readonly IApiClient _client;

    public ManufacturerEffects(IApiClient client)
    {
        _client = client;
    }

    [EffectMethod]
    public async Task HandleSearchAction(SearchManufacturersAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SearchManufacturersStartedAction());

        var result = await _client.GetManufacturerSearchResultsAsync(action.SearchInput);

        var finishAction = new SearchManufacturersFinishedAction(result.ToList());
        dispatcher.Dispatch(finishAction);
    }
}
