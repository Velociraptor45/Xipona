using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using System;
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
            var storeItem = storeItemFixture.CreateValid();

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
        public void IsAvailableInStore_WithNotAvailableInStore_ShouldReturnFalse()
        {
            // Arrange
            IStoreItem storeItem = storeItemFixture.CreateValid();
            var availabilityStoreIds = storeItem.Availabilities.Select(av => av.StoreId.Value).ToList();

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
            IStoreItem storeItem = storeItemFixture.CreateValid();
            var availabilityStoreIds = storeItem.Availabilities.Select(av => av.StoreId).ToList();

            // Act
            StoreId chosenStoreId = commonFixture.ChooseRandom(availabilityStoreIds);
            bool result = storeItem.IsAvailableInStore(chosenStoreId);

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

            var definition = StoreItemDefinition.FromTemporary(true);
            IStoreItem storeItem = storeItemFixture.CreateValid(definition);
            PermanentItem permanentItem = fixture.Create<PermanentItem>();
            List<IStoreItemAvailability> availabilities = storeItemAvailabilityFixture.CreateManyValid().ToList();

            // Act
            storeItem.MakePermanent(permanentItem, availabilities);

            // Assert
            using (new AssertionScope())
            {
                storeItem.Name.Should().Be(permanentItem.Name);
                storeItem.Comment.Should().Be(permanentItem.Comment);
                storeItem.QuantityType.Should().Be(permanentItem.QuantityType);
                storeItem.QuantityInPacket.Should().Be(permanentItem.QuantityInPacket);
                storeItem.QuantityTypeInPacket.Should().Be(permanentItem.QuantityTypeInPacket);
                storeItem.Availabilities.Should().BeEquivalentTo(availabilities);
                storeItem.ItemCategoryId.Should().Be(permanentItem.ItemCategoryId);
                storeItem.ManufacturerId.Should().Be(permanentItem.ManufacturerId);
                storeItem.IsTemporary.Should().BeFalse();
            }
        }

        #endregion MakePermanent

        #region Modify

        [Fact]
        public void Modify_WithValidData_ShouldModifyItem()
        {
            // Arrange
            Fixture fixture = commonFixture.GetNewFixture();

            var isTemporary = commonFixture.NextBool();
            var definition = StoreItemDefinition.FromTemporary(isTemporary);
            IStoreItem storeItem = storeItemFixture.CreateValid(definition);
            ItemModify itemModify = fixture.Create<ItemModify>();
            IEnumerable<IStoreItemAvailability> availabilities = storeItemAvailabilityFixture.CreateManyValid().ToList();

            // Act
            storeItem.Modify(itemModify, availabilities);

            // Assert
            using (new AssertionScope())
            {
                storeItem.Name.Should().Be(itemModify.Name);
                storeItem.Comment.Should().Be(itemModify.Comment);
                storeItem.QuantityType.Should().Be(itemModify.QuantityType);
                storeItem.QuantityInPacket.Should().Be(itemModify.QuantityInPacket);
                storeItem.QuantityTypeInPacket.Should().Be(itemModify.QuantityTypeInPacket);
                storeItem.Availabilities.Should().BeEquivalentTo(availabilities);
                storeItem.ItemCategoryId.Should().Be(itemModify.ItemCategoryId);
                storeItem.ManufacturerId.Should().Be(itemModify.ManufacturerId);
                storeItem.IsTemporary.Should().Be(isTemporary);
            }
        }

        #endregion Modify

        #region GetDefaultSectionIdForStore

        [Fact]
        public void GetDefaultSectionIdForStore_WithInvalidStoreId_ShouldThrowDomainException()
        {
            // Arrange
            IStoreItem storeItem = storeItemFixture.CreateValid();
            var allStoreIds = storeItem.Availabilities.Select(av => av.StoreId.Value);
            var requestStoreId = new StoreId(commonFixture.NextInt(allStoreIds));

            // Act
            Action action = () => storeItem.GetDefaultSectionIdForStore(requestStoreId);

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
            IStoreItem storeItem = storeItemFixture.CreateValid();
            var chosenAvailability = commonFixture.ChooseRandom(storeItem.Availabilities);

            // Act
            var result = storeItem.GetDefaultSectionIdForStore(chosenAvailability.StoreId);

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
            IStoreItem storeItem = storeItemFixture.CreateValid();
            IStoreItem predecessor = storeItemFixture.CreateValid();

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