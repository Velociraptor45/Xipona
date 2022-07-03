using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Services.Conversion.StoreItemReadModels;
using ShoppingList.Api.Domain.TestKit.StoreItems.Services.Validation;
using ShoppingList.Api.TestTools.AutoFixture;
using ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Services.ItemCreations;

public class ItemCreationServiceTests
{
    public class CreateAsyncTests
    {
        private readonly CreateAsyncFixture _fixture;

        public CreateAsyncTests()
        {
            _fixture = new CreateAsyncFixture();
        }

        #region WithManufacturerId

        [Fact]
        public async Task CreateAsync_WithManufacturerId_ShouldNotThrow()
        {
            // Arrange
            _fixture.SetupWithManufacturerId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCreation);

            // Act
            var func = async () => await sut.CreateAsync(_fixture.ItemCreation);

            // Assert
            using (new AssertionScope())
            {
                await func.Should().NotThrowAsync();
            }
        }

        [Fact]
        public async Task CreateAsync_WithManufacturerId_ShouldStoreItem()
        {
            // Arrange
            _fixture.SetupWithManufacturerId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCreation);

            // Act
            await sut.CreateAsync(_fixture.ItemCreation);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyStoreingItem();
            }
        }

        [Fact]
        public async Task CreateAsync_WithManufacturerId_ShouldValidateItemCategory()
        {
            // Arrange
            _fixture.SetupWithManufacturerId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCreation);

            // Act
            await sut.CreateAsync(_fixture.ItemCreation);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyValidateItemCategoryOnce();
            }
        }

        [Fact]
        public async Task CreateAsync_WithManufacturerId_ShouldValidateManufacturer()
        {
            // Arrange
            _fixture.SetupWithManufacturerId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCreation);

            // Act
            await sut.CreateAsync(_fixture.ItemCreation);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyValidateManufacturerOnce();
            }
        }

        [Fact]
        public async Task CreateAsync_WithManufacturerId_ShouldValidateAvailabilities()
        {
            // Arrange
            _fixture.SetupWithManufacturerId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCreation);

            // Act
            await sut.CreateAsync(_fixture.ItemCreation);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyValidateAvailabilitiesOnce();
            }
        }

        #endregion WithManufacturerId

        #region WithManufacturerIdNull

        [Fact]
        public async Task CreateAsync_WithManufacturerIdNull_ShouldNotThrow()
        {
            // Arrange
            _fixture.SetupWithManufacturerIdNull();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCreation);

            // Act
            var func = async () => await sut.CreateAsync(_fixture.ItemCreation);

            // Assert
            using (new AssertionScope())
            {
                await func.Should().NotThrowAsync();
            }
        }

        [Fact]
        public async Task CreateAsync_WithManufacturerIdNull_ShouldStoreItem()
        {
            // Arrange
            _fixture.SetupWithManufacturerIdNull();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCreation);

            // Act
            await sut.CreateAsync(_fixture.ItemCreation);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyStoreingItem();
            }
        }

        [Fact]
        public async Task CreateAsync_WithManufacturerIdNull_ShouldValidateItemCategory()
        {
            // Arrange
            _fixture.SetupWithManufacturerIdNull();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCreation);

            // Act
            await sut.CreateAsync(_fixture.ItemCreation);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyValidateItemCategoryOnce();
            }
        }

        [Fact]
        public async Task CreateAsync_WithManufacturerIdNull_ShouldNotValidateManufacturer()
        {
            // Arrange
            _fixture.SetupWithManufacturerIdNull();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCreation);

            // Act
            await sut.CreateAsync(_fixture.ItemCreation);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyValidateManufacturerNever();
            }
        }

        [Fact]
        public async Task CreateAsync_WithManufacturerIdNull_ShouldValidateAvailabilities()
        {
            // Arrange
            _fixture.SetupWithManufacturerIdNull();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCreation);

            // Act
            await sut.CreateAsync(_fixture.ItemCreation);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyValidateAvailabilitiesOnce();
            }
        }

        #endregion WithManufacturerIdNull

        private sealed class CreateAsyncFixture : LocalFixture
        {
            private IItem? _storeItem;
            private ManufacturerId? _manufacturerId;
            private List<IItemAvailability>? _availabilities;
            private StoreItemReadModel? _storeItemReadModel;

            public ItemCreation? ItemCreation { get; private set; }

            public void SetupItemCreation()
            {
                TestPropertyNotSetException.ThrowIfNull(_availabilities);
                Fixture.ConstructorArgumentFor<ItemCreation, ManufacturerId?>("manufacturerId", _manufacturerId);
                Fixture.ConstructorArgumentFor<ItemCreation, IEnumerable<IItemAvailability>>(
                    "availabilities", _availabilities);

                ItemCreation = Fixture.Create<ItemCreation>();
            }

            public void SetupManufacturerId()
            {
                _manufacturerId = new ManufacturerId(Guid.NewGuid());
            }

            public void SetupManufacturerIdNull()
            {
                _manufacturerId = null;
            }

            public void SetupAvailabilities()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeItem);
                _availabilities = _storeItem.Availabilities.ToList();
            }

            public void SetupStoreItem()
            {
                _storeItem = StoreItemMother.Initial().Create();
            }

            public void SetupStoreItemFactoryCreate()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemCreation);
                TestPropertyNotSetException.ThrowIfNull(_storeItem);
                StoreItemFactoryMock.SetupCreate(ItemCreation, _storeItem);
            }

            public void SetupValidatingItemCategory()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemCreation);
                ValidatorMock.SetupValidateAsync(ItemCreation.ItemCategoryId);
            }

            public void SetupValidatingManufacturer()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemCreation);
                TestPropertyNotSetException.ThrowIfNull(ItemCreation.ManufacturerId);
                ValidatorMock.SetupValidateAsync(ItemCreation.ManufacturerId.Value);
            }

            public void SetupValidatingAvailabilities()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemCreation);
                ValidatorMock.SetupValidateAsync(ItemCreation.Availabilities);
            }

            public void SetupStoringItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeItem);
                ItemRepositoryMock.SetupStoreAsync(_storeItem, _storeItem);
            }

            public void SetupConvertingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeItem);
                _storeItemReadModel = Fixture.Create<StoreItemReadModel>();
                ConversionServiceMock.SetupConvertAsync(_storeItem, _storeItemReadModel);
            }

            #region Verify

            public void VerifyValidateAvailabilitiesOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(_availabilities);
                ValidatorMock.VerifyValidateAsync(_availabilities, Times.Once);
            }

            public void VerifyValidateManufacturerOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemCreation);
                TestPropertyNotSetException.ThrowIfNull(ItemCreation.ManufacturerId);
                ValidatorMock.VerifyValidateAsync(ItemCreation.ManufacturerId.Value, Times.Once);
            }

            public void VerifyValidateManufacturerNever()
            {
                ValidatorMock.VerifyValidateAsyncNever_ManufacturerId();
            }

            public void VerifyValidateItemCategoryOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemCreation);
                ValidatorMock.VerifyValidateAsync(ItemCreation.ItemCategoryId, Times.Once);
            }

            public void VerifyStoreingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeItem);
                ItemRepositoryMock.VerifyStoreAsyncOnce(_storeItem);
            }

            #endregion Verify

            #region Aggregates

            public void SetupWithManufacturerIdNull()
            {
                SetupStoreItem();
                SetupAvailabilities();

                SetupManufacturerIdNull();
                SetupItemCreation();

                SetupStoreItemFactoryCreate();
                SetupStoringItem();

                SetupValidatingAvailabilities();
                SetupValidatingItemCategory();

                SetupConvertingItem();
            }

            public void SetupWithManufacturerId()
            {
                SetupStoreItem();
                SetupAvailabilities();

                SetupManufacturerId();
                SetupItemCreation();

                SetupStoreItemFactoryCreate();
                SetupStoringItem();

                SetupValidatingAvailabilities();
                SetupValidatingItemCategory();
                SetupValidatingManufacturer();

                SetupConvertingItem();
            }

            #endregion Aggregates
        }
    }

    public class CreateTemporaryAsyncTests
    {
        private readonly CreateTemporaryAsyncFixture _fixture;

        public CreateTemporaryAsyncTests()
        {
            _fixture = new CreateTemporaryAsyncFixture();
        }

        #region WithValidData

        [Fact]
        public async Task CreateTemporaryAsync_WithValidData_ShouldNotThrow()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupWithValidData();

            TestPropertyNotSetException.ThrowIfNull(_fixture.TemporaryItemCreation);

            // Act
            var func = async () => await sut.CreateTemporaryAsync(_fixture.TemporaryItemCreation);

            // Assert
            using (new AssertionScope())
            {
                await func.Should().NotThrowAsync();
            }
        }

        [Fact]
        public async Task CreateTemporaryAsync_WithValidData_ShouldValidateAvailabilities()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupWithValidData();

            TestPropertyNotSetException.ThrowIfNull(_fixture.TemporaryItemCreation);

            // Act
            await sut.CreateTemporaryAsync(_fixture.TemporaryItemCreation);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyValidatingAvailabilities();
            }
        }

        [Fact]
        public async Task CreateTemporaryAsync_WithValidData_ShouldStoreItem()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupWithValidData();

            TestPropertyNotSetException.ThrowIfNull(_fixture.TemporaryItemCreation);

            // Act
            await sut.CreateTemporaryAsync(_fixture.TemporaryItemCreation);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyStoringItem();
            }
        }

        #endregion WithValidData

        private sealed class CreateTemporaryAsyncFixture : LocalFixture
        {
            private IItem? _storeItem;
            private IItemAvailability? _availability;
            private StoreItemReadModel? _storeItemReadModel;
            public TemporaryItemCreation? TemporaryItemCreation { get; private set; }

            public void SetupCommand()
            {
                TestPropertyNotSetException.ThrowIfNull(_availability);
                Fixture.ConstructorArgumentFor<TemporaryItemCreation, IItemAvailability>("availability",
                    _availability);
                TemporaryItemCreation = Fixture.Create<TemporaryItemCreation>();
            }

            public void SetupStoreItem()
            {
                _storeItem = StoreItemMother.Initial().Create();
            }

            public void SetupRandomAvailability()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeItem);
                _availability = CommonFixture.ChooseRandom(_storeItem.Availabilities);
            }

            #region Mock Setup

            public void SetupCreatingStoreItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeItem);
                TestPropertyNotSetException.ThrowIfNull(TemporaryItemCreation);
                StoreItemFactoryMock.SetupCreate(TemporaryItemCreation, _storeItem);
            }

            public void SetupValidatingAvailabilities()
            {
                TestPropertyNotSetException.ThrowIfNull(_availability);
                ValidatorMock.SetupValidateAsync(_availability.ToMonoList());
            }

            public void SetupStoringItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeItem);
                ItemRepositoryMock.SetupStoreAsync(_storeItem, _storeItem);
            }

            public void SetupConvertingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeItem);
                _storeItemReadModel = Fixture.Create<StoreItemReadModel>();
                ConversionServiceMock.SetupConvertAsync(_storeItem, _storeItemReadModel);
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyValidatingAvailabilities()
            {
                TestPropertyNotSetException.ThrowIfNull(TemporaryItemCreation);
                ValidatorMock.VerifyValidateAsync(TemporaryItemCreation.Availability.ToMonoList(), Times.Once);
            }

            public void VerifyStoringItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeItem);
                ItemRepositoryMock.VerifyStoreAsyncOnce(_storeItem);
            }

            #endregion Verify

            #region Aggregates

            public void SetupWithValidData()
            {
                SetupStoreItem();
                SetupRandomAvailability();
                SetupCommand();
                SetupCreatingStoreItem();
                SetupStoringItem();

                SetupValidatingAvailabilities();
                SetupConvertingItem();
            }

            #endregion Aggregates
        }
    }

    public abstract class LocalFixture
    {
        protected Fixture Fixture;
        protected CommonFixture CommonFixture = new();
        protected ItemRepositoryMock ItemRepositoryMock;
        protected StoreItemFactoryMock StoreItemFactoryMock;
        protected ValidatorMock ValidatorMock;
        protected StoreItemReadModelConversionServiceMock ConversionServiceMock;

        protected LocalFixture()
        {
            Fixture = CommonFixture.GetNewFixture();

            ItemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
            StoreItemFactoryMock = new StoreItemFactoryMock(MockBehavior.Strict);
            ValidatorMock = new ValidatorMock(MockBehavior.Strict);
            ConversionServiceMock = new StoreItemReadModelConversionServiceMock(MockBehavior.Strict);
        }

        public ItemCreationService CreateSut()
        {
            return new ItemCreationService(
                ItemRepositoryMock.Object,
                _ => ValidatorMock.Object,
                StoreItemFactoryMock.Object,
                ConversionServiceMock.Object,
                default);
        }
    }
}