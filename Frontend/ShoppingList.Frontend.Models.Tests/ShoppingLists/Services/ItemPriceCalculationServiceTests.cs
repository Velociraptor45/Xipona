using FluentAssertions;
using Fluxor;
using Moq;
using ProjectHermes.ShoppingList.Frontend.Models.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
using ProjectHermes.ShoppingList.Frontend.TestTools.AutoFixture.Builder;
using ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Index.Services;
using ShoppingList.Frontend.Redux.ShoppingList.States;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ProjectHermes.ShoppingList.Frontend.Models.Tests.ShoppingLists.Services;

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
        var sut = _fixture.CreateSut();
        var quantityTypeId = _fixture.QuantityTypes.First().Id;

        // Act
        var result = sut.CalculatePrice(quantityTypeId, pricePerQuantity, quantity);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void CalculatePrice_WithInvalidQuantityId_ShouldThrow()
    {
        // Arrange
        var sut = _fixture.CreateSut();
        var quantityTypeId = new IntBuilder().CreatePositive();

        // Act
        var func = () => sut.CalculatePrice(quantityTypeId, 1, 1);

        // Assert
        func.Should().ThrowExactly<InvalidOperationException>()
            .WithMessage($"Quantity type {quantityTypeId} not recognized.");
    }

    private class ItemPriceCalculationServiceFixture
    {
        public List<QuantityType> QuantityTypes { get; private set; } = new();

        public ItemPriceCalculationService CreateSut()
        {
            var mock = new Mock<IState<ShoppingListState>>(MockBehavior.Strict);
            var sut = new ItemPriceCalculationService(mock.Object);
            //sut.Initialize(QuantityTypes);
            return sut;
        }

        public void SetupQuantityType(int quantityNormalizer)
        {
            QuantityTypes = new QuantityTypeBuilder()
                .WithQuantityNormalizer(quantityNormalizer)
                .CreateMany(1).ToList();
        }
    }
}