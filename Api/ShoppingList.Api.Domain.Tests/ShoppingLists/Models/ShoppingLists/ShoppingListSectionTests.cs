using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
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
        public void AddItem_WithItemNotAlreadyExisting_ShouldReturnExpectedResult(bool throwIfAlreadyPresent)
        {
            // Arrange
            _fixture.SetupItemNotAlreadyExisting();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultForNotAlreadyExisting(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.AddItem(_fixture.Item, throwIfAlreadyPresent);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void AddItem_WithItemAlreadyExisting_WithNotThrowIfAlreadyPresent_ShouldReturnExpectedResult()
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
        public void AddItem_WithItemAlreadyExisting_WithThrowIfAlreadyPresent_ShouldThrowException()
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
        public void AddItem_WithItemWithTypeNotAlreadyExisting_ShouldReturnExpectedResult(bool throwIfAlreadyPresent)
        {
            // Arrange
            _fixture.SetupItemWithTypeNotAlreadyExisting();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResultForNotAlreadyExisting(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.AddItem(_fixture.Item, throwIfAlreadyPresent);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void AddItem_WithItemWithTypeAlreadyExisting_WithNotThrowIfAlreadyPresent_ShouldReturnExpectedResult()
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
        public void AddItem_WithItemWithTypeAlreadyExisting_WithThrowIfAlreadyPresent_ShouldThrowException()
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
            public ShoppingListItem? Item { get; private set; }
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

            public void SetupExpectedResultForItemAlreadyExisting(ShoppingListSection sut)
            {
                TestPropertyNotSetException.ThrowIfNull(Item);

                var withoutExistingItem = sut.Items.Where(i => i.Id != Item!.Id).ToList();
                var existingItem = sut.Items.First(i => i.Id == Item!.Id);
                withoutExistingItem.Add(new ShoppingListItem(Item.Id, null, existingItem.IsInBasket,
                    existingItem.Quantity + Item.Quantity));

                ExpectedResult = new ShoppingListSection(sut.Id, withoutExistingItem);
            }

            public void SetupExpectedResultForNotAlreadyExisting(ShoppingListSection sut)
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

    public class PutItemInBasket
    {
        private readonly PutItemInBasketFixture _fixture = new();

        [Fact]
        public void PutItemInBasket_WithItemNotAlreadyInBasket_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemWithTypeNotAlreadyInBasket();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var result = sut.PutItemInBasket(_fixture.Item.Id, _fixture.Item.TypeId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void PutItemInBasket_WithItemAlreadyInBasket_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemWithTypeAlreadyInBasket();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var result = sut.PutItemInBasket(_fixture.Item.Id, _fixture.Item.TypeId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void PutItemInBasket_WithItemTypeIdNotProvided_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemWithTypeAlreadyInBasket();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var act = () => sut.PutItemInBasket(_fixture.Item.Id, null);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        [Fact]
        public void PutItemInBasket_WithWrongItemTypeIdProvided_ShouldThrowException()
        {
            // Arrange
            _fixture.SetupItemNotAlreadyInBasket();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            Action act = () => sut.PutItemInBasket(_fixture.Item.Id, ItemTypeId.New);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        [Fact]
        public void PutItemInBasket_WithWrongItemIdProvided_ShouldThrowException()
        {
            // Arrange
            _fixture.SetupItemNotAlreadyInBasket();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            Action act = () => sut.PutItemInBasket(ItemId.New, null);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        private sealed class PutItemInBasketFixture : ShoppingListSectionFixture
        {
            public ShoppingListSection? ExpectedResult { get; private set; }
            public ShoppingListItem? Item { get; private set; }

            public void SetupItemNotAlreadyInBasket()
            {
                Item = ShoppingListItemMother.NotInBasket().WithoutTypeId().Create();
                SetupSectionContainingItem(Item);
            }

            public void SetupItemWithTypeNotAlreadyInBasket()
            {
                Item = ShoppingListItemMother.NotInBasket().Create();
                SetupSectionContainingItem(Item);
            }

            public void SetupItemWithTypeAlreadyInBasket()
            {
                Item = ShoppingListItemMother.InBasket().Create();
                SetupSectionContainingItem(Item);
            }

            public void SetupExpectedResult(ShoppingListSection sut)
            {
                var item = sut.Items.First();
                var newItem = new ShoppingListItem(item.Id, item.TypeId, true, item.Quantity);

                ExpectedResult = new ShoppingListSection(sut.Id, newItem.ToMonoList());
            }
        }
    }

    public class RemoveItemFromBasket
    {
        private readonly RemoveItemFromBasketFixture _fixture = new();

        [Fact]
        public void RemoveItemFromBasket_WithItemNotAlreadyInBasket_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemWithTypeNotAlreadyInBasket();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var result = sut.RemoveItemFromBasket(_fixture.Item.Id, _fixture.Item.TypeId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void RemoveItemFromBasket_WithItemAlreadyInBasket_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemWithTypeAlreadyInBasket();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var result = sut.RemoveItemFromBasket(_fixture.Item.Id, _fixture.Item.TypeId);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void RemoveItemFromBasket_WithItemTypeIdNotProvided_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemWithTypeAlreadyInBasket();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var act = () => sut.RemoveItemFromBasket(_fixture.Item.Id, null);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        [Fact]
        public void RemoveItemFromBasket_WithWrongItemTypeIdProvided_ShouldThrowException()
        {
            // Arrange
            _fixture.SetupItemNotAlreadyInBasket();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            Action act = () => sut.RemoveItemFromBasket(_fixture.Item.Id, ItemTypeId.New);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        [Fact]
        public void RemoveItemFromBasket_WithWrongItemIdProvided_ShouldThrowException()
        {
            // Arrange
            _fixture.SetupItemNotAlreadyInBasket();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            Action act = () => sut.RemoveItemFromBasket(ItemId.New, null);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        private sealed class RemoveItemFromBasketFixture : ShoppingListSectionFixture
        {
            public ShoppingListSection? ExpectedResult { get; private set; }
            public ShoppingListItem? Item { get; private set; }

            public void SetupItemNotAlreadyInBasket()
            {
                Item = ShoppingListItemMother.NotInBasket().WithoutTypeId().Create();
                SetupSectionContainingItem(Item);
            }

            public void SetupItemWithTypeNotAlreadyInBasket()
            {
                Item = ShoppingListItemMother.NotInBasket().Create();
                SetupSectionContainingItem(Item);
            }

            public void SetupItemWithTypeAlreadyInBasket()
            {
                Item = ShoppingListItemMother.InBasket().Create();
                SetupSectionContainingItem(Item);
            }

            public void SetupExpectedResult(ShoppingListSection sut)
            {
                var item = sut.Items.First();
                var newItem = new ShoppingListItem(item.Id, item.TypeId, false, item.Quantity);

                ExpectedResult = new ShoppingListSection(sut.Id, newItem.ToMonoList());
            }
        }
    }

    public class ChangeItemQuantity
    {
        private readonly ChangeItemQuantityFixture _fixture = new();

        [Fact]
        public void ChangeItemQuantity_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemQuantity();
            _fixture.SetupItemWithType();
            var sut = _fixture.CreateSut();
            _fixture.SetupExpectedResult(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Quantity);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var result = sut.ChangeItemQuantity(_fixture.Item.Id, _fixture.Item.TypeId, _fixture.Quantity.Value);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public void ChangeItemQuantity_WithItemTypeIdNotProvided_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemQuantity();
            _fixture.SetupItemWithType();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Quantity);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            var act = () => sut.ChangeItemQuantity(_fixture.Item.Id, null, _fixture.Quantity.Value);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        [Fact]
        public void ChangeItemQuantity_WithWrongItemTypeIdProvided_ShouldThrowException()
        {
            // Arrange
            _fixture.SetupItemQuantity();
            _fixture.SetupItem();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Quantity);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            Action act = () => sut.ChangeItemQuantity(_fixture.Item.Id, ItemTypeId.New, _fixture.Quantity.Value);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        [Fact]
        public void ChangeItemQuantity_WithWrongItemIdProvided_ShouldThrowException()
        {
            // Arrange
            _fixture.SetupItemQuantity();
            _fixture.SetupItem();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Quantity);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);

            // Act
            Action act = () => sut.ChangeItemQuantity(ItemId.New, null, _fixture.Quantity.Value);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotInSection);
        }

        private sealed class ChangeItemQuantityFixture : ShoppingListSectionFixture
        {
            public ShoppingListSection? ExpectedResult { get; private set; }
            public ShoppingListItem? Item { get; private set; }
            public QuantityInBasket? Quantity { get; private set; }

            public void SetupItemQuantity()
            {
                Quantity = new DomainTestBuilder<QuantityInBasket>().Create();
            }

            public void SetupItem()
            {
                Item = new ShoppingListItemBuilder().WithoutTypeId().Create();
                SetupSectionContainingItem(Item);
            }

            public void SetupItemWithType()
            {
                Item = new ShoppingListItemBuilder().Create();
                SetupSectionContainingItem(Item);
            }

            public void SetupExpectedResult(ShoppingListSection sut)
            {
                TestPropertyNotSetException.ThrowIfNull(Quantity);

                var item = sut.Items.First();
                var newItem = new ShoppingListItem(item.Id, item.TypeId, item.IsInBasket, Quantity.Value);

                ExpectedResult = new ShoppingListSection(sut.Id, newItem.ToMonoList());
            }
        }
    }

    #region RemoveAllItemsInBasket
    // todo implement
    #endregion RemoveAllItemsInBasket

    #region RemoveAllItemsNotInBasket
    // todo implement
    #endregion RemoveAllItemsNotInBasket

    private abstract class ShoppingListSectionFixture
    {
        private readonly ShoppingListSectionBuilder _builder = new();

        protected void SetupSectionContainingItem(ShoppingListItem item)
        {
            _builder.WithItem(item);
        }

        public ShoppingListSection CreateSut()
        {
            return _builder.Create();
        }
    }
}