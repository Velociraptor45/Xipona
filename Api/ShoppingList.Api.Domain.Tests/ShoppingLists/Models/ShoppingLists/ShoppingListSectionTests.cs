using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Models.ShoppingLists;

public class ShoppingListSectionTests
{
    public ShoppingListSectionTests()
    {
    }

    #region RemoveItem
    // todo implement
    #endregion RemoveItem

    #region ContainsItem
    // todo implement
    #endregion ContainsItem

    public class AddItem
    {
        private readonly AddItemFixture _fixture = new();

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddItem_WithItemNotAlreadyExisting_ReturnsExpectedResult(bool throwIfAlreadyPresent)
        {
            // Arrange
            _fixture.SetupItemNotAlreadyExisting();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultForItemNotAlreadyExisting(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.AddItem(_fixture.Item, throwIfAlreadyPresent);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void AddItem_WithItemAlreadyExisting_WithNotThrowIfAlreadyPresent_ReturnsExpectedResult()
        {
            // Arrange
            _fixture.SetupItemAlreadyExisting();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultForItemAlreadyExisting(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.AddItem(_fixture.Item, false);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void AddItem_WithItemAlreadyExisting_WithThrowIfAlreadyPresent_ThrowsException()
        {
            // Arrange
            _fixture.SetupItemAlreadyExisting();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            Action act = () => sut.AddItem(_fixture.Item);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemAlreadyInSection);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddItem_WithItemWithTypeNotAlreadyExisting_ReturnsExpectedResult(bool throwIfAlreadyPresent)
        {
            // Arrange
            _fixture.SetupItemWithTypeNotAlreadyExisting();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultForItemWithTypeNotAlreadyExisting(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.AddItem(_fixture.Item, throwIfAlreadyPresent);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void AddItem_WithItemWithTypeAlreadyExisting_WithNotThrowIfAlreadyPresent_ReturnsExpectedResult()
        {
            // Arrange
            _fixture.SetupItemWithTypeAlreadyExisting();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultForItemWithTypeAlreadyExisting(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.AddItem(_fixture.Item, false);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void AddItem_WithItemWithTypeAlreadyExisting_WithThrowIfAlreadyPresent_ThrowsException()
        {
            // Arrange
            _fixture.SetupItemWithTypeAlreadyExisting();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            Action act = () => sut.AddItem(_fixture.Item);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemAlreadyInSection);
        }

        private sealed class AddItemFixture : ShoppingListSectionFixture
        {
            public IShoppingListItem? Item { get; private set; }
            public ShoppingListSection? ExpectedResult { get; private set; }

            public void SetupItemNotAlreadyExisting()
            {
                Item = ShoppingListItemMother.NotInBasket().WithoutTypeId().Create();
            }

            public void SetupItemAlreadyExisting()
            {
                Item = ShoppingListItemMother.NotInBasket().WithoutTypeId().Create();
                var existingItem = ShoppingListItemMother.NotInBasket().WithId(Item.Id).WithoutTypeId().Create();
                SetupSectionContainingItem(existingItem);
            }

            public void SetupItemWithTypeNotAlreadyExisting()
            {
                Item = ShoppingListItemMother.NotInBasket().Create();
            }

            public void SetupItemWithTypeAlreadyExisting()
            {
                Item = ShoppingListItemMother.NotInBasket().Create();
                var existingItem = ShoppingListItemMother.NotInBasket().WithId(Item.Id).WithTypeId(Item.TypeId).Create();
                SetupSectionContainingItem(existingItem);
            }

            public void SetupExpectedResultForItemNotAlreadyExisting(ShoppingListSection sut)
            {
                TestPropertyNotSetException.ThrowIfNull(Item);

                ExpectedResult = new ShoppingListSection(sut.Id, sut.Items.Append(Item));
            }

            public void SetupExpectedResultForItemAlreadyExisting(ShoppingListSection sut)
            {
                TestPropertyNotSetException.ThrowIfNull(Item);

                var withoutExistingItem = sut.Items.Where(i => i.Id != Item!.Id).ToList();
                var existingItem = sut.Items.First(i => i.Id == Item!.Id);
                withoutExistingItem.Add(new ShoppingListItem(Item.Id, null, existingItem.IsInBasket,
                    existingItem.Quantity + Item.Quantity));

                ExpectedResult = new ShoppingListSection(sut.Id, withoutExistingItem);
            }

            public void SetupExpectedResultForItemWithTypeNotAlreadyExisting(ShoppingListSection sut)
            {
                TestPropertyNotSetException.ThrowIfNull(Item);

                ExpectedResult = new ShoppingListSection(sut.Id, sut.Items.Append(Item));
            }

            public void SetupExpectedResultForItemWithTypeAlreadyExisting(ShoppingListSection sut)
            {
                TestPropertyNotSetException.ThrowIfNull(Item);

                var withoutExistingItem = sut.Items.Where(i => i.Id != Item!.Id && i.TypeId != Item.TypeId).ToList();
                var existingItem = sut.Items.First(i => i.Id == Item!.Id && i.TypeId == Item.TypeId);
                withoutExistingItem.Add(new ShoppingListItem(Item.Id, Item.TypeId, existingItem.IsInBasket,
                    existingItem.Quantity + Item.Quantity));

                ExpectedResult = new ShoppingListSection(sut.Id, withoutExistingItem);
            }
        }
    }

    #region PutItemInBasket
    // todo implement
    #endregion PutItemInBasket

    #region RemoveItemFromBasket
    // todo implement
    #endregion RemoveItemFromBasket

    #region ChangeItemQuantity
    // todo implement
    #endregion ChangeItemQuantity

    #region RemoveAllItemsInBasket
    // todo implement
    #endregion RemoveAllItemsInBasket

    #region RemoveAllItemsNotInBasket
    // todo implement
    #endregion RemoveAllItemsNotInBasket

    private abstract class ShoppingListSectionFixture
    {
        private readonly ShoppingListSectionBuilder _builder = new();

        protected void SetupSectionContainingItem(IShoppingListItem item)
        {
            _builder.WithItem(item);
        }

        public ShoppingListSection CreateSut()
        {
            return _builder.Create();
        }
    }
}