namespace ProjectHermes.Xipona.Api.Domain.Common.Attributes;

public class CurrencySymbolAttribute : Attribute
{
    public CurrencySymbolAttribute(string symbol)
    {
        Symbol = symbol;
    }

    public string Symbol { get; }
}