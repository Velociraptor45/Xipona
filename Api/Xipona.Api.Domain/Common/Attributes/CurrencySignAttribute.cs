namespace ProjectHermes.Xipona.Api.Domain.Common.Attributes;

public class CurrencySignAttribute : Attribute
{
    public CurrencySignAttribute(string sign)
    {
        Sign = sign;
    }

    public string Sign { get; }
}