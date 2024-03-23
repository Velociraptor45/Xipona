namespace ProjectHermes.Xipona.Frontend.TestTools.Extensions;

public static class StringExtensions
{
    public static string LowerFirstChar(this string str)
    {
        return char.ToLowerInvariant(str[0]) + str[1..];
    }
}