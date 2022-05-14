using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.TemporaryItems;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Services.Validation;
using ShoppingList.Api.TestTools.AutoFixture;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Services.TemporaryItems;

public class TemporaryItemServiceTests
{
    public class MakePermanentAsyncTests
    {
        private readonly MakePermanentAsyncFixture _fixture;

        public MakePermanentAsyncTests()
        {
            _fixture = new MakePermanentAsyncFixture();
        }

        [Fact]
        public async Task MakePermanentAsync_WithPermanentItemIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var sut = _fixture.CreateSut();

            // Act
            Func<Task> func = async () => await sut.MakePermanentAsync(null);

            // Assert
            using (new AssertionScope())
            {
                await func.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task MakePermanentAsync_WithInvalidItemId_ShouldThrowDomainException()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupPermanentItem();
            _fixture.SetupNotFindingItem();

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
            _fixture.SetupStoreItemMock();
            _fixture.SetupFindingItem();

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
            _fixture.SetupTemporaryStoreItemMock();

            List<IStoreItemAvailability> availabilities = _fixture.StoreItemMock.Object.Availabilities.ToList();
            _fixture.SetupPermanentItem(availabilities);
            _fixture.SetupValidatingAvailabilities();
            _fixture.SetupValidatingItemCategory();
            _fixture.SetupValidatingManufacturer();
            _fixture.SetupMakingPermanent();
            _fixture.SetupStoringItem();
            _fixture.SetupFindingItem();

            // Act
            await sut.MakePermanentAsync(_fixture.PermanentItem);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyValidatingItemCategory();
                _fixture.VerifyValidatingManufacturer();
                _fixture.VerifyValidatingAvailabilities();
                _fixture.StoreItemMock.VerifyMakePermanentOnce(_fixture.PermanentItem, availabilities);
                _fixture.VerifyStoringItem();
            }
        }

        [Fact]
        public async Task MakePermanentAsync_WithValidDataAndManufacturerIdIsNull_ShouldMakeTemporaryItemPermanent()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupTemporaryStoreItemMock();

            List<IStoreItemAvailability> availabilities = _fixture.StoreItemMock.Object.Availabilities.ToList();
            _fixture.SetupCommandWithoutManufacturerId(availabilities);
            _fixture.SetupValidatingAvailabilities();
            _fixture.SetupValidatingItemCategory();
            _fixture.SetupMakingPermanent();
            _fixture.SetupStoringItem();
            _fixture.SetupFindingItem();

            // Act
            await sut.MakePermanentAsync(_fixture.PermanentItem);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyValidatingItemCategory();
                _fixture.VerifyNotValidatingManufacturer();
                _fixture.VerifyValidatingAvailabilities();
                _fixture.StoreItemMock.VerifyMakePermanentOnce(_fixture.PermanentItem, availabilities);
                _fixture.VerifyStoringItem();
            }
        }

        private sealed class MakePermanentAsyncFixture : LocalFixture
        {
            public PermanentItem PermanentItem { get; private set; }
            public StoreItemMock StoreItemMock { get; private set; }

            public void SetupPermanentItem()
            {
                PermanentItem = Fixture.Create<PermanentItem>();
            }

            public void SetupPermanentItem(IEnumerable<IStoreItemAvailability> availabilities)
            {
                Fixture.ConstructorArgumentFor<PermanentItem, IEnumerable<IStoreItemAvailability>>(
                    "availabilities", availabilities);

                PermanentItem = Fixture.Create<PermanentItem>();
            }

            public void SetupCommandWithoutManufacturerId(
                IEnumerable<IStoreItemAvailability> availabilities)
            {
                Fixture.ConstructorArgumentFor<PermanentItem, ManufacturerId?>("manufacturerId", null);
                Fixture.ConstructorArgumentFor<PermanentItem, IEnumerable<IStoreItemAvailability>>(
                    "availabilities", availabilities);

                PermanentItem = Fixture.Create<PermanentItem>();
            }

            public void SetupStoreItemMock()
            {
                StoreItemMock = new StoreItemMock(StoreItemMother.Initial().Create(), MockBehavior.Strict);
            }

            public void SetupTemporaryStoreItemMock()
            {
                StoreItemMock = new StoreItemMock(StoreItemMother.InitialTemporary().Create(), MockBehavior.Strict);
            }

            public void SetupValidatingItemCategory()
            {
                ValidatorMock.SetupValidateAsync(PermanentItem.ItemCategoryId);
            }

            public void SetupValidatingManufacturer()
            {
                ValidatorMock.SetupValidateAsync(PermanentItem.ManufacturerId.Value);
            }

            public void SetupValidatingAvailabilities()
            {
                ValidatorMock.SetupValidateAsync(StoreItemMock.Object.Availabilities);
            }

            public void SetupMakingPermanent()
            {
                StoreItemMock.SetupMakePermanent(PermanentItem, PermanentItem.Availabilities);
            }

            public void SetupStoringItem()
            {
                ItemRepositoryMock.SetupStoreAsync(StoreItemMock.Object, StoreItemMock.Object);
            }

            public void SetupFindingItem()
            {
                ItemRepositoryMock.SetupFindByAsync(PermanentItem.Id, StoreItemMock.Object);
            }

            public void SetupNotFindingItem()
            {
                ItemRepositoryMock.SetupFindByAsync(PermanentItem.Id, null);
            }

            #region Verify

            public void VerifyValidatingItemCategory()
            {
                ValidatorMock.VerifyValidateAsync(PermanentItem.ItemCategoryId, Times.Once);
            }

            public void VerifyValidatingManufacturer()
            {
                ValidatorMock.VerifyValidateAsync(PermanentItem.ManufacturerId.Value, Times.Once);
            }

            public void VerifyNotValidatingManufacturer()
            {
                ValidatorMock.VerifyValidateAsyncNever_ManufacturerId();
            }

            public void VerifyValidatingAvailabilities()
            {
                ValidatorMock.VerifyValidateAsync(StoreItemMock.Object.Availabilities, Times.Once);
            }

            public void VerifyStoringItem()
            {
                ItemRepositoryMock.VerifyStoreAsync(StoreItemMock.Object, Times.Once);
            }

            #endregion Verify
        }
    }

    private abstract class LocalFixture
    {
        protected Fixture Fixture { get; }
        protected ItemRepositoryMock ItemRepositoryMock;
        protected ValidatorMock ValidatorMock;

        protected LocalFixture()
        {
            Fixture = new CommonFixture().GetNewFixture();
            ItemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
            ValidatorMock = new ValidatorMock(MockBehavior.Strict);
        }

        public TemporaryItemService CreateSut()
        {
            return new TemporaryItemService(
                ItemRepositoryMock.Object,
                _ => ValidatorMock.Object,
                default);
        }
    }
}