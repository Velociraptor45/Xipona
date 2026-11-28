namespace Xipona.Api.Domain.Common.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class CurrencySymbolAttribute : Attribute
{
    public CurrencySymbolAttribute(string symbol, bool isTrailing)
    {
        Symbol = symbol;
        IsTrailing = isTrailing;
    }

    public string Symbol { get; }
    public bool IsTrailing { get; }
}