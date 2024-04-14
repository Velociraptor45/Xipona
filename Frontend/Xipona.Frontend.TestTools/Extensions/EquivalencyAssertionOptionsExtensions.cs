using FluentAssertions.Equivalency;
using System.Text.RegularExpressions;

namespace ProjectHermes.Xipona.Frontend.TestTools.Extensions;

public static class EquivalencyAssertionOptionsExtensions
{
    public static EquivalencyAssertionOptions<T> ExcludingKey<T>(this EquivalencyAssertionOptions<T> options)
    {
        return options.Excluding(info => Regex.IsMatch(info.Path, @"(\[\d+\]\.)?Key"));
    }
}