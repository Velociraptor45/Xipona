using Fluxor;
using System.Text.RegularExpressions;
using Xipona.Frontend.Redux.Shared.States;

namespace Xipona.Frontend.WebApp.Services.Prices;

public interface IPriceLabelService
{
    string GetPriceLabel(decimal price, QuantityType quantityType, bool spaceBetweenCurrencyAndValue = false);
    string GetPriceLabel(decimal price, string priceLabel, bool spaceBetweenCurrencyAndValue = false);
    string ParsePrice(string price);
}

public class PriceLabelService(IState<SharedState> SharedState) : IPriceLabelService
{
    public string GetPriceLabel(decimal price, QuantityType quantityType, bool spaceBetweenCurrencyAndValue = false)
    {
        return GetPriceLabel(price, quantityType.PriceLabel, spaceBetweenCurrencyAndValue);
    }
    
    public string GetPriceLabel(decimal price, string priceLabel, bool spaceBetweenCurrencyAndValue = false)
    {
        if (SharedState.Value.Settings.GeneralSettings!.Currency.IsTrailing)
        {
            if (spaceBetweenCurrencyAndValue)
                return $"{price:n2} {SharedState.Value.Settings.GeneralSettings.Currency.Symbol}{priceLabel}".TrimEnd();

            return $"{price:n2}{SharedState.Value.Settings.GeneralSettings.Currency.Symbol}{priceLabel}".TrimEnd();
        }

        if (spaceBetweenCurrencyAndValue)
        {
            return $"{SharedState.Value.Settings.GeneralSettings.Currency.Symbol} {price:n2} {priceLabel}".TrimEnd();
        }
        return $"{SharedState.Value.Settings.GeneralSettings.Currency.Symbol}{price:n2}{priceLabel}".TrimEnd();
    }
    
    public string ParsePrice(string price)
    {
        return Regex.Match(price, @"-?\d+.*\d").Value;
    }
}