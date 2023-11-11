using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Models;

public class QuantityTests
{
    public class Ctor
    {
        [Fact]
        public void Ctor_WithoutArgument_ShouldThrow()
        {
            // Act
            var action = () => new Quantity();

            // Assert
            action.Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Ctor_WithQuantityZero_ShouldThrow()
        {
            // Act
            var action = () => new Quantity(0);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.InvalidQuantity);
        }

        [Fact]
        public void Ctor_WithQuantityNegative_ShouldThrow()
        {
            // Arrange
            var price = new TestBuilder<float>().Create();

            // Act
            var action = () => new Quantity(-price);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.InvalidQuantity);
        }

        [Fact]
        public void Ctor_WithQuantityPositive_ShouldNotThrow()
        {
            // Arrange
            var price = new TestBuilder<float>().Create();

            // Act
            var result = new Quantity(price);

            // Assert
            result.Value.Should().Be(price);
        }
    }
}