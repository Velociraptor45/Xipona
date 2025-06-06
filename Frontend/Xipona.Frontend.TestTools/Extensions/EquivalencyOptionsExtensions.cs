using FluentAssertions.Equivalency;
using System.Text.RegularExpressions;

namespace Xipona.Frontend.TestTools.Extensions;

public static class EquivalencyOptionsExtensions
{
    public static EquivalencyOptions<T> ExcludingKey<T>(this EquivalencyOptions<T> options)
    {
        return options.Excluding(info => Regex.IsMatch(info.Path, @"(\[\d+\]\.)?Key"));
    }
}