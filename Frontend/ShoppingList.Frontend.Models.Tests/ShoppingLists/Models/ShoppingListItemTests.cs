using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Frontend.Models.TestKit.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Frontend.TestTools.AutoFixture.Builder;
using Xunit;

namespace ProjectHermes.ShoppingList.Frontend.Models.Tests.ShoppingLists.Models;

public class ShoppingListItemTests
{
    private readonly ShoppingListItemFixture _fixture;

    public ShoppingListItemTests()
    {
        _fixture = new ShoppingListItemFixture();
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(0, 1)]
    [InlineData(-1, 1)]
    public void SetQuantity_WithEdgeCases_ShouldReturnExpectedResult(float quantity, float expectedResult)
    {
        // Arrange
        var sut = _fixture.CreateSut();

        // Act
        sut.SetQuantity(quantity);

        // Assert
        sut.Quantity.Should().Be(expectedResult);
    }

    [Fact]
    public void SetQuantity_WithGreaterZero_ShouldReturnExpectedResult()
    {
        // Arrange
        var sut = _fixture.CreateSut();
        _fixture.SetupQuantityGreaterZero();

        // Act
        sut.SetQuantity(_fixture.Quantity);

        // Assert
        sut.Quantity.Should().Be(_fixture.Quantity);
    }

    [Fact]
    public void SetQuantity_WithBelowZero_ShouldReturnExpectedResult()
    {
        // Arrange
        var sut = _fixture.CreateSut();
        _fixture.SetupQuantityBelowZero();

        // Act
        sut.SetQuantity(_fixture.Quantity);

        // Assert
        sut.Quantity.Should().Be(1);
    }

    [Fact]
    public void ChangeQuantity_WithDifferenceBiggerThanQuantity_ShouldCorrectQuantity()
    {
        // Arrange
        _fixture.SetupSutQuantityGreaterZero();
        var sut = _fixture.CreateSut();
        var difference = -(sut.Quantity + 10);

        // Act
        sut.ChangeQuantity(difference);

        // Assert
        sut.Quantity.Should().Be(1);
    }

    [Fact]
    public void ChangeQuantity_WithDifferenceSmallerThanQuantity_ShouldReduceQuantity()
    {
        // Arrange
        _fixture.SetupSutQuantityGreaterZero();
        var sut = _fixture.CreateSut();
        var expectedResult = new IntBuilder().CreatePositive(maxValue: (int)sut.Quantity - 1);
        var difference = -(sut.Quantity - expectedResult);

        // Act
        sut.ChangeQuantity(difference);

        // Assert
        sut.Quantity.Should().Be(expectedResult);
    }

    [Fact]
    public void ChangeQuantity_WithPositiveDifference_ShouldReduceQuantity()
    {
        // Arrange
        _fixture.SetupSutQuantityGreaterZero();
        var sut = _fixture.CreateSut();
        var difference = new IntBuilder().CreatePositive(2000);
        var expectedResult = sut.Quantity + difference;

        // Act
        sut.ChangeQuantity(difference);

        // Assert
        sut.Quantity.Should().Be(expectedResult);
    }

    private class ShoppingListItemFixture
    {
        private readonly ShoppingListItemBuilder _builder;

        public ShoppingListItemFixture()
        {
            _builder = new ShoppingListItemBuilder();
        }

        public int Quantity { get; private set; }

        public ShoppingListItem CreateSut()
        {
            return _builder.Create();
        }

        public void SetupQuantityGreaterZero()
        {
            Quantity = new IntBuilder().CreatePositive();
        }

        public void SetupQuantityBelowZero()
        {
            Quantity = new IntBuilder().CreateNegative();
        }

        public void SetupSutQuantityGreaterZero()
        {
            var quantity = new IntBuilder().CreatePositive(2, 40000);
            _builder.WithQuantity(quantity);
        }
    }
}