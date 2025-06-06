namespace Xipona.Api.Domain.Common.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class CurrencySymbolAttribute : Attribute
{
    public CurrencySymbolAttribute(string symbol)
    {
        Symbol = symbol;
    }

    public string Symbol { get; }
}