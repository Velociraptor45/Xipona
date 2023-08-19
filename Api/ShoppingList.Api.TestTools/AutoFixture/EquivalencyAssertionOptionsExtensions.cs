using FluentAssertions;
using FluentAssertions.Equivalency;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ProjectHermes.ShoppingList.Api.TestTools.AutoFixture;

public static partial class EquivalencyAssertionOptionsExtensions
{
    [GeneratedRegex(@"ItemTypes\[\d+\].Id")]
    private static partial Regex ItemTypesId();

    [GeneratedRegex(@"ItemTypes\[\d+\].Item")]
    private static partial Regex ItemTypesItemCycle();

    [GeneratedRegex(@"ItemTypes\[\d+\].Predecessor\b")]
    private static partial Regex ItemTypesPredecessorCycle();

    [GeneratedRegex(@"AvailableAt\[\d+\].ItemType")]
    private static partial Regex AvailableAtItemTypeCycle();

    [GeneratedRegex(@"AvailableAt\[\d+\].Item")]
    private static partial Regex AvailableAtItemCycle();

    [GeneratedRegex(@"RowVersion$")]
    private static partial Regex RowVersion();

    public static EquivalencyAssertionOptions<T> ExcludeRowVersion<T>(this EquivalencyAssertionOptions<T> options)
    {
        return options.Excluding(info => RowVersion().IsMatch(info.Path));
    }

    public static EquivalencyAssertionOptions<T> ExcludeItemCycleRef<T>(this EquivalencyAssertionOptions<T> options)
    {
        return options.Excluding(info => ItemTypesItemCycle().IsMatch(info.Path)
                                         || ItemTypesPredecessorCycle().IsMatch(info.Path)
                                         || AvailableAtItemTypeCycle().IsMatch(info.Path)
                                         || AvailableAtItemCycle().IsMatch(info.Path));
    }

    public static EquivalencyAssertionOptions<T> ExcludeItemTypeId<T>(this EquivalencyAssertionOptions<T> options)
    {
        return options.Excluding(info => ItemTypesId().IsMatch(info.Path));
    }

    [GeneratedRegex(@"ItemsOnList\[\d+\].ShoppingList")]
    private static partial Regex ItemsOnListShoppingListCycle();

    public static EquivalencyAssertionOptions<T> ExcludeShoppingListCycleRef<T>(this EquivalencyAssertionOptions<T> options)
    {
        return options.Excluding(info => ItemsOnListShoppingListCycle().IsMatch(info.Path));
    }

    [GeneratedRegex(@"PreparationSteps\[\d+\].Recipe")]
    private static partial Regex PreparationStepsRecipeCycle();

    [GeneratedRegex(@"Ingredients\[\d+\].Recipe")]
    private static partial Regex IngredientsRecipeCycle();

    [GeneratedRegex(@"Tags\[\d+\].Recipe")]
    private static partial Regex RecipeTagRecipeCycle();

    public static EquivalencyAssertionOptions<T> ExcludeRecipeCycleRef<T>(this EquivalencyAssertionOptions<T> options)
    {
        return options.Excluding(info => PreparationStepsRecipeCycle().IsMatch(info.Path)
                                         || IngredientsRecipeCycle().IsMatch(info.Path)
                                         || RecipeTagRecipeCycle().IsMatch(info.Path));
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