namespace ProjectHermes.ShoppingList.Api.TestTools.Extensions;

public static class StringExtensions
{
    public static string LowerFirstChar(this string str) => str.Substring(0, 1).ToLower() + str.Substring(1);
}