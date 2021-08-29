﻿using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Models.StoreItems
{
    public class StoreItemTests
    {
        private readonly CommonFixture commonFixture;

        public StoreItemTests()
        {
            commonFixture = new CommonFixture();
        }

        #region Delete

        [Fact]
        public void Delete_WithNotDeltedStoreItem_ShouldMarkStoreItemAsDeleted()
        {
            // Arrange
            var storeItem = StoreItemMother.Initial().Create();

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
            IStoreItem testObject = StoreItemMother.Initial().Create();
            var availabilityStoreIds = testObject.Availabilities.Select(av => av.StoreId.Value).ToList();

            // Act
            StoreId storeId = new StoreId(commonFixture.NextInt(availabilityStoreIds));
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
            IStoreItem testObject = StoreItemMother.Initial().Create();
            var availabilityStoreIds = testObject.Availabilities.Select(av => av.StoreId).ToList();

            // Act
            StoreId chosenStoreId = commonFixture.ChooseRandom(availabilityStoreIds);
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
            Fixture fixture = commonFixture.GetNewFixture();

            IStoreItem testObject = StoreItemMother.Initial().Create();
            PermanentItem permanentItem = fixture.Create<PermanentItem>();
            IEnumerable<IStoreItemAvailability> availabilities =
                StoreItemAvailabilityMother.Initial().CreateMany(3).ToList();

            // Act
            testObject.MakePermanent(permanentItem, availabilities);

            // Assert
            using (new AssertionScope())
            {
                testObject.Name.Should().Be(permanentItem.Name);
                testObject.Comment.Should().Be(permanentItem.Comment);
                testObject.QuantityType.Should().Be(permanentItem.QuantityType);
                testObject.QuantityInPacket.Should().Be(permanentItem.QuantityInPacket);
                testObject.QuantityTypeInPacket.Should().Be(permanentItem.QuantityTypeInPacket);
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
            Fixture fixture = commonFixture.GetNewFixture();

            IStoreItem testObject = StoreItemMother.Initial().WithIsTemporary(isTemporary).Create();
            ItemModify itemModify = fixture.Create<ItemModify>();
            IEnumerable<IStoreItemAvailability> availabilities =
                StoreItemAvailabilityMother.Initial().CreateMany(3).ToList();

            // Act
            testObject.Modify(itemModify, availabilities);

            // Assert
            using (new AssertionScope())
            {
                testObject.Name.Should().Be(itemModify.Name);
                testObject.Comment.Should().Be(itemModify.Comment);
                testObject.QuantityType.Should().Be(itemModify.QuantityType);
                testObject.QuantityInPacket.Should().Be(itemModify.QuantityInPacket);
                testObject.QuantityTypeInPacket.Should().Be(itemModify.QuantityTypeInPacket);
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
            IStoreItem testObject = StoreItemMother.Initial().Create();
            var allStoreIds = testObject.Availabilities.Select(av => av.StoreId.Value);
            var requestStoreId = new StoreId(commonFixture.NextInt(allStoreIds));

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
            IStoreItem testObject = StoreItemMother.Initial().Create();
            var chosenAvailability = commonFixture.ChooseRandom(testObject.Availabilities);

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
            IStoreItem testObject = StoreItemMother.Initial().Create();
            IStoreItem predecessor = StoreItemMother.Initial().Create();

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
}