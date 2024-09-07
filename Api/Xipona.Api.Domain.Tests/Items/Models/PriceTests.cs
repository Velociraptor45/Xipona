using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common.Extensions.FluentAssertions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Items.Models;

public class PriceTests
{
    public class Ctor
    {
        [Fact]
        public void Ctor_WithoutArgument_ShouldThrow()
        {
            // Act
            var action = () => new Price();

            // Assert
            action.Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Ctor_WithPriceZero_ShouldThrow()
        {
            // Act
            var action = () => new Price(0);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.PriceNotValid);
        }

        [Fact]
        public void Ctor_WithPriceNegative_ShouldThrow()
        {
            // Arrange
            var price = new TestBuilder<decimal>().Create();

            // Act
            var action = () => new Price(-price);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.PriceNotValid);
        }

        [Fact]
        public void Ctor_WithPricePositive_ShouldNotThrow()
        {
            // Arrange
            var price = new TestBuilder<decimal>().Create();

            // Act
            var result = new Price(price);

            // Assert
            result.Value.Should().Be(price);
        }
    }
}