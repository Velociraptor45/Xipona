namespace ProjectHermes.Xipona.Api.TestTools.Extensions;

public static class StringExtensions
{
    public static string LowerFirstChar(this string str)
    {
        return string.Concat(str[..1].ToLower(), str.AsSpan(1));
    }
}