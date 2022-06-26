using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Stores.Models.Factories;
using ShoppingList.Api.Domain.TestKit.Stores.Ports;
using ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Services;

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
        Func<Task> function = async () => await service.ValidateAsync(_local.Availabilities, default);

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
        Func<Task> function = async () => await service.ValidateAsync(_local.Availabilities, default);

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
        Func<Task> function = async () => await service.ValidateAsync(_local.Availabilities, default);

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
        Func<Task> function = async () => await service.ValidateAsync(_local.Availabilities, default);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().NotThrowAsync();
        }
    }

    private class LocalFixture
    {
        private readonly StoreRepositoryMock _storeRepositoryMock;
        private readonly StoreSectionFactoryMock _sectionFactoryMock;

        private readonly List<IStore> _stores = new();

        public LocalFixture()
        {
            _storeRepositoryMock = new StoreRepositoryMock(MockBehavior.Strict);
            _sectionFactoryMock = new StoreSectionFactoryMock(MockBehavior.Strict);
        }

        public List<IStoreItemAvailability>? Availabilities { get; private set; }

        public AvailabilityValidationService CreateSut()
        {
            return new AvailabilityValidationService(_storeRepositoryMock.Object);
        }

        public void SetupAvailabilitiesWithDuplicatedStoreIds()
        {
            Availabilities = new List<IStoreItemAvailability>();
            var availability = StoreItemAvailabilityMother.Initial().Create();
            var availability2 = StoreItemAvailabilityMother.Initial().WithStoreId(availability.StoreId).Create();
            Availabilities.Add(availability);
            Availabilities.Add(availability2);
        }

        public void SetupAvailabilities()
        {
            Availabilities =
                ((IEnumerable<IStoreItemAvailability>)StoreItemAvailabilityMother.Initial().CreateMany(3))
                .ToList();
        }

        public void SetupStores()
        {
            TestPropertyNotSetException.ThrowIfNull(Availabilities);

            foreach (var availability in Availabilities)
            {
                var section = new StoreSectionBuilder().WithId(availability.DefaultSectionId).Create();
                var sections = new StoreSections(section.ToMonoList(), _sectionFactoryMock.Object);
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
            _storeRepositoryMock.SetupFindByAsync(storeIds, _stores);
        }

        public void SetupFindingNoStores()
        {
            TestPropertyNotSetException.ThrowIfNull(Availabilities);
            var storeIds = Availabilities.Select(av => av.StoreId);
            _storeRepositoryMock.SetupFindByAsync(storeIds, Enumerable.Empty<IStore>());
        }

        #endregion Mock Setup
    }
}