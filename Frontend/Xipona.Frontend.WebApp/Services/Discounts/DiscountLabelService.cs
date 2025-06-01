using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using System;

namespace ProjectHermes.Xipona.Frontend.WebApp.Services.Discounts;

public class DiscountLabelService : IDiscountLabelService
{
    private readonly IState<SharedState> _sharedState;

    public DiscountLabelService(IState<SharedState> sharedState)
    {
        _sharedState = sharedState;
    }

    public string GetLabel(ShoppingListDiscountType type)
    {
        return type switch
        {
            ShoppingListDiscountType.Price => _sharedState.Value.Settings.GeneralSettings!.Currency.Symbol,
            ShoppingListDiscountType.Percentage => "%",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
