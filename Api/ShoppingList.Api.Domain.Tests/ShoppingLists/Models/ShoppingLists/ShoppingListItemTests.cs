using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Models.ShoppingLists;

public class ShoppingListItemTests
{
    public class PutInBasket
    {
        private readonly PutInBasketFixture _fixture = new();

        [Fact]
        public void PutInBasket_WithNotInBasket_ShouldPutItemInBasket()
        {
            // Arrange
            _fixture.SetupNotInBasket();
            _fixture.SetupExpectedResult();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.PutInBasket();

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
            sut.IsInBasket.Should().BeFalse();
            ReferenceEquals(result, sut).Should().BeFalse();
        }

        [Fact]
        public void PutInBasket_WithInBasket_ShouldNotChangeItem()
        {
            // Arrange
            _fixture.SetupInBasket();
            _fixture.SetupExpectedResult();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.PutInBasket();

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
            sut.IsInBasket.Should().BeTrue();
            ReferenceEquals(result, sut).Should().BeFalse();
        }

        private sealed class PutInBasketFixture : ShoppingListItemFixture
        {
            public ShoppingListItem? ExpectedResult { get; private set; }

            public void SetupNotInBasket()
            {
                Builder.WithIsInBasket(false);
            }

            public void SetupInBasket()
            {
                Builder.WithIsInBasket(true);
            }

            public void SetupExpectedResult()
            {
                ExpectedResult = new ShoppingListItemBuilder()
                    .WithIsInBasket(true)
                    .Create();

                Builder
                    .WithId(ExpectedResult.Id)
                    .WithTypeId(ExpectedResult.TypeId)
                    .WithQuantity(ExpectedResult.Quantity);
            }
        }
    }

    public class RemoveFromBasket
    {
        private readonly RemoveFromBasketFixture _fixture = new();

        [Fact]
        public void RemoveFromBasket_WithInBasket_ShouldRemoveItemFromBasket()
        {
            // Arrange
            _fixture.SetupInBasket();
            _fixture.SetupExpectedResult();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.RemoveFromBasket();

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
            sut.IsInBasket.Should().BeTrue();
            ReferenceEquals(result, sut).Should().BeFalse();
        }

        [Fact]
        public void RemoveFromBasket_WithNotInBasket_ShouldNotChangeItem()
        {
            // Arrange
            _fixture.SetupNotInBasket();
            _fixture.SetupExpectedResult();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.RemoveFromBasket();

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
            sut.IsInBasket.Should().BeFalse();
            ReferenceEquals(result, sut).Should().BeFalse();
        }

        private sealed class RemoveFromBasketFixture : ShoppingListItemFixture
        {
            public ShoppingListItem? ExpectedResult { get; private set; }

            public void SetupNotInBasket()
            {
                Builder.WithIsInBasket(false);
            }

            public void SetupInBasket()
            {
                Builder.WithIsInBasket(true);
            }

            public void SetupExpectedResult()
            {
                ExpectedResult = new ShoppingListItemBuilder()
                    .WithIsInBasket(false)
                    .Create();

                Builder
                    .WithId(ExpectedResult.Id)
                    .WithTypeId(ExpectedResult.TypeId)
                    .WithQuantity(ExpectedResult.Quantity);
            }
        }
    }

    public class ChangeQuantity
    {
        private readonly ChangeQuantityFixture _fixture = new();

        [Fact]
        public void ChangeQuantity_WithValidData_ShouldSetNewQuantity()
        {
            // Arrange
            _fixture.SetupQuantity();
            _fixture.SetupExpectedResult();
            var sut = _fixture.CreateSut();
            var originalQuantity = sut.Quantity;

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Quantity);

            // Act
            var result = sut.ChangeQuantity(_fixture.Quantity.Value);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
            sut.Quantity.Should().Be(originalQuantity);
            ReferenceEquals(result, sut).Should().BeFalse();
        }

        private sealed class ChangeQuantityFixture : ShoppingListItemFixture
        {
            public QuantityInBasket? Quantity { get; private set; }
            public ShoppingListItem? ExpectedResult { get; private set; }

            public void SetupQuantity()
            {
                Quantity = new DomainTestBuilder<QuantityInBasket>().Create();
            }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(Quantity);

                ExpectedResult = new ShoppingListItemBuilder()
                    .WithQuantity(Quantity.Value)
                    .Create();

                Builder
                    .WithId(ExpectedResult.Id)
                    .WithIsInBasket(ExpectedResult.IsInBasket)
                    .WithTypeId(ExpectedResult.TypeId);
            }
        }
    }

    public class AddQuantity
    {
        private readonly AddQuantityFixture _fixture = new();

        [Fact]
        public void AddQuantity_WithValidData_ShouldAddQuantity()
        {
            // Arrange
            _fixture.SetupQuantity();
            _fixture.SetupExpectedResult();
            var sut = _fixture.CreateSut();
            var originalQuantity = sut.Quantity;

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Quantity);

            // Act
            var result = sut.AddQuantity(_fixture.Quantity.Value);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
            sut.Quantity.Should().Be(originalQuantity);
            ReferenceEquals(result, sut).Should().BeFalse();
        }

        private sealed class AddQuantityFixture : ShoppingListItemFixture
        {
            public QuantityInBasket? Quantity { get; private set; }
            public ShoppingListItem? ExpectedResult { get; private set; }

            public void SetupQuantity()
            {
                Quantity = new DomainTestBuilder<QuantityInBasket>().Create();
            }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(Quantity);

                var originalQuantity = new DomainTestBuilder<QuantityInBasket>().Create();

                ExpectedResult = new ShoppingListItemBuilder()
                    .WithQuantity(Quantity.Value + originalQuantity)
                    .Create();

                Builder
                    .WithId(ExpectedResult.Id)
                    .WithQuantity(originalQuantity)
                    .WithIsInBasket(ExpectedResult.IsInBasket)
                    .WithTypeId(ExpectedResult.TypeId);
            }
        }
    }

    private abstract class ShoppingListItemFixture
    {
        protected readonly ShoppingListItemBuilder Builder = new();

        public ShoppingListItem CreateSut()
        {
            return Builder.Create();
        }
    }
}