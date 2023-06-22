using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States.Comparer;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.ShoppingLists.States;

public class ShoppingListStateTests
{
    public class AllItemsInBasketHidden
    {
        private readonly AllItemsInBasketHiddenFixture _fixture = new();

        [Fact]
        public void AllItemsInBasketHidden_WithItemsInBasketVisible_WithAllItemsInBasket_ShouldReturnFalse()
        {
            // Arrange
            _fixture.SetupAllItemsInBasket();
            _fixture.SetupItemsInBasketVisible();

            // Act
            var result = _fixture.Sut.AllItemsInBasketHidden;

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void AllItemsInBasketHidden_WithItemsInBasketVisible_WithNotAllItemsInBasket_ShouldReturnFalse()
        {
            // Arrange
            _fixture.SetupNotAllItemsInBasket();
            _fixture.SetupItemsInBasketVisible();

            // Act
            var result = _fixture.Sut.AllItemsInBasketHidden;

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void AllItemsInBasketHidden_WithItemsInBasketInvisible_WithNotAllItemsInBasket_ShouldReturnFalse()
        {
            // Arrange
            _fixture.SetupNotAllItemsInBasket();
            _fixture.SetupItemsInBasketInvisible();

            // Act
            var result = _fixture.Sut.AllItemsInBasketHidden;

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void AllItemsInBasketHidden_WithItemsInBasketInvisible_WithAllItemsInBasket_ShouldReturnTrue()
        {
            // Arrange
            _fixture.SetupAllItemsInBasket();
            _fixture.SetupItemsInBasketInvisible();

            // Act
            var result = _fixture.Sut.AllItemsInBasketHidden;

            // Assert
            result.Should().BeTrue();
        }

        private sealed class AllItemsInBasketHiddenFixture : ShoppingListStateFixture
        {
            public void SetupItemsInBasketVisible()
            {
                SetupItemsInBasketVisible(true);
            }

            public void SetupItemsInBasketInvisible()
            {
                SetupItemsInBasketVisible(false);
            }

            public void SetupAllItemsInBasket()
            {
                SetupItem(true);
            }

            public void SetupNotAllItemsInBasket()
            {
                SetupItem(false);
            }

            private void SetupItem(bool inBasket)
            {
                var sections = new List<ShoppingListSection>
                {
                    new DomainTestBuilder<ShoppingListSection>().Create() with
                    {
                        Items = new List<ShoppingListItem>()
                        {
                            new DomainTestBuilder<ShoppingListItem>().Create() with { IsInBasket = inBasket }
                        }
                    }
                };

                Sut = Sut with
                {
                    ShoppingList = Sut.ShoppingList! with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
                    }
                };
            }
        }
    }

    public abstract class ShoppingListStateFixture
    {
        public ShoppingListState Sut { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();

        protected void SetupItemsInBasketVisible(bool value)
        {
            Sut = Sut with
            {
                ItemsInBasketVisible = value
            };
        }
    }
}