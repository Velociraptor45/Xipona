using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.Services.Queries;

public class QuantityTranslationServiceTests
{
    public sealed class NormalizeForOneServing
    {
        private readonly QuantityTranslationService _sut;

        public NormalizeForOneServing()
        {
            _sut = new QuantityTranslationService();
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(1, 2, 2)]
        [InlineData(2, 1, 0.5)]
        [InlineData(2, 2.5, 1.25)]
        public void NormalizeForOneServing_WithIngredientUnit_WithItemUnit_ShouldReturnCorrectResult(
            int numberOfServingsRaw, float ingredientQuantityRaw, float expectedQuantityRaw)
        {
            // Arrange
            var numberOfServings = new NumberOfServings(numberOfServingsRaw);
            var ingredientQuantityType = IngredientQuantityType.Unit;
            var ingredientQuantity = new IngredientQuantity(ingredientQuantityRaw);
            var itemQuantity = new ItemQuantity(QuantityType.Unit, new ItemQuantityInPacketBuilder().Create());

            // Act
            var result = _sut.NormalizeForOneServing(numberOfServings, ingredientQuantityType, ingredientQuantity,
                itemQuantity);

            // Assert
            result.QuantityType.Should().Be(QuantityType.Unit);
            result.Quantity.Should().Be(new Quantity(expectedQuantityRaw));
        }

        [Theory]
        [InlineData(1, 1, 100)]
        [InlineData(1, 2, 200)]
        [InlineData(2, 1, 50)]
        [InlineData(2, 2.5, 125)]
        public void NormalizeForOneServing_WithIngredientUnit_WithItemWeight_ShouldReturnCorrectResult(
            int numberOfServingsRaw, float ingredientQuantityRaw, float expectedQuantityRaw)
        {
            // Arrange
            var numberOfServings = new NumberOfServings(numberOfServingsRaw);
            var ingredientQuantityType = IngredientQuantityType.Unit;
            var ingredientQuantity = new IngredientQuantity(ingredientQuantityRaw);
            var itemQuantity = new ItemQuantity(QuantityType.Weight, null);

            // Act
            var result = _sut.NormalizeForOneServing(numberOfServings, ingredientQuantityType, ingredientQuantity,
                itemQuantity);

            // Assert
            result.QuantityType.Should().Be(QuantityType.Weight);
            result.Quantity.Should().Be(new Quantity(expectedQuantityRaw));
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(1, 2, 2)]
        [InlineData(2, 1, 0.5)]
        [InlineData(2, 2.5, 1.25)]
        public void NormalizeForOneServing_WithIngredientWeight_WithItemWeight_ShouldReturnCorrectResult(
            int numberOfServingsRaw, float ingredientQuantityRaw, float expectedQuantityRaw)
        {
            // Arrange
            var numberOfServings = new NumberOfServings(numberOfServingsRaw);
            var ingredientQuantityType = IngredientQuantityType.Weight;
            var ingredientQuantity = new IngredientQuantity(ingredientQuantityRaw);
            var itemQuantity = new ItemQuantity(QuantityType.Weight, null);

            // Act
            var result = _sut.NormalizeForOneServing(numberOfServings, ingredientQuantityType, ingredientQuantity,
                itemQuantity);

            // Assert
            result.QuantityType.Should().Be(QuantityType.Weight);
            result.Quantity.Should().Be(new Quantity(expectedQuantityRaw));
        }

        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(1, 2, 2, 1)]
        [InlineData(1, 2, 1, 2)]
        [InlineData(2, 1, 1, 0.5)]
        [InlineData(2, 2.5, 1, 1.25)]
        [InlineData(2, 2.5, 2, 0.625)]
        public void NormalizeForOneServing_WithIngredientWeight_WithItemUnit_ShouldReturnCorrectResult(
            int numberOfServingsRaw, float ingredientQuantityRaw, float quantityInPacket, float expectedQuantityRaw)
        {
            // Arrange
            var numberOfServings = new NumberOfServings(numberOfServingsRaw);
            var ingredientQuantityType = IngredientQuantityType.Weight;
            var ingredientQuantity = new IngredientQuantity(ingredientQuantityRaw);
            var itemQuantity = new ItemQuantity(
                QuantityType.Unit,
                new ItemQuantityInPacketBuilder().WithQuantity(new Quantity(quantityInPacket)).Create());

            // Act
            var result = _sut.NormalizeForOneServing(numberOfServings, ingredientQuantityType, ingredientQuantity,
                itemQuantity);

            // Assert
            result.QuantityType.Should().Be(QuantityType.Unit);
            result.Quantity.Should().Be(new Quantity(expectedQuantityRaw));
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(1, 2, 2)]
        [InlineData(2, 1, 0.5)]
        [InlineData(2, 2.5, 1.25)]
        public void NormalizeForOneServing_WithIngredientFluid_WithItemWeight_ShouldReturnCorrectResult(
            int numberOfServingsRaw, float ingredientQuantityRaw, float expectedQuantityRaw)
        {
            // Arrange
            var numberOfServings = new NumberOfServings(numberOfServingsRaw);
            var ingredientQuantityType = IngredientQuantityType.Fluid;
            var ingredientQuantity = new IngredientQuantity(ingredientQuantityRaw);
            var itemQuantity = new ItemQuantity(QuantityType.Weight, null);

            // Act
            var result = _sut.NormalizeForOneServing(numberOfServings, ingredientQuantityType, ingredientQuantity,
                itemQuantity);

            // Assert
            result.QuantityType.Should().Be(QuantityType.Weight);
            result.Quantity.Should().Be(new Quantity(expectedQuantityRaw));
        }

        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(1, 2, 2, 1)]
        [InlineData(1, 2, 1, 2)]
        [InlineData(2, 1, 1, 0.5)]
        [InlineData(2, 2.5, 1, 1.25)]
        [InlineData(2, 2.5, 2, 0.625)]
        public void NormalizeForOneServing_WithIngredientFluid_WithItemUnit_ShouldReturnCorrectResult(
            int numberOfServingsRaw, float ingredientQuantityRaw, float quantityInPacket, float expectedQuantityRaw)
        {
            // Arrange
            var numberOfServings = new NumberOfServings(numberOfServingsRaw);
            var ingredientQuantityType = IngredientQuantityType.Fluid;
            var ingredientQuantity = new IngredientQuantity(ingredientQuantityRaw);
            var itemQuantity = new ItemQuantity(QuantityType.Unit,
                new ItemQuantityInPacketBuilder().WithQuantity(new Quantity(quantityInPacket)).Create());

            // Act
            var result = _sut.NormalizeForOneServing(numberOfServings, ingredientQuantityType, ingredientQuantity,
                itemQuantity);

            // Assert
            result.QuantityType.Should().Be(QuantityType.Unit);
            result.Quantity.Should().Be(new Quantity(expectedQuantityRaw));
        }

        [Theory]
        [InlineData(1, 1, 10 * 1.8)]
        [InlineData(1, 2, 20 * 1.8)]
        [InlineData(2, 1, 5 * 1.8)]
        [InlineData(2, 2.5, 12.5 * 1.8)]
        public void NormalizeForOneServing_WithIngredientTablespoon_WithItemWeight_ShouldReturnCorrectResult(
            int numberOfServingsRaw, float ingredientQuantityRaw, float expectedQuantityRaw)
        {
            // Arrange
            var numberOfServings = new NumberOfServings(numberOfServingsRaw);
            var ingredientQuantityType = IngredientQuantityType.Tablespoon;
            var ingredientQuantity = new IngredientQuantity(ingredientQuantityRaw);
            var itemQuantity = new ItemQuantity(QuantityType.Weight, null);

            // Act
            var result = _sut.NormalizeForOneServing(numberOfServings, ingredientQuantityType, ingredientQuantity,
                itemQuantity);

            // Assert
            result.QuantityType.Should().Be(QuantityType.Weight);
            result.Quantity.Value.Should().Be(new Quantity(expectedQuantityRaw));
        }

        [Theory]
        [InlineData(1, 1, 1, 10 * 1.8)]
        [InlineData(1, 2, 2, 10 * 1.8)]
        [InlineData(1, 2, 1, 20 * 1.8)]
        [InlineData(2, 1, 1, 5 * 1.8)]
        [InlineData(2, 2.5, 1, 12.5 * 1.8)]
        [InlineData(2, 2.5, 2, 6.25 * 1.8)]
        public void NormalizeForOneServing_WithIngredientTablespoon_WithItemUnit_ShouldReturnCorrectResult(
            int numberOfServingsRaw, float ingredientQuantityRaw, float quantityInPacket, float expectedQuantityRaw)
        {
            // Arrange
            var numberOfServings = new NumberOfServings(numberOfServingsRaw);
            var ingredientQuantityType = IngredientQuantityType.Tablespoon;
            var ingredientQuantity = new IngredientQuantity(ingredientQuantityRaw);
            var itemQuantity = new ItemQuantity(QuantityType.Unit,
                               new ItemQuantityInPacketBuilder().WithQuantity(new Quantity(quantityInPacket)).Create());

            // Act
            var result = _sut.NormalizeForOneServing(numberOfServings, ingredientQuantityType, ingredientQuantity,
                itemQuantity);

            // Assert
            result.QuantityType.Should().Be(QuantityType.Unit);
            result.Quantity.Should().Be(new Quantity(expectedQuantityRaw));
        }

        [Theory]
        [InlineData(1, 1, 10 * 0.6)]
        [InlineData(1, 2, 20 * 0.6)]
        [InlineData(2, 1, 5 * 0.6)]
        [InlineData(2, 2.5, 12.5 * 0.6)]
        public void NormalizeForOneServing_WithIngredientTeaspoon_WithItemWeight_ShouldReturnCorrectResult(
            int numberOfServingsRaw, float ingredientQuantityRaw, float expectedQuantityRaw)
        {
            // Arrange
            var numberOfServings = new NumberOfServings(numberOfServingsRaw);
            var ingredientQuantityType = IngredientQuantityType.Teaspoon;
            var ingredientQuantity = new IngredientQuantity(ingredientQuantityRaw);
            var itemQuantity = new ItemQuantity(QuantityType.Weight, null);

            // Act
            var result = _sut.NormalizeForOneServing(numberOfServings, ingredientQuantityType, ingredientQuantity,
                itemQuantity);

            // Assert
            result.QuantityType.Should().Be(QuantityType.Weight);
            result.Quantity.Value.Should().Be(new Quantity(expectedQuantityRaw));
        }

        [Theory]
        [InlineData(1, 1, 1, 10 * 0.6)]
        [InlineData(1, 2, 2, 10 * 0.6)]
        [InlineData(1, 2, 1, 20 * 0.6)]
        [InlineData(2, 1, 1, 5 * 0.6)]
        [InlineData(2, 2.5, 1, 12.5 * 0.6)]
        [InlineData(2, 2.5, 2, 6.25 * 0.6)]
        public void NormalizeForOneServing_WithIngredientTeaspoon_WithItemUnit_ShouldReturnCorrectResult(
            int numberOfServingsRaw, float ingredientQuantityRaw, float quantityInPacket, float expectedQuantityRaw)
        {
            // Arrange
            var numberOfServings = new NumberOfServings(numberOfServingsRaw);
            var ingredientQuantityType = IngredientQuantityType.Teaspoon;
            var ingredientQuantity = new IngredientQuantity(ingredientQuantityRaw);
            var itemQuantity = new ItemQuantity(QuantityType.Unit,
                               new ItemQuantityInPacketBuilder().WithQuantity(new Quantity(quantityInPacket)).Create());

            // Act
            var result = _sut.NormalizeForOneServing(numberOfServings, ingredientQuantityType, ingredientQuantity,
                itemQuantity);

            // Assert
            result.QuantityType.Should().Be(QuantityType.Unit);
            result.Quantity.Should().Be(new Quantity(expectedQuantityRaw));
        }
    }
}