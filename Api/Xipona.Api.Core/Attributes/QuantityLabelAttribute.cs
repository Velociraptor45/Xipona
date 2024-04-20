namespace ProjectHermes.Xipona.Api.Core.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class QuantityLabelAttribute : Attribute
{
    public QuantityLabelAttribute(string quantityLabel)
    {
        QuantityLabel = quantityLabel;
    }

    public string QuantityLabel { get; }
}