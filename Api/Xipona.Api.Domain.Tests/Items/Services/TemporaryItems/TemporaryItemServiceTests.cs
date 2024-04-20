﻿using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.TemporaryItems;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Items.Services.TemporaryItems;

public class TemporaryItemServiceTests
{
    public class MakePermanentAsyncTests
    {
        private readonly MakePermanentAsyncFixture _fixture = new();

        [Fact]
        public async Task MakePermanentAsync_WithInvalidItemId_ShouldThrowDomainException()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupPermanentItem();
            _fixture.SetupNotFindingItem();

            TestPropertyNotSetException.ThrowIfNull(_fixture.PermanentItem);

            // Act
            Func<Task> func = async () => await sut.MakePermanentAsync(_fixture.PermanentItem);

            // Assert
            using (new AssertionScope())
            {
                (await func.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemNotFound);
            }
        }

        [Fact]
        public async Task MakePermanentAsync_WithNonTemporaryItem_ShouldThrowDomainException()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupPermanentItem();
            _fixture.SetupItemMock();
            _fixture.SetupFindingItem();

            TestPropertyNotSetException.ThrowIfNull(_fixture.PermanentItem);

            // Act
            Func<Task> func = async () => await sut.MakePermanentAsync(_fixture.PermanentItem);

            // Assert
            using (new AssertionScope())
            {
                (await func.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemNotTemporary);
            }
        }

        [Fact]
        public async Task MakePermanentAsync_WithValidDataAndManufacturerId_ShouldMakeTemporaryItemPermanent()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupTemporaryItemMock();
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemMock);

            List<ItemAvailability> availabilities = _fixture.ItemMock.Object.Availabilities.ToList();
            _fixture.SetupPermanentItem(availabilities);
            _fixture.SetupValidatingAvailabilities();
            _fixture.SetupValidatingItemCategory();
            _fixture.SetupValidatingManufacturer();
            _fixture.SetupMakingPermanent();
            _fixture.SetupStoringItem();
            _fixture.SetupFindingItem();

            TestPropertyNotSetException.ThrowIfNull(_fixture.PermanentItem);

            // Act
            await sut.MakePermanentAsync(_fixture.PermanentItem);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyValidatingItemCategory();
                _fixture.VerifyValidatingManufacturer();
                _fixture.VerifyValidatingAvailabilities();
                _fixture.ItemMock.VerifyMakePermanentOnce(_fixture.PermanentItem, availabilities);
                _fixture.VerifyStoringItem();
            }
        }

        [Fact]
        public async Task MakePermanentAsync_WithValidDataAndManufacturerIdIsNull_ShouldMakeTemporaryItemPermanent()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupTemporaryItemMock();
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemMock);

            List<ItemAvailability> availabilities = _fixture.ItemMock.Object.Availabilities.ToList();
            _fixture.SetupCommandWithoutManufacturerId(availabilities);
            _fixture.SetupValidatingAvailabilities();
            _fixture.SetupValidatingItemCategory();
            _fixture.SetupMakingPermanent();
            _fixture.SetupStoringItem();
            _fixture.SetupFindingItem();

            TestPropertyNotSetException.ThrowIfNull(_fixture.PermanentItem);

            // Act
            await sut.MakePermanentAsync(_fixture.PermanentItem);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyValidatingItemCategory();
                _fixture.VerifyNotValidatingManufacturer();
                _fixture.VerifyValidatingAvailabilities();
                _fixture.ItemMock.VerifyMakePermanentOnce(_fixture.PermanentItem, availabilities);
                _fixture.VerifyStoringItem();
            }
        }

        private sealed class MakePermanentAsyncFixture : LocalFixture
        {
            public PermanentItem? PermanentItem { get; private set; }
            public ItemMock? ItemMock { get; private set; }

            public void SetupPermanentItem()
            {
                PermanentItem = new DomainTestBuilder<PermanentItem>().Create<PermanentItem>();
            }

            public void SetupPermanentItem(IEnumerable<ItemAvailability> availabilities)
            {
                PermanentItem = new DomainTestBuilder<PermanentItem>()
                    .FillConstructorWith(nameof(availabilities), availabilities)
                    .Create();
            }

            public void SetupCommandWithoutManufacturerId(IEnumerable<ItemAvailability> availabilities)
            {
                PermanentItem = new DomainTestBuilder<PermanentItem>()
                    .FillConstructorWith("manufacturerId", (ManufacturerId?)null)
                    .FillConstructorWith(nameof(availabilities), availabilities)
                    .Create();
            }

            public void SetupItemMock()
            {
                ItemMock = new ItemMock(ItemMother.Initial().Create(), MockBehavior.Strict);
            }

            public void SetupTemporaryItemMock()
            {
                ItemMock = new ItemMock(ItemMother.InitialTemporary().Create(), MockBehavior.Strict);
            }

            public void SetupValidatingItemCategory()
            {
                TestPropertyNotSetException.ThrowIfNull(PermanentItem);
                ValidatorMock.SetupValidateAsync(PermanentItem.ItemCategoryId);
            }

            public void SetupValidatingManufacturer()
            {
                TestPropertyNotSetException.ThrowIfNull(PermanentItem);
                TestPropertyNotSetException.ThrowIfNull(PermanentItem.ManufacturerId);
                ValidatorMock.SetupValidateAsync(PermanentItem.ManufacturerId.Value);
            }

            public void SetupValidatingAvailabilities()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemMock);
                ValidatorMock.SetupValidateAsync(ItemMock.Object.Availabilities);
            }

            public void SetupMakingPermanent()
            {
                TestPropertyNotSetException.ThrowIfNull(PermanentItem);
                TestPropertyNotSetException.ThrowIfNull(ItemMock);
                ItemMock.SetupMakePermanent(PermanentItem, PermanentItem.Availabilities);
            }

            public void SetupStoringItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemMock);
                ItemRepositoryMock.SetupStoreAsync(ItemMock.Object, ItemMock.Object);
            }

            public void SetupFindingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(PermanentItem);
                TestPropertyNotSetException.ThrowIfNull(ItemMock);
                ItemRepositoryMock.SetupFindActiveByAsync(PermanentItem.Id, ItemMock.Object);
            }

            public void SetupNotFindingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(PermanentItem);
                ItemRepositoryMock.SetupFindActiveByAsync(PermanentItem.Id, null);
            }

            #region Verify

            public void VerifyValidatingItemCategory()
            {
                TestPropertyNotSetException.ThrowIfNull(PermanentItem);
                ValidatorMock.VerifyValidateAsync(PermanentItem.ItemCategoryId, Times.Once);
            }

            public void VerifyValidatingManufacturer()
            {
                TestPropertyNotSetException.ThrowIfNull(PermanentItem);
                TestPropertyNotSetException.ThrowIfNull(PermanentItem.ManufacturerId);
                ValidatorMock.VerifyValidateAsync(PermanentItem.ManufacturerId.Value, Times.Once);
            }

            public void VerifyNotValidatingManufacturer()
            {
                ValidatorMock.VerifyValidateAsyncNever_ManufacturerId();
            }

            public void VerifyValidatingAvailabilities()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemMock);
                ValidatorMock.VerifyValidateAsync(ItemMock.Object.Availabilities, Times.Once);
            }

            public void VerifyStoringItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemMock);
                ItemRepositoryMock.VerifyStoreAsync(ItemMock.Object, Times.Once);
            }

            #endregion Verify
        }
    }

    private abstract class LocalFixture
    {
        protected readonly ItemRepositoryMock ItemRepositoryMock = new(MockBehavior.Strict);
        protected readonly ValidatorMock ValidatorMock = new(MockBehavior.Strict);

        public TemporaryItemService CreateSut()
        {
            return new TemporaryItemService(
                ItemRepositoryMock.Object,
                ValidatorMock.Object);
        }
    }
}