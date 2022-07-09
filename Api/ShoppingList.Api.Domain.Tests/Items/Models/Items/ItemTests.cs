using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.TemporaryItems;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Items.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Models.Items;

public class ItemTests
{
    private readonly CommonFixture _commonFixture;

    public ItemTests()
    {
        _commonFixture = new CommonFixture();
    }

    #region Delete

    [Fact]
    public void Delete_WithNotDeletedItem_ShouldMarkItemAsDeleted()
    {
        // Arrange
        var item = ItemMother.Initial().Create();

        // Act
        item.Delete();

        // Assert
        using (new AssertionScope())
        {
            item.IsDeleted.Should().BeTrue();
        }
    }

    #endregion Delete

    #region IsAvailableInStore

    [Fact]
    public void IsAvailableInStore_WithNotAvailableInStore_ShouldReturnFalse()
    {
        // Arrange
        IItem testObject = ItemMother.Initial().Create();

        // Act
        StoreId storeId = new StoreIdBuilder().Create();
        bool result = testObject.IsAvailableInStore(storeId);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
        }
    }

    [Fact]
    public void IsAvailableInStore_WithAvailableInStore_ShouldReturnTrue()
    {
        // Arrange
        IItem testObject = ItemMother.Initial().Create();
        var availabilityStoreIds = testObject.Availabilities.Select(av => av.StoreId).ToList();

        // Act
        StoreId chosenStoreId = _commonFixture.ChooseRandom(availabilityStoreIds);
        bool result = testObject.IsAvailableInStore(chosenStoreId);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
        }
    }

    #endregion IsAvailableInStore

    #region MakePermanent

    [Fact]
    public void MakePermanent_WithValidData_ShouldMakeItemPermanent()
    {
        // Arrange
        Fixture fixture = _commonFixture.GetNewFixture();

        IItem testObject = ItemMother.Initial().Create();
        PermanentItem permanentItem = fixture.Create<PermanentItem>();
        IEnumerable<IItemAvailability> availabilities =
            ItemAvailabilityMother.Initial().CreateMany(3).ToList();

        // Act
        testObject.MakePermanent(permanentItem, availabilities);

        // Assert
        using (new AssertionScope())
        {
            testObject.Name.Should().Be(permanentItem.Name);
            testObject.Comment.Should().Be(permanentItem.Comment);
            testObject.ItemQuantity.Should().Be(permanentItem.ItemQuantity);
            testObject.Availabilities.Should().BeEquivalentTo(availabilities);
            testObject.ItemCategoryId.Should().Be(permanentItem.ItemCategoryId);
            testObject.ManufacturerId.Should().Be(permanentItem.ManufacturerId);
            testObject.IsTemporary.Should().BeFalse();
        }
    }

    #endregion MakePermanent

    #region Modify

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Modify_WithValidData_ShouldModifyItem(bool isTemporary)
    {
        // Arrange
        Fixture fixture = _commonFixture.GetNewFixture();

        IItem testObject = ItemMother.Initial().WithIsTemporary(isTemporary).Create();
        ItemModification itemModify = fixture.Create<ItemModification>();
        IEnumerable<IItemAvailability> availabilities =
            ItemAvailabilityMother.Initial().CreateMany(3).ToList();

        // Act
        testObject.Modify(itemModify, availabilities);

        // Assert
        using (new AssertionScope())
        {
            testObject.Name.Should().Be(itemModify.Name);
            testObject.Comment.Should().Be(itemModify.Comment);
            testObject.ItemQuantity.Should().Be(itemModify.ItemQuantity);
            testObject.Availabilities.Should().BeEquivalentTo(availabilities);
            testObject.ItemCategoryId.Should().Be(itemModify.ItemCategoryId);
            testObject.ManufacturerId.Should().Be(itemModify.ManufacturerId);
            testObject.IsTemporary.Should().Be(isTemporary);
        }
    }

    #endregion Modify

    #region GetDefaultSectionIdForStore

    [Fact]
    public void GetDefaultSectionIdForStore_WithInvalidStoreId_ShouldThrowDomainException()
    {
        // Arrange
        IItem testObject = ItemMother.Initial().Create();
        var requestStoreId = new StoreIdBuilder().Create();

        // Act
        Action action = () => testObject.GetDefaultSectionIdForStore(requestStoreId);

        // Assert
        using (new AssertionScope())
        {
            action.Should().Throw<DomainException>()
                .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemAtStoreNotAvailable);
        }
    }

    [Fact]
    public void GetDefaultSectionIdForStore_WithValidStoreId_ShouldReturnSectionId()
    {
        // Arrange
        IItem testObject = ItemMother.Initial().Create();
        var chosenAvailability = _commonFixture.ChooseRandom(testObject.Availabilities);

        // Act
        var result = testObject.GetDefaultSectionIdForStore(chosenAvailability.StoreId);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(chosenAvailability.DefaultSectionId);
        }
    }

    #endregion GetDefaultSectionIdForStore

    #region SetPredecessor

    [Fact]
    public void SetPredecessor_WithValidPredecessor_ShouldSetPredecessor()
    {
        // Arrange
        IItem testObject = ItemMother.Initial().Create();
        IItem predecessor = ItemMother.Initial().Create();

        // Act
        testObject.SetPredecessor(predecessor);

        // Assert
        using (new AssertionScope())
        {
            testObject.Predecessor.Should().BeEquivalentTo(predecessor);
        }
    }

    #endregion SetPredecessor
}