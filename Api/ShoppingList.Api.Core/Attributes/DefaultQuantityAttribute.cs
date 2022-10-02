namespace ProjectHermes.ShoppingList.Api.Core.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class DefaultQuantityAttribute : Attribute
{
    public DefaultQuantityAttribute(int defaultQuantity)
    {
        DefaultQuantity = defaultQuantity;
    }

    public int DefaultQuantity { get; }
}