namespace ProjectHermes.Xipona.Api.Core.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class QuantityNormalizerAttribute : Attribute
{
    public QuantityNormalizerAttribute(int value)
    {
        Value = value;
    }

    public int Value { get; }
}