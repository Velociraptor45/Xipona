using FluentAssertions;
using FluentAssertions.Equivalency;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ProjectHermes.ShoppingList.Api.TestTools.AutoFixture;

public static class EquivalencyAssertionOptionsExtensions
{
    public static EquivalencyAssertionOptions<T> ExcludeItemCycleRef<T>(this EquivalencyAssertionOptions<T> options)
    {
        return options.Excluding(info => Regex.IsMatch(info.Path, @"ItemTypes\[\d+\].Item")
                                         || Regex.IsMatch(info.Path, @"ItemTypes\[\d+\].Predecessor\b")
                                         || Regex.IsMatch(info.Path, @"AvailableAt\[\d+\].ItemType")
                                         || Regex.IsMatch(info.Path, @"AvailableAt\[\d+\].Item"));
    }

    public static EquivalencyAssertionOptions<T> ExcludeShoppingListCycleRef<T>(this EquivalencyAssertionOptions<T> options)
    {
        return options.Excluding(info => Regex.IsMatch(info.Path, @"ItemsOnList\[\d+\].ShoppingList"));
    }

    public static EquivalencyAssertionOptions<T> ExcludeRecipeCycleRef<T>(this EquivalencyAssertionOptions<T> options)
    {
        return options.Excluding(info => Regex.IsMatch(info.Path, @"PreparationSteps\[\d+\].Recipe")
                                         || Regex.IsMatch(info.Path, @"Ingredients\[\d+\].Recipe")
                                         || Regex.IsMatch(info.Path, @"Ingredients\[\d+\].Id"));
    }

    public static EquivalencyAssertionOptions<T> UsingDateTimeOffsetWithPrecision<T>(
        this EquivalencyAssertionOptions<T> options, TimeSpan? precision = null)
    {
        precision ??= TimeSpan.FromSeconds(1);

        return options
            .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, precision.Value))
            .WhenTypeIs<DateTimeOffset>();
    }

    public static EquivalencyAssertionOptions<T> UsingDateTimeOffsetWithPrecision<T, TParameter>(
        this EquivalencyAssertionOptions<T> options, Expression<Func<T, TParameter>> property, TimeSpan? precision = null)
    {
        var body = (MemberExpression)property.Body;
        return UsingDateTimeOffsetWithPrecision(options, body.Member.Name, precision);
    }

    public static EquivalencyAssertionOptions<T> UsingDateTimeOffsetWithPrecision<T>(
        this EquivalencyAssertionOptions<T> options, [StringSyntax(StringSyntaxAttribute.Regex)] string pathRegex,
        TimeSpan? precision = null)
    {
        precision ??= TimeSpan.FromSeconds(1);

        return options
            .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, precision.Value))
            .When(info => Regex.IsMatch(info.Path, pathRegex));
    }
}