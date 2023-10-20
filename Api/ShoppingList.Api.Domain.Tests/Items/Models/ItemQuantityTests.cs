using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Models;

public class ItemQuantityTests
{
    public class Ctor
    {
        private readonly CtorFixture _fixture = new();

        [Fact]
        public void Ctor_WithTypeWeight_WithInPacketNull_ShouldNotThrow()
        {
            // Arrange
            _fixture.SetupTypeWeight();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Type);

            // Act
            var result = new ItemQuantity(_fixture.Type.Value, null);

            // Assert
            result.Type.Should().Be(_fixture.Type);
            result.InPacket.Should().BeNull();
        }

        [Fact]
        public void Ctor_WithTypeWeight_WithInPacketNotNull_ShouldThrow()
        {
            // Arrange
            _fixture.SetupTypeWeight();
            _fixture.SetupQuantityInPacket();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Type);
            TestPropertyNotSetException.ThrowIfNull(_fixture.QuantityInPacket);

            // Act
            var func = () => new ItemQuantity(_fixture.Type.Value, _fixture.QuantityInPacket);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.QuantityTypeHasInPacketValues);
        }

        [Fact]
        public void Ctor_WithTypeUnit_WithInPacketNull_ShouldThrow()
        {
            // Arrange
            _fixture.SetupTypeUnit();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Type);

            // Act
            var func = () => new ItemQuantity(_fixture.Type.Value, null);

            // Assert
            func.Should().ThrowDomainException(ErrorReasonCode.QuantityTypeHasNoInPacketValues);
        }

        [Fact]
        public void Ctor_WithTypeUnit_WithInPacketNotNull_ShouldNotThrow()
        {
            // Arrange
            _fixture.SetupTypeUnit();
            _fixture.SetupQuantityInPacket();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Type);
            TestPropertyNotSetException.ThrowIfNull(_fixture.QuantityInPacket);

            // Act
            var result = new ItemQuantity(_fixture.Type.Value, _fixture.QuantityInPacket);

            // Assert
            result.Type.Should().Be(_fixture.Type);
            result.InPacket.Should().Be(_fixture.QuantityInPacket);
        }

        private sealed class CtorFixture
        {
            public QuantityType? Type { get; private set; }
            public ItemQuantityInPacket? QuantityInPacket { get; private set; }

            public void SetupTypeWeight()
            {
                Type = QuantityType.Weight;
            }

            public void SetupTypeUnit()
            {
                Type = QuantityType.Unit;
            }

            public void SetupQuantityInPacket()
            {
                QuantityInPacket = new DomainTestBuilder<ItemQuantityInPacket>().Create();
            }
        }
    }
}