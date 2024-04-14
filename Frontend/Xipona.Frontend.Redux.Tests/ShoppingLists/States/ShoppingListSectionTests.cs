using FluentAssertions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.ShoppingLists.States;

public class ShoppingListSectionTests
{
    public class GetDisplayedItems
    {
        private readonly GetDisplayedItemsFixture _fixture = new();

        [Fact]
        public void GetDisplayedItems_WithItemsInBasketVisible_ShouldReturnAllItems()
        {
            // Arrange
            _fixture.SetupHiddenAndNotHiddenItem();
            _fixture.SetupItemsInBasketVisible();
            _fixture.SetupExpectedResultForItemsInBasketVisible();

            // Act
            var result = _fixture.Sut.GetDisplayedItems(_fixture.ItemsInBasketVisible);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public void GetDisplayedItems_WithItemsInBasketNotVisible_ShouldReturnOnlyNotHiddenItems()
        {
            // Arrange
            _fixture.SetupHiddenAndNotHiddenItem();
            _fixture.SetupItemsInBasketNotVisible();
            _fixture.SetupExpectedResultForItemsInBasketNotVisible();

            // Act
            var result = _fixture.Sut.GetDisplayedItems(_fixture.ItemsInBasketVisible);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.WithStrictOrdering());
        }

        private sealed class GetDisplayedItemsFixture : ShoppingListSectionFixture
        {
            public bool ItemsInBasketVisible { get; private set; }
            public IReadOnlyCollection<ShoppingListItem>? ExpectedResult { get; private set; }

            public void SetupHiddenAndNotHiddenItem()
            {
                Sut = Sut with
                {
                    Items = new List<ShoppingListItem>()
                    {
                        new DomainTestBuilder<ShoppingListItem>().Create() with
                        {
                            Name = "C",
                            Hidden = true
                        },
                        new DomainTestBuilder<ShoppingListItem>().Create() with
                        {
                            Name = "M",
                            Hidden = true
                        },
                        new DomainTestBuilder<ShoppingListItem>().Create() with
                        {
                            Name = "H",
                            Hidden = false
                        },
                        new DomainTestBuilder<ShoppingListItem>().Create() with
                        {
                            Name = "A",
                            Hidden = false
                        }
                    }
                };
            }

            public void SetupItemsInBasketVisible()
            {
                ItemsInBasketVisible = true;
            }

            public void SetupItemsInBasketNotVisible()
            {
                ItemsInBasketVisible = false;
            }

            public void SetupExpectedResultForItemsInBasketNotVisible()
            {
                ExpectedResult = new List<ShoppingListItem>
                {
                    Sut.Items.ElementAt(3),
                    Sut.Items.ElementAt(2)
                };
            }

            public void SetupExpectedResultForItemsInBasketVisible()
            {
                ExpectedResult = new List<ShoppingListItem>
                {
                    Sut.Items.ElementAt(3),
                    Sut.Items.ElementAt(0),
                    Sut.Items.ElementAt(2),
                    Sut.Items.ElementAt(1)
                };
            }
        }
    }

    private abstract class ShoppingListSectionFixture
    {
        public ShoppingListSection Sut { get; protected set; } = new DomainTestBuilder<ShoppingListSection>().Create();
    }
}