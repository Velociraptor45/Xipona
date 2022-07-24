using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.Models;

public class IngredientQuantityTests
{
    private readonly IngredientQuantityFixture _fixture;

    public IngredientQuantityTests()
    {
        _fixture = new IngredientQuantityFixture();
    }

    [Fact]
    public void Ctor_WithValueNegative_ShouldThrow()
    {
        // Arrange
        _fixture.SetupValueNegative();

        // Act
        var func = () => new IngredientQuantity(_fixture.Value);

        // Assert
        func.Should().ThrowDomainException(ErrorReasonCode.IngredientQuantityNotValid);
    }

    [Fact]
    public void Ctor_WithValueZero_ShouldThrow()
    {
        // Act
        var func = () => new IngredientQuantity(0);

        // Assert
        func.Should().ThrowDomainException(ErrorReasonCode.IngredientQuantityNotValid);
    }

    [Fact]
    public void Ctor_WithValuePositive_ShouldNotThrow()
    {
        // Arrange
        _fixture.SetupValuePositive();

        // Act
        var func = () => new IngredientQuantity(_fixture.Value);

        // Assert
        func.Should().NotThrow();
    }

    private class IngredientQuantityFixture
    {
        public float Value { get; private set; }

        public void SetupValueNegative()
        {
            Value = -Math.Abs(new TestBuilder<float>().Create());
        }

        public void SetupValuePositive()
        {
            Value = Math.Abs(new TestBuilder<float>().Create());
        }
    }
}