using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ProjectHermes.Xipona.Api.TestTools.AutoFixture;

public static partial class EquivalencyOptionsExtensions
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

    [GeneratedRegex("RowVersion$")]
    private static partial Regex RowVersion();

    public static EquivalencyOptions<T> WithCreatedAtPrecision<T>(this EquivalencyOptions<T> options,
        TimeSpan? precision = null)
    {
        precision ??= 1.Milliseconds();

        return options
            .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, precision.Value))
            .When(info => info.Path.EndsWith("CreatedAt"));
    }

    public static EquivalencyOptions<T> WithUpdatedOnPrecision<T>(this EquivalencyOptions<T> options)
    {
        return options
            .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1.Milliseconds()))
            .When(info => info.Path.EndsWith("UpdatedOn"));
    }

    public static EquivalencyOptions<T> ExcludeRowVersion<T>(this EquivalencyOptions<T> options)
    {
        return options.Excluding(info => RowVersion().IsMatch(info.Path));
    }

    public static EquivalencyOptions<T> ExcludeItemCycleRef<T>(this EquivalencyOptions<T> options)
    {
        return options.Excluding(info => info.Path == "Predecessor"
                                         || ItemTypesItemCycle().IsMatch(info.Path)
                                         || ItemTypesPredecessorCycle().IsMatch(info.Path)
                                         || AvailableAtItemTypeCycle().IsMatch(info.Path)
                                         || AvailableAtItemCycle().IsMatch(info.Path));
    }

    public static EquivalencyOptions<T> ExcludeItemTypeId<T>(this EquivalencyOptions<T> options)
    {
        return options.Excluding(info => ItemTypesId().IsMatch(info.Path));
    }

    [GeneratedRegex(@"ItemsOnList\[\d+\].ShoppingList")]
    private static partial Regex ItemsOnListShoppingListCycle();

    [GeneratedRegex(@"Discounts\[\d+\].ShoppingList")]
    private static partial Regex DiscountsShoppingListCycle();

    public static EquivalencyOptions<T> ExcludeShoppingListCycleRef<T>(this EquivalencyOptions<T> options)
    {
        return options.Excluding(info =>
            ItemsOnListShoppingListCycle().IsMatch(info.Path)
            || DiscountsShoppingListCycle().IsMatch(info.Path));
    }

    [GeneratedRegex(@"ItemsOnList\[\d+\].Id")]
    private static partial Regex ItemsOnListId();

    public static EquivalencyOptions<T> ExcludeItemsOnListId<T>(this EquivalencyOptions<T> options)
    {
        return options.Excluding(info => ItemsOnListId().IsMatch(info.Path));
    }

    [GeneratedRegex(@"Discounts\[\d+\].Id")]
    private static partial Regex DiscountId();

    public static EquivalencyOptions<T> ExcludeDiscountId<T>(this EquivalencyOptions<T> options)
    {
        return options.Excluding(info => DiscountId().IsMatch(info.Path));
    }

    [GeneratedRegex(@"PreparationSteps\[\d+\].Recipe")]
    private static partial Regex PreparationStepsRecipeCycle();

    [GeneratedRegex(@"Ingredients\[\d+\].Recipe")]
    private static partial Regex IngredientsRecipeCycle();

    [GeneratedRegex(@"Tags\[\d+\].Recipe")]
    private static partial Regex RecipeTagRecipeCycle();

    public static EquivalencyOptions<T> ExcludeRecipeCycleRef<T>(this EquivalencyOptions<T> options)
    {
        return options.Excluding(info => PreparationStepsRecipeCycle().IsMatch(info.Path)
                                         || IngredientsRecipeCycle().IsMatch(info.Path)
                                         || RecipeTagRecipeCycle().IsMatch(info.Path));
    }

    public static EquivalencyOptions<T> UsingDateTimeOffsetWithPrecision<T>(
        this EquivalencyOptions<T> options, TimeSpan? precision = null)
    {
        precision ??= TimeSpan.FromSeconds(1);

        return options
            .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, precision.Value))
            .WhenTypeIs<DateTimeOffset>();
    }

    public static EquivalencyOptions<T> UsingDateTimeOffsetWithPrecision<T, TParameter>(
        this EquivalencyOptions<T> options, Expression<Func<T, TParameter>> property, TimeSpan? precision = null)
    {
        var body = (MemberExpression)property.Body;
        return UsingDateTimeOffsetWithPrecision(options, body.Member.Name, precision);
    }

    public static EquivalencyOptions<T> UsingDateTimeOffsetWithPrecision<T>(
        this EquivalencyOptions<T> options, [StringSyntax(StringSyntaxAttribute.Regex)] string pathRegex,
        TimeSpan? precision = null)
    {
        precision ??= TimeSpan.FromSeconds(1);

        return options
            .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, precision.Value))
            .When(info => Regex.IsMatch(info.Path, pathRegex));
    }
}