namespace ProjectHermes.ShoppingList.Api.Core.Extensions;

public static class ObjectExtensions
{
    public static List<T> ToMonoList<T>(this T obj)
    {
        return new List<T> { obj };
    }
}