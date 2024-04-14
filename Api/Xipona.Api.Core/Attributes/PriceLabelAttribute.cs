namespace ProjectHermes.Xipona.Api.Core.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class PriceLabelAttribute : Attribute
{
    public PriceLabelAttribute(string priceLabel)
    {
        PriceLabel = priceLabel;
    }

    public string PriceLabel { get; }
}