namespace ProjectHermes.ShoppingList.Api.Core.Extensions;

public static class IntExtensions
{
    public static T ToEnum<T>(this int value)
        where T : Enum
    {
        if (!Enum.IsDefined(typeof(T), value))
            throw new InvalidOperationException($"{value} is an invalid {typeof(T).FullName} value.");

        return (T)(object)value;
    }
}