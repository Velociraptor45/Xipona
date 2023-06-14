using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Models;

public class QuantityInBasketTests
{
    [Theory]
    [InlineData(1f, 1f, 2f)]
    [InlineData(1f, 2f, 3f)]
    [InlineData(2f, 1f, 3f)]
    public void Add_ShouldReturnExpectedResult(float left, float right, float expectedResult)
    {
        // Arrange
        var quantityLeft = new QuantityInBasket(left);
        var quantityRight = new QuantityInBasket(right);

        // Act
        var result = quantityLeft + quantityRight;

        // Assert
        result.Should().BeEquivalentTo(new QuantityInBasket(expectedResult));
    }

    [Theory]
    [InlineData(2f, 1f, 1f)]
    [InlineData(3f, 2f, 1f)]
    [InlineData(5f, 2f, 3f)]
    public void Subtract_ShouldReturnExpectedResult(float left, float right, float expectedResult)
    {
        // Arrange
        var quantityLeft = new QuantityInBasket(left);
        var quantityRight = new QuantityInBasket(right);

        // Act
        var result = quantityLeft - quantityRight;

        // Assert
        result.Should().BeEquivalentTo(new QuantityInBasket(expectedResult));
    }

    [Theory]
    [InlineData(2f, 1f, 2f)]
    [InlineData(1f, 2f, 2f)]
    [InlineData(1f, 1f, 1f)]
    [InlineData(5f, 2f, 10f)]
    public void Multiply_ShouldReturnExpectedResult(float left, float right, float expectedResult)
    {
        // Arrange
        var quantityLeft = new QuantityInBasket(left);
        var quantityRight = new QuantityInBasket(right);

        // Act
        var result = quantityLeft * quantityRight;

        // Assert
        result.Should().BeEquivalentTo(new QuantityInBasket(expectedResult));
    }

    [Theory]
    [InlineData(2f, 1f, 2f)]
    [InlineData(1f, 2f, 0.5f)]
    [InlineData(1f, 1f, 1f)]
    [InlineData(5f, 2f, 2.5f)]
    public void Divide_ShouldReturnExpectedResult(float left, float right, float expectedResult)
    {
        // Arrange
        var quantityLeft = new QuantityInBasket(left);
        var quantityRight = new QuantityInBasket(right);

        // Act
        var result = quantityLeft / quantityRight;

        // Assert
        result.Should().BeEquivalentTo(new QuantityInBasket(expectedResult));
    }

    [Fact]
    public void Ctor_WithZero_ShouldThrow()
    {
        // Act
        Action action = () => _ = new QuantityInBasket(0f);

        // Assert
        action.Should().ThrowDomainException(ErrorReasonCode.InvalidQuantityInBasket);
    }

    [Fact]
    public void Ctor_WithNegative_ShouldThrow()
    {
        // Act
        Action action = () => _ = new QuantityInBasket(-1f);

        // Assert
        action.Should().ThrowDomainException(ErrorReasonCode.InvalidQuantityInBasket);
    }

    [Fact]
    public void Ctor_WithPositive_ShouldNotThrow()
    {
        // Act
        Action action = () => _ = new QuantityInBasket(1f);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Ctor_WithoutParameter_ShouldThrow()
    {
        // Act
        Action action = () => _ = new QuantityInBasket();

        // Assert
        action.Should().Throw<NotSupportedException>();
    }
}