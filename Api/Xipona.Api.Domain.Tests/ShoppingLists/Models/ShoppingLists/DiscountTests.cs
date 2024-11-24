using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;

namespace ProjectHermes.Xipona.Api.Domain.Tests.ShoppingLists.Models.ShoppingLists;

public class DiscountTests
{
    public class Ctor
    {
        private readonly CtorFixture _fixture = new();

        [Fact]
        public void Ctor_WithNoArguments_ShouldThrowNotSupportedException()
        {
            // Act
            var act = () => new Discount();

            // Assert
            act.Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Ctor_WithoutTypeId_ShouldSetTypeIdNull()
        {
            // Arrange
            var itemId = _fixture.ItemId;
            var price = _fixture.Price;

            // Act
            var discount = new Discount(itemId, price);

            // Assert
            discount.ItemId.Should().Be(itemId);
            discount.ItemTypeId.Should().BeNull();
            discount.Price.Should().Be(price);
        }

        [Fact]
        public void Ctor_WithTypeId_ShouldSetTypeId()
        {
            // Arrange
            var itemId = _fixture.ItemId;
            var itemTypeId = _fixture.ItemTypeId;
            var price = _fixture.Price;

            // Act
            var discount = new Discount(itemId, itemTypeId, price);

            // Assert
            discount.ItemId.Should().Be(itemId);
            discount.ItemTypeId.Should().Be(itemTypeId);
            discount.Price.Should().Be(price);
        }

        private sealed class CtorFixture
        {
            public ItemId ItemId { get; private set; } = ItemId.New;
            public ItemTypeId ItemTypeId { get; private set; } = ItemTypeId.New;
            public Price Price { get; private set; } = new DomainTestBuilder<Price>().Create();
        }
    }
}
