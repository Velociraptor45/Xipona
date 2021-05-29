using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.Stores.Fixtures;
using ShoppingList.Api.Domain.TestKit.Stores.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Services
{
    public class AvailabilityValidationServiceTests
    {
        [Fact]
        public async Task ValidateAsync_WithAvailabilitiesIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            // Act
            Func<Task> function = async () => await service.ValidateAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task ValidateAsync_WithDuplicatedStoreIds_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();
            var availabilities = local.CreateAvailabilitiesWithDuplicatedStoreIds();

            // Act
            Func<Task> function = async () => await service.ValidateAsync(availabilities, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.MultipleAvailabilitiesForStore);
            }
        }

        [Fact]
        public async Task ValidateAsync_WithInvalidStoreId_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();
            var availabilities = local.CreateValidAvailabilities();

            var storeIds = availabilities.Select(av => av.StoreId);
            local.StoreRepositoryMock.SetupFindByAsync(storeIds, Enumerable.Empty<IStore>());

            // Act
            Func<Task> function = async () => await service.ValidateAsync(availabilities, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.StoreNotFound);
            }
        }

        [Fact]
        public async Task ValidateAsync_WithSectionIdNotInStore_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();
            var availabilities = local.CreateValidAvailabilities().ToList();

            var storeIds = availabilities.Select(av => av.StoreId);
            var stores = local.CreateStoresWithInvalidSections(availabilities).ToList();
            local.StoreRepositoryMock.SetupFindByAsync(storeIds, stores);

            // Act
            Func<Task> function = async () => await service.ValidateAsync(availabilities, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.SectionInStoreNotFound);
            }
        }

        [Fact]
        public async Task ValidateAsync_WithValidData_ShouldNotThrow()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();
            var availabilities = local.CreateValidAvailabilities().ToList();

            var storeIds = availabilities.Select(av => av.StoreId);
            var stores = local.CreateValidStores(availabilities);
            local.StoreRepositoryMock.SetupFindByAsync(storeIds, stores);

            // Act
            Func<Task> function = async () => await service.ValidateAsync(availabilities, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().NotThrowAsync();
            }
        }

        private class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public StoreRepositoryMock StoreRepositoryMock { get; }
            public StoreItemAvailabilityFixture StoreItemAvailabilityFixture { get; }
            public StoreSectionFixture StoreSectionFixture { get; }
            public StoreFixture StoreFixture { get; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                StoreRepositoryMock = new StoreRepositoryMock(Fixture);

                StoreItemAvailabilityFixture = new StoreItemAvailabilityFixture(CommonFixture);
                StoreSectionFixture = new StoreSectionFixture(CommonFixture);
                StoreFixture = new StoreFixture(CommonFixture);
            }

            public AvailabilityValidationService CreateService()
            {
                return Fixture.Create<AvailabilityValidationService>();
            }

            public IEnumerable<IStoreItemAvailability> CreateAvailabilitiesWithDuplicatedStoreIds()
            {
                var availabilities = StoreItemAvailabilityFixture.CreateManyValid().ToList();

                var def = new StoreItemAvailabilityDefinition
                {
                    StoreId = CommonFixture.ChooseRandom(availabilities).StoreId
                };
                availabilities.Add(StoreItemAvailabilityFixture.Create(def));

                return availabilities;
            }

            public IEnumerable<IStoreItemAvailability> CreateValidAvailabilities()
            {
                return StoreItemAvailabilityFixture.CreateManyValid();
            }

            public IEnumerable<IStore> CreateStoresWithInvalidSections(IEnumerable<IStoreItemAvailability> availabilities)
            {
                foreach (var av in availabilities.ToList())
                {
                    var def = new StoreDefinition
                    {
                        Id = av.StoreId,
                        Sections = Enumerable.Empty<IStoreSection>()
                    };
                    yield return StoreFixture.Create(def);
                }
            }

            public IEnumerable<IStore> CreateValidStores(IEnumerable<IStoreItemAvailability> availabilities)
            {
                var availabilitiesList = availabilities.ToList();
                foreach (var availability in availabilitiesList)
                {
                    var section = StoreSectionFixture.Create(StoreSectionDefinition.FromId(availability.DefaultSectionId));

                    var def = new StoreDefinition
                    {
                        Id = availability.StoreId,
                        Sections = section.ToMonoList()
                    };
                    yield return StoreFixture.CreateValid(def);
                }
            }
        }
    }
}