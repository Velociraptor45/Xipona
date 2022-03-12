using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemCreations;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Services.Validation;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Services.ItemCreations
{
    public class ItemCreationServiceTests
    {
        public class ItemCreationService_CreateAsyncTests
        {
            private readonly CreateAsyncFixture _fixture;

            public ItemCreationService_CreateAsyncTests()
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
                public ItemCreation ItemCreation { get; private set; }
                public ManufacturerId? ManufacturerId { get; private set; }
                public List<IStoreItemAvailability> Availabilities { get; private set; }

                public void SetupItemCreation()
                {
                    Fixture.ConstructorArgumentFor<ItemCreation, ManufacturerId?>("manufacturerId", ManufacturerId);
                    Fixture.ConstructorArgumentFor<ItemCreation, IEnumerable<IStoreItemAvailability>>(
                        "availabilities", Availabilities);

                    ItemCreation = Fixture.Create<ItemCreation>();
                }

                public void SetupManufacturerId()
                {
                    ManufacturerId = new ManufacturerId(Guid.NewGuid());
                }

                public void SetupManufacturerIdNull()
                {
                    ManufacturerId = null;
                }

                public void SetupAvailabilities()
                {
                    Availabilities = _storeItem.Availabilities.ToList();
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

                #region Verify

                public void VerifyValidateAvailabilitiesOnce()
                {
                    ValidatorMock.VerifyValidateAsync(Availabilities, Times.Once);
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
                }

                #endregion Aggregates
            }
        }

        public abstract class LocalFixture
        {
            protected Fixture Fixture;
            protected ItemRepositoryMock ItemRepositoryMock;
            protected StoreItemFactoryMock StoreItemFactoryMock;
            protected ValidatorMock ValidatorMock;

            protected LocalFixture()
            {
                Fixture = new CommonFixture().GetNewFixture();

                ItemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
                StoreItemFactoryMock = new StoreItemFactoryMock(MockBehavior.Strict);
                ValidatorMock = new ValidatorMock(MockBehavior.Strict);
            }

            public ItemCreationService CreateSut()
            {
                return new ItemCreationService(
                    ItemRepositoryMock.Object,
                    _ => ValidatorMock.Object,
                    StoreItemFactoryMock.Object,
                    default);
            }
        }
    }
}