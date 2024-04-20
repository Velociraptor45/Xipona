using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Creations;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Conversion.ItemReadModels;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.Xipona.Api.TestTools.AutoFixture;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Items.Services.ItemCreations;

public class ItemCreationServiceTests
{
    public class CreateAsyncTests
    {
        private readonly CreateAsyncFixture _fixture = new();

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
                _fixture.VerifyStoringItem();
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
                _fixture.VerifyStoringItem();
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
            private IItem? _item;
            private ManufacturerId? _manufacturerId;
            private List<ItemAvailability>? _availabilities;
            private ItemReadModel? _itemReadModel;

            public ItemCreation? ItemCreation { get; private set; }

            public void SetupItemCreation()
            {
                TestPropertyNotSetException.ThrowIfNull(_availabilities);

                ItemCreation = new DomainTestBuilder<ItemCreation>()
                    .ConstructorArgumentFor<ItemCreation, ManufacturerId?>("manufacturerId", _manufacturerId)
                    .ConstructorArgumentFor<ItemCreation, IEnumerable<ItemAvailability>>("availabilities", _availabilities)
                    .Create<ItemCreation>();
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
                TestPropertyNotSetException.ThrowIfNull(_item);
                _availabilities = _item.Availabilities.ToList();
            }

            public void SetupItem()
            {
                _item = ItemMother.Initial().Create();
            }

            public void SetupItemFactoryCreate()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemCreation);
                TestPropertyNotSetException.ThrowIfNull(_item);
                ItemFactoryMock.SetupCreate(ItemCreation, _item);
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
                TestPropertyNotSetException.ThrowIfNull(_item);
                ItemRepositoryMock.SetupStoreAsync(_item, _item);
            }

            public void SetupConvertingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                _itemReadModel = new DomainTestBuilder<ItemReadModel>().Create();
                ConversionServiceMock.SetupConvertAsync(_item, _itemReadModel);
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

            public void VerifyStoringItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                ItemRepositoryMock.VerifyStoreAsyncOnce(_item);
            }

            #endregion Verify

            #region Aggregates

            public void SetupWithManufacturerIdNull()
            {
                SetupItem();
                SetupAvailabilities();

                SetupManufacturerIdNull();
                SetupItemCreation();

                SetupItemFactoryCreate();
                SetupStoringItem();

                SetupValidatingAvailabilities();
                SetupValidatingItemCategory();

                SetupConvertingItem();
            }

            public void SetupWithManufacturerId()
            {
                SetupItem();
                SetupAvailabilities();

                SetupManufacturerId();
                SetupItemCreation();

                SetupItemFactoryCreate();
                SetupStoringItem();

                SetupValidatingAvailabilities();
                SetupValidatingItemCategory();
                SetupValidatingManufacturer();

                SetupConvertingItem();
            }

            #endregion Aggregates
        }
    }

    public abstract class LocalFixture
    {
        protected ItemRepositoryMock ItemRepositoryMock = new(MockBehavior.Strict);
        protected ItemFactoryMock ItemFactoryMock = new(MockBehavior.Strict);
        protected ValidatorMock ValidatorMock = new(MockBehavior.Strict);
        protected ItemReadModelConversionServiceMock ConversionServiceMock = new(MockBehavior.Strict);

        public ItemCreationService CreateSut()
        {
            return new ItemCreationService(
                ItemRepositoryMock.Object,
                ValidatorMock.Object,
                ItemFactoryMock.Object,
                ConversionServiceMock.Object);
        }
    }
}