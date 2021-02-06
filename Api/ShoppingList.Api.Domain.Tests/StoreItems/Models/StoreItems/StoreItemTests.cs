using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Models.StoreItems
{
    public class StoreItemTests
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemFixture storeItemFixture;

        public StoreItemTests()
        {
            commonFixture = new CommonFixture();
            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(storeItemAvailabilityFixture, commonFixture);
        }

        #region Delete

        [Fact]
        public void Delete_WithNotDeltedStoreItem_ShouldMarkStoreItemAsDeleted()
        {
            // Arrange
            var storeItem = storeItemFixture.GetStoreItem(new StoreItemId(commonFixture.NextInt()), isDeleted: false);

            // Act
            storeItem.Delete();

            // Assert
            using (new AssertionScope())
            {
                storeItem.IsDeleted.Should().BeTrue();
            }
        }

        #endregion Delete

        #region IsAvailableInStore

        [Fact]
        public void IsAvailableInStore_WithNotAvailableInStore_ShouldReturnTrue()
        {
            // Arrange
            var availabilities = storeItemAvailabilityFixture.GetAvailabilities(4).ToList();
            IStoreItem storeItem = storeItemFixture.GetStoreItem(
                availabilityCount: 0,
                additionalAvailabilities: availabilities);
            var availabilityStoreIds = availabilities.Select(av => av.StoreId.Value).ToList();

            // Act
            StoreId storeId = new StoreId(commonFixture.NextInt(availabilityStoreIds));
            bool result = storeItem.IsAvailableInStore(storeId);

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
            var availabilities = storeItemAvailabilityFixture.GetAvailabilities(4).ToList();
            IStoreItem storeItem = storeItemFixture.GetStoreItem(
                availabilityCount: 0,
                additionalAvailabilities: availabilities);
            var availabilityStoreIds = availabilities.Select(av => av.StoreId).ToList();

            // Act
            int storeIdIndex = commonFixture.NextInt(0, availabilityStoreIds.Count - 1);
            bool result = storeItem.IsAvailableInStore(availabilityStoreIds[storeIdIndex]);

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
            Fixture fixture = commonFixture.GetNewFixture();
            IStoreItem storeItem = storeItemFixture.GetStoreItem(isTemporary: true);
            PermanentItem permanentItem = fixture.Create<PermanentItem>();
            IManufacturer manufacturer = fixture.Create<IManufacturer>();
            IItemCategory itemCategory = fixture.Create<IItemCategory>();
            IEnumerable<IStoreItemAvailability> availabilities = storeItemAvailabilityFixture.GetAvailabilities(4);

            // Act
            storeItem.MakePermanent(permanentItem, itemCategory, manufacturer, availabilities);

            // Assert
            using (new AssertionScope())
            {
                storeItem.Name.Should().Be(permanentItem.Name);
                storeItem.Comment.Should().Be(permanentItem.Comment);
                storeItem.QuantityType.Should().Be(permanentItem.QuantityType);
                storeItem.QuantityInPacket.Should().Be(permanentItem.QuantityInPacket);
                storeItem.QuantityTypeInPacket.Should().Be(permanentItem.QuantityTypeInPacket);
                storeItem.Availabilities.Should().BeEquivalentTo(availabilities);
                storeItem.ItemCategory.Should().Be(itemCategory);
                storeItem.Manufacturer.Should().Be(manufacturer);
                storeItem.IsTemporary.Should().BeFalse();
            }
        }

        #endregion MakePermanent

        #region Modify

        [Fact]
        public void Modify_WithValidData_ShouldMakeItemPermanent()
        {
            // Arrange
            Fixture fixture = commonFixture.GetNewFixture();
            bool isTemporary = commonFixture.NextBool();
            IStoreItem storeItem = storeItemFixture.GetStoreItem(isTemporary: isTemporary);
            ItemModify itemModify = fixture.Create<ItemModify>();
            IManufacturer manufacturer = fixture.Create<IManufacturer>();
            IItemCategory itemCategory = fixture.Create<IItemCategory>();
            IEnumerable<IStoreItemAvailability> availabilities = storeItemAvailabilityFixture.GetAvailabilities(4);

            // Act
            storeItem.Modify(itemModify, itemCategory, manufacturer, availabilities);

            // Assert
            using (new AssertionScope())
            {
                storeItem.Name.Should().Be(itemModify.Name);
                storeItem.Comment.Should().Be(itemModify.Comment);
                storeItem.QuantityType.Should().Be(itemModify.QuantityType);
                storeItem.QuantityInPacket.Should().Be(itemModify.QuantityInPacket);
                storeItem.QuantityTypeInPacket.Should().Be(itemModify.QuantityTypeInPacket);
                storeItem.Availabilities.Should().BeEquivalentTo(availabilities);
                storeItem.ItemCategory.Should().Be(itemCategory);
                storeItem.Manufacturer.Should().Be(manufacturer);
                storeItem.IsTemporary.Should().Be(isTemporary);
            }
        }

        #endregion Modify

        #region SetPredecessor

        [Fact]
        public void SetPredecessor_WithValidPredecessor_ShouldSetPredecessor()
        {
            // Arrange
            IStoreItem storeItem = storeItemFixture.GetStoreItem();
            IStoreItem predecessor = storeItemFixture.GetStoreItem();

            // Act
            storeItem.SetPredecessor(predecessor);

            // Assert
            using (new AssertionScope())
            {
                storeItem.Predecessor.Should().BeEquivalentTo(predecessor);
            }
        }

        #endregion SetPredecessor
    }
}