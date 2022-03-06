using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Services;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Services;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Services;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.CreateItem;

public class CreateItemCommandHandlerTests
{
    private readonly LocalFixture _local;

    public CreateItemCommandHandlerTests()
    {
        _local = new LocalFixture();
    }

    [Fact]
    public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var handler = _local.CreateSut();

        // Act
        Func<Task<bool>> action = async () => await handler.HandleAsync(null, default);

        // Assert
        using (new AssertionScope())
        {
            await action.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }

    #region WithManufacturerId

    [Fact]
    public async Task HandleAsync_WithManufacturerId_ShouldReturnTrue()
    {
        // Arrange
        _local.SetupWithManufacturerId();
        var handler = _local.CreateSut();

        // Act
        var result = await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleAsync_WithManufacturerId_ShouldStoreItem()
    {
        // Arrange
        _local.SetupWithManufacturerId();
        var handler = _local.CreateSut();

        // Act
        await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            _local.VerifyStoreingItem();
        }
    }

    [Fact]
    public async Task HandleAsync_WithManufacturerId_ShouldValidateItemCategory()
    {
        // Arrange
        _local.SetupWithManufacturerId();
        var handler = _local.CreateSut();

        // Act
        await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            _local.VerifyValidateItemCategoryOnce();
        }
    }

    [Fact]
    public async Task HandleAsync_WithManufacturerId_ShouldValidateManufacturer()
    {
        // Arrange
        _local.SetupWithManufacturerId();
        var handler = _local.CreateSut();

        // Act
        await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            _local.VerifyValidateManufacturerOnce();
        }
    }

    [Fact]
    public async Task HandleAsync_WithManufacturerId_ShouldValidateAvailabilities()
    {
        // Arrange
        _local.SetupWithManufacturerId();
        var handler = _local.CreateSut();

        // Act
        await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            _local.VerifyValidateAvailabilitiesOnce();
        }
    }

    #endregion WithManufacturerId

    #region WithManufacturerIdNull

    [Fact]
    public async Task HandleAsync_WithManufacturerIdNull_ShouldReturnTrue()
    {
        // Arrange
        _local.SetupWithManufacturerIdNull();
        var handler = _local.CreateSut();

        // Act
        var result = await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
        }
    }

    [Fact]
    public async Task HandleAsync_WithManufacturerIdNull_ShouldStoreItem()
    {
        // Arrange
        _local.SetupWithManufacturerIdNull();
        var handler = _local.CreateSut();

        // Act
        await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            _local.VerifyStoreingItem();
        }
    }

    [Fact]
    public async Task HandleAsync_WithManufacturerIdNull_ShouldValidateItemCategory()
    {
        // Arrange
        _local.SetupWithManufacturerIdNull();
        var handler = _local.CreateSut();

        // Act
        await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            _local.VerifyValidateItemCategoryOnce();
        }
    }

    [Fact]
    public async Task HandleAsync_WithManufacturerIdNull_ShouldNotValidateManufacturer()
    {
        // Arrange
        _local.SetupWithManufacturerIdNull();
        var handler = _local.CreateSut();

        // Act
        await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            _local.VerifyValidateManufacturerNever();
        }
    }

    [Fact]
    public async Task HandleAsync_WithManufacturerIdNull_ShouldValidateAvailabilities()
    {
        // Arrange
        _local.SetupWithManufacturerIdNull();
        var handler = _local.CreateSut();

        // Act
        await handler.HandleAsync(_local.Command, default);

        // Assert
        using (new AssertionScope())
        {
            _local.VerifyValidateAvailabilitiesOnce();
        }
    }

    #endregion WithManufacturerIdNull

    private sealed class LocalFixture
    {
        public Fixture Fixture { get; }
        public CommonFixture CommonFixture { get; } = new CommonFixture();
        public ItemRepositoryMock ItemRepositoryMock { get; }
        public StoreItemFactoryMock StoreItemFactoryMock { get; }
        public ItemCategoryValidationServiceMock ItemCategoryValidationServiceMock { get; }
        public ManufacturerValidationServiceMock ManufacturerValidationServiceMock { get; }
        public AvailabilityValidationServiceMock AvailabilityValidationServiceMock { get; }
        public IStoreItem StoreItem { get; private set; }
        public CreateItemCommand Command { get; private set; }
        public ManufacturerId? ManufacturerId { get; private set; }
        public List<IStoreItemAvailability> Availabilities { get; private set; }

        public LocalFixture()
        {
            Fixture = CommonFixture.GetNewFixture();

            ItemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
            StoreItemFactoryMock = new StoreItemFactoryMock(MockBehavior.Strict);
            ItemCategoryValidationServiceMock = new ItemCategoryValidationServiceMock(MockBehavior.Strict);
            ManufacturerValidationServiceMock = new ManufacturerValidationServiceMock(MockBehavior.Strict);
            AvailabilityValidationServiceMock = new AvailabilityValidationServiceMock(MockBehavior.Strict);
        }

        public void SetupCommand()
        {
            Fixture.ConstructorArgumentFor<ItemCreation, ManufacturerId?>("manufacturerId", ManufacturerId);
            Fixture.ConstructorArgumentFor<ItemCreation, IEnumerable<IStoreItemAvailability>>(
                "availabilities", Availabilities);

            Command = Fixture.Create<CreateItemCommand>();
        }

        public CreateItemCommandHandler CreateSut()
        {
            return new CreateItemCommandHandler(
                ItemCategoryValidationServiceMock.Object,
                ManufacturerValidationServiceMock.Object,
                AvailabilityValidationServiceMock.Object,
                ItemRepositoryMock.Object,
                StoreItemFactoryMock.Object);
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
            Availabilities = StoreItem.Availabilities.ToList();
        }

        public void SetupStoreItem()
        {
            StoreItem = StoreItemMother.Initial().Create();
        }

        public void SetupStoreItemFactoryCreate()
        {
            StoreItemFactoryMock.SetupCreate(Command.ItemCreation, StoreItem);
        }

        public void SetupValidatingItemCategory()
        {
            ItemCategoryValidationServiceMock.SetupValidateAsync(Command.ItemCreation.ItemCategoryId);
        }

        public void SetupValidatingManufacturer()
        {
            ManufacturerValidationServiceMock.SetupValidateAsync(Command.ItemCreation.ManufacturerId.Value);
        }

        public void SetupValidatingAvailabilities()
        {
            AvailabilityValidationServiceMock.SetupValidateAsync(Command.ItemCreation.Availabilities);
        }

        public void SetupStoringItem()
        {
            ItemRepositoryMock.SetupStoreAsync(StoreItem, StoreItem);
        }

        #region Verify

        public void VerifyValidateAvailabilitiesOnce()
        {
            AvailabilityValidationServiceMock.VerifyValidateOnce(Availabilities);
        }

        public void VerifyValidateManufacturerOnce()
        {
            ManufacturerValidationServiceMock.VerifyValidateAsyncOnce(Command.ItemCreation.ManufacturerId.Value);
        }

        public void VerifyValidateManufacturerNever()
        {
            ManufacturerValidationServiceMock.VerifyValidateAsyncNever();
        }

        public void VerifyValidateItemCategoryOnce()
        {
            ItemCategoryValidationServiceMock.VerifyValidateAsyncOnce(Command.ItemCreation.ItemCategoryId);
        }

        public void VerifyStoreingItem()
        {
            ItemRepositoryMock.VerifyStoreAsyncOnce(StoreItem);
        }

        #endregion Verify

        #region Aggregates

        public void SetupWithManufacturerIdNull()
        {
            SetupStoreItem();
            SetupAvailabilities();

            SetupManufacturerIdNull();
            SetupCommand();

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
            SetupCommand();

            SetupStoreItemFactoryCreate();
            SetupStoringItem();

            SetupValidatingAvailabilities();
            SetupValidatingItemCategory();
            SetupValidatingManufacturer();
        }

        #endregion Aggregates
    }
}