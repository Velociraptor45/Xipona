using FluentAssertions;
using Moq;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.TestTools.AutoFixture.Builder;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests;

public class ItemPriceCalculationServiceTests
{
    private readonly ItemPriceCalculationServiceFixture _fixture;

    public ItemPriceCalculationServiceTests()
    {
        _fixture = new ItemPriceCalculationServiceFixture();
    }

    [Theory]
    [InlineData(1, 2, 2, 4)]
    [InlineData(1, 1.5f, 1, 1.5f)]
    [InlineData(1000, 1.5f, 300, 0.45f)]
    [InlineData(1000, 1.5f, 512, 0.77f)] // round up above .5
    [InlineData(1000, 1.5f, 470, 0.71f)] // round up at .5
    [InlineData(1000, 1.5f, 462, 0.69f)] // round down
    public void CalculatePrice_ShouldReturnExpectedResult(int quantityNormalizer, float pricePerQuantity,
        float quantity, float expectedResult)
    {
        // Arrange
        _fixture.SetupQuantityType(quantityNormalizer);
        _fixture.SetupValidQuantityTypeId();
        _fixture.SetupStateWithQuantityTypes();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.QuantityTypeId);

        // Act
        var result = sut.CalculatePrice(_fixture.QuantityTypeId.Value, pricePerQuantity, quantity);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void CalculatePrice_WithInvalidQuantityId_ShouldThrow()
    {
        // Arrange
        _fixture.SetupInvalidQuantityTypeId();
        _fixture.SetupStateWithoutQuantityTypes();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.QuantityTypeId);

        // Act
        var func = () => sut.CalculatePrice(_fixture.QuantityTypeId.Value, 1, 1);

        // Assert
        func.Should().ThrowExactly<InvalidOperationException>()
            .WithMessage($"Quantity type {_fixture.QuantityTypeId.Value} not recognized.");
    }

    private class ItemPriceCalculationServiceFixture
    {
        private readonly ShoppingListStateMock _shoppingListStateMock = new(MockBehavior.Strict);
        private List<QuantityType>? _quantityTypes;

        public int? QuantityTypeId { get; private set; }

        public ItemPriceCalculationService CreateSut()
        {
            var sut = new ItemPriceCalculationService(_shoppingListStateMock.Object);
            return sut;
        }

        public void SetupStateWithQuantityTypes()
        {
            TestPropertyNotSetException.ThrowIfNull(_quantityTypes);
            var state = new DomainTestBuilder<ShoppingListState>().Create() with
            {
                QuantityTypes = _quantityTypes
            };
            _shoppingListStateMock.SetupValue(state);
        }

        public void SetupStateWithoutQuantityTypes()
        {
            var state = new DomainTestBuilder<ShoppingListState>().Create();
            _shoppingListStateMock.SetupValue(state);
        }

        public void SetupQuantityType(int quantityNormalizer)
        {
            _quantityTypes =
                new List<QuantityType>()
                {
                    new DomainTestBuilder<QuantityType>().Create() with
                    {
                        QuantityNormalizer = quantityNormalizer
                    }
                };
        }

        public void SetupValidQuantityTypeId()
        {
            TestPropertyNotSetException.ThrowIfNull(_quantityTypes);

            QuantityTypeId = _quantityTypes.First().Id;
        }

        public void SetupInvalidQuantityTypeId()
        {
            QuantityTypeId = new IntBuilder().CreatePositive();
        }
    }
}