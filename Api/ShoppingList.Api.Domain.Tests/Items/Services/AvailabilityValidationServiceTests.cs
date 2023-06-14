using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Services;

public class AvailabilityValidationServiceTests
{
    private readonly LocalFixture _local;

    public AvailabilityValidationServiceTests()
    {
        _local = new LocalFixture();
    }

    [Fact]
    public async Task ValidateAsync_WithDuplicatedStoreIds_ShouldThrowDomainException()
    {
        // Arrange
        var service = _local.CreateSut();
        _local.SetupAvailabilitiesWithDuplicatedStoreIds();

        TestPropertyNotSetException.ThrowIfNull(_local.Availabilities);

        // Act
        Func<Task> function = async () => await service.ValidateAsync(_local.Availabilities);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.MultipleAvailabilitiesForStore);
        }
    }

    [Fact]
    public async Task ValidateAsync_WithInvalidStoreId_ShouldThrowDomainException()
    {
        // Arrange
        var service = _local.CreateSut();
        _local.SetupAvailabilities();
        _local.SetupFindingNoStores();

        TestPropertyNotSetException.ThrowIfNull(_local.Availabilities);

        // Act
        Func<Task> function = async () => await service.ValidateAsync(_local.Availabilities);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.StoreNotFound);
        }
    }

    [Fact]
    public async Task ValidateAsync_WithSectionIdNotInStore_ShouldThrowDomainException()
    {
        // Arrange
        var service = _local.CreateSut();
        _local.SetupAvailabilities();
        _local.SetupStoresWithInvalidSectionIds();
        _local.SetupFindingStores();

        TestPropertyNotSetException.ThrowIfNull(_local.Availabilities);

        // Act
        Func<Task> function = async () => await service.ValidateAsync(_local.Availabilities);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.SectionInStoreNotFound);
        }
    }

    [Fact]
    public async Task ValidateAsync_WithValidData_ShouldNotThrow()
    {
        // Arrange
        var service = _local.CreateSut();
        _local.SetupAvailabilities();
        _local.SetupStores();
        _local.SetupFindingStores();

        TestPropertyNotSetException.ThrowIfNull(_local.Availabilities);

        // Act
        Func<Task> function = async () => await service.ValidateAsync(_local.Availabilities);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().NotThrowAsync();
        }
    }

    private class LocalFixture
    {
        private readonly StoreRepositoryMock _storeRepositoryMock;
        private readonly SectionFactoryMock _sectionFactoryMock;

        private readonly List<IStore> _stores = new();

        public LocalFixture()
        {
            _storeRepositoryMock = new StoreRepositoryMock(MockBehavior.Strict);
            _sectionFactoryMock = new SectionFactoryMock(MockBehavior.Strict);
        }

        public List<IItemAvailability>? Availabilities { get; private set; }

        public AvailabilityValidationService CreateSut()
        {
            return new AvailabilityValidationService(_ => _storeRepositoryMock.Object, default);
        }

        public void SetupAvailabilitiesWithDuplicatedStoreIds()
        {
            Availabilities = new List<IItemAvailability>();
            var availability = ItemAvailabilityMother.Initial().Create();
            var availability2 = ItemAvailabilityMother.Initial().WithStoreId(availability.StoreId).Create();
            Availabilities.Add(availability);
            Availabilities.Add(availability2);
        }

        public void SetupAvailabilities()
        {
            Availabilities =
                ((IEnumerable<IItemAvailability>)ItemAvailabilityMother.Initial().CreateMany(3))
                .ToList();
        }

        public void SetupStores()
        {
            TestPropertyNotSetException.ThrowIfNull(Availabilities);

            foreach (var availability in Availabilities)
            {
                var section = new SectionBuilder().WithId(availability.DefaultSectionId).Create();
                var sections = new Sections(section.ToMonoList(), _sectionFactoryMock.Object);
                var store = StoreMother.Initial()
                    .WithSections(sections)
                    .WithId(availability.StoreId)
                    .Create();
                _stores.Add(store);
            }
        }

        public void SetupStoresWithInvalidSectionIds()
        {
            TestPropertyNotSetException.ThrowIfNull(Availabilities);

            foreach (var availability in Availabilities)
            {
                var store = StoreMother.Initial()
                    .WithId(availability.StoreId)
                    .Create();
                _stores.Add(store);
            }
        }

        #region Mock Setup

        public void SetupFindingStores()
        {
            TestPropertyNotSetException.ThrowIfNull(Availabilities);
            var storeIds = Availabilities.Select(av => av.StoreId);
            _storeRepositoryMock.SetupFindActiveByAsync(storeIds, _stores);
        }

        public void SetupFindingNoStores()
        {
            TestPropertyNotSetException.ThrowIfNull(Availabilities);
            var storeIds = Availabilities.Select(av => av.StoreId);
            _storeRepositoryMock.SetupFindActiveByAsync(storeIds, Enumerable.Empty<IStore>());
        }

        #endregion Mock Setup
    }
}