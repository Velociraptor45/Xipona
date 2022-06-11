using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using ShoppingList.Frontend.Models.TestKit.Stores.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ShoppingList.Frontend.Models.Tests.Items.Models;

public class AvailableTests
{
    public class AddStore
    {
        private readonly AddStoreFixture _fixture;

        public AddStore()
        {
            _fixture = new AddStoreFixture();
        }

        [Fact]
        public void AddStore_WithEmptyStores_ShouldReturnFalse()
        {
            // Arrange
            var sut = _fixture.CreateSut();

            // Act
            var result = sut.AddStore(Enumerable.Empty<Store>());

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void AddStore_WithEmptyStores_ShouldNotAddAvailability()
        {
            // Arrange
            var sut = _fixture.CreateSut();

            // Act
            sut.AddStore(Enumerable.Empty<Store>());

            // Assert
            sut.Availabilities.Should().BeEmpty();
        }

        [Fact]
        public void AddStore_WithNotRegisteredStore_ShouldReturnTrue()
        {
            // Arrange
            _fixture.SetupStores();
            var sut = _fixture.CreateSut();

            // Act
            var result = sut.AddStore(_fixture.Stores);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void AddStore_WithNotRegisteredStore_ShouldHaveExpectedAvailability()
        {
            // Arrange
            _fixture.SetupStores();
            _fixture.SetupExpectedAvailability(0);
            var sut = _fixture.CreateSut();

            // Act
            sut.AddStore(_fixture.Stores);

            // Assert
            sut.Availabilities.Should().HaveCount(1);
            sut.Availabilities.First().Should().BeEquivalentTo(_fixture.ExpectedAvailability);
        }

        [Fact]
        public void AddStore_WithExistingAvailability_ShouldChooseNotRegisteredStore()
        {
            // Arrange
            _fixture.SetupStores();
            _fixture.SetupExpectedAvailability(1);
            var sut = _fixture.CreateSut();
            _fixture.AddAvailability(sut.Availabilities, 0);

            // Act
            sut.AddStore(_fixture.Stores);

            // Assert
            sut.Availabilities.Should().HaveCount(2);
            sut.Availabilities[1].Should().BeEquivalentTo(_fixture.ExpectedAvailability);
        }

        [Fact]
        public void AddStore_WithExistingAvailability_ShouldReturnTrue()
        {
            // Arrange
            _fixture.SetupStores();
            _fixture.SetupExpectedAvailability(1);
            var sut = _fixture.CreateSut();
            _fixture.AddAvailability(sut.Availabilities, 0);

            // Act
            var result = sut.AddStore(_fixture.Stores);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void AddStore_WithAllStoresRegistered_ShouldNotAddAvailability()
        {
            // Arrange
            _fixture.SetupStores();
            var sut = _fixture.CreateSut();
            _fixture.AddAvailability(sut.Availabilities, 0);
            _fixture.AddAvailability(sut.Availabilities, 1);

            // Act
            sut.AddStore(_fixture.Stores);

            // Assert
            sut.Availabilities.Should().HaveCount(2);
        }

        [Fact]
        public void AddStore_WithAllStoresRegistered_ShouldReturnFalse()
        {
            // Arrange
            _fixture.SetupStores();
            var sut = _fixture.CreateSut();
            _fixture.AddAvailability(sut.Availabilities, 0);
            _fixture.AddAvailability(sut.Availabilities, 1);

            // Act
            var result = sut.AddStore(_fixture.Stores);

            // Assert
            result.Should().BeFalse();
        }

        private class AddStoreFixture : AvailableFixture
        {
            public ItemAvailability ExpectedAvailability { get; private set; }

            public void SetupExpectedAvailability(int index)
            {
                var store = Stores.ElementAt(index);
                ExpectedAvailability = CreateAvailability(store);
            }
        }
    }

    public class GetNotRegisteredStores
    {
        private readonly GetNotRegisteredStoresFixture _fixture;

        public GetNotRegisteredStores()
        {
            _fixture = new GetNotRegisteredStoresFixture();
        }

        [Fact]
        public void GetNotRegisteredStores_WithNoStores_ShouldReturnEmptyResult()
        {
            // Arrange
            var sut = _fixture.CreateSut();

            // Act
            var result = sut.GetNotRegisteredStores(Enumerable.Empty<Store>());

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetNotRegisteredStores_WithNoStoreRegistered_ShouldReturnAllStores()
        {
            // Arrange
            _fixture.SetupStores();
            var sut = _fixture.CreateSut();

            // Act
            var result = sut.GetNotRegisteredStores(_fixture.Stores);

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetNotRegisteredStores_WithOneStoreRegistered_ShouldReturnExpectedStore()
        {
            // Arrange
            _fixture.SetupStores();
            _fixture.SetupExpectedStore(1);
            var sut = _fixture.CreateSut();
            _fixture.AddAvailability(sut.Availabilities, 0);

            // Act
            var result = sut.GetNotRegisteredStores(_fixture.Stores);

            // Assert
            result.Should().HaveCount(1);
            result.First().Should().BeEquivalentTo(_fixture.ExpectedStore);
        }

        private class GetNotRegisteredStoresFixture : AvailableFixture
        {
            public ItemStore ExpectedStore { get; private set; }

            public void SetupExpectedStore(int index)
            {
                var store = Stores.ElementAt(index);
                ExpectedStore = CreateItemStore(store);
            }
        }
    }

    private class AvailableFixture
    {
        public IReadOnlyCollection<Store> Stores { get; private set; } = new List<Store>();

        public IAvailable CreateSut()
        {
            return new AvailableMock();
        }

        public void SetupStores()
        {
            Stores = new List<Store>
            {
                new StoreBuilder()
                    .WithSections(CreateSections())
                    .Create(),
                new StoreBuilder()
                    .WithSections(CreateSections())
                    .Create()
            };
        }

        public void AddAvailability(List<ItemAvailability> availabilities, int index)
        {
            var store = Stores.ElementAt(index);
            availabilities.Add(CreateAvailability(store));
        }

        protected ItemAvailability CreateAvailability(Store store)
        {
            return new ItemAvailability(
                CreateItemStore(store),
                1,
                store.DefaultSection.Id.BackendId);
        }

        protected ItemStore CreateItemStore(Store store)
        {
            return new ItemStore(
                store.Id,
                store.Name,
                store.Sections.Select(s => new ItemSection(s.Id.BackendId, s.Name, s.SortingIndex)));
        }

        private IEnumerable<Section> CreateSections()
        {
            yield return SectionMother.NotDefault().Create();
            yield return SectionMother.Default().Create();
        }
    }

    private class AvailableMock : IAvailable

    {
        public List<ItemAvailability> Availabilities { get; } = new();
    }
}