using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Services.Conversion.StoreItemReadModels;
using ShoppingList.Api.Domain.TestKit.StoreItems.Services.Validation;

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

        [Fact]
        public async Task CreateAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var sut = _fixture.CreateSut();

            // Act
            var func = async () => await sut.CreateAsync(null);

            // Assert
            using (new AssertionScope())
            {
                await func.Should().ThrowExactlyAsync<ArgumentNullException>().WithMessage("*creation*");
            }
        }

        #region WithManufacturerId

        [Fact]
        public async Task CreateAsync_WithManufacturerId_ShouldNotThrow()
        {
            // Arrange
            _fixture.SetupWithManufacturerId();
            var sut = _fixture.CreateSut();

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
            private IStoreItem _storeItem;
            private ManufacturerId? _manufacturerId;
            private List<IStoreItemAvailability> _availabilities;

            public ItemCreation ItemCreation { get; private set; }
            public StoreItemReadModel StoreItemReadModel { get; private set; }

            public void SetupItemCreation()
            {
                Fixture.ConstructorArgumentFor<ItemCreation, ManufacturerId?>("manufacturerId", _manufacturerId);
                Fixture.ConstructorArgumentFor<ItemCreation, IEnumerable<IStoreItemAvailability>>(
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
                _availabilities = _storeItem.Availabilities.ToList();
            }

            public void SetupStoreItem()
            {
                _storeItem = StoreItemMother.Initial().Create();
            }

            public void SetupStoreItemFactoryCreate()
            {
                StoreItemFactoryMock.SetupCreate(ItemCreation, _storeItem);
            }

            public void SetupValidatingItemCategory()
            {
                ValidatorMock.SetupValidateAsync(ItemCreation.ItemCategoryId);
            }

            public void SetupValidatingManufacturer()
            {
                ValidatorMock.SetupValidateAsync(ItemCreation.ManufacturerId.Value);
            }

            public void SetupValidatingAvailabilities()
            {
                ValidatorMock.SetupValidateAsync(ItemCreation.Availabilities);
            }

            public void SetupStoringItem()
            {
                ItemRepositoryMock.SetupStoreAsync(_storeItem, _storeItem);
            }

            public void SetupConvertingItem()
            {
                StoreItemReadModel = Fixture.Create<StoreItemReadModel>();
                ConversionServiceMock.SetupConvertAsync(_storeItem, StoreItemReadModel);
            }

            #region Verify

            public void VerifyValidateAvailabilitiesOnce()
            {
                ValidatorMock.VerifyValidateAsync(_availabilities, Times.Once);
            }

            public void VerifyValidateManufacturerOnce()
            {
                ValidatorMock.VerifyValidateAsync(ItemCreation.ManufacturerId.Value, Times.Once);
            }

            public void VerifyValidateManufacturerNever()
            {
                ValidatorMock.VerifyValidateAsyncNever_ManufacturerId();
            }

            public void VerifyValidateItemCategoryOnce()
            {
                ValidatorMock.VerifyValidateAsync(ItemCreation.ItemCategoryId, Times.Once);
            }

            public void VerifyStoreingItem()
            {
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

        [Fact]
        public async Task CreateTemporaryAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var sut = _fixture.CreateSut();

            // Act
            Func<Task> func = async () => await sut.CreateTemporaryAsync(null);

            // Assert
            using (new AssertionScope())
            {
                await func.Should().ThrowAsync<ArgumentNullException>().WithMessage("*creation*");
            }
        }

        #region WithValidData

        [Fact]
        public async Task CreateTemporaryAsync_WithValidData_ShouldNotThrow()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupWithValidData();

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
            private IStoreItem _storeItem;
            private IStoreItemAvailability _availability;
            public TemporaryItemCreation TemporaryItemCreation { get; private set; }

            public void SetupCommand()
            {
                Fixture.ConstructorArgumentFor<TemporaryItemCreation, IStoreItemAvailability>("availability",
                    _availability);
                TemporaryItemCreation = Fixture.Create<TemporaryItemCreation>();
            }

            public void SetupStoreItem()
            {
                _storeItem = StoreItemMother.Initial().Create();
            }

            public void SetupRandomAvailability()
            {
                _availability = CommonFixture.ChooseRandom(_storeItem.Availabilities);
            }

            #region Mock Setup

            public void SetupCreatingStoreItem()
            {
                StoreItemFactoryMock.SetupCreate(TemporaryItemCreation, _storeItem);
            }

            public void SetupValidatingAvailabilities()
            {
                ValidatorMock.SetupValidateAsync(_availability.ToMonoList());
            }

            public void SetupStoringItem()
            {
                ItemRepositoryMock.SetupStoreAsync(_storeItem, _storeItem);
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyValidatingAvailabilities()
            {
                ValidatorMock.VerifyValidateAsync(TemporaryItemCreation.Availability.ToMonoList(), Times.Once);
            }

            public void VerifyStoringItem()
            {
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