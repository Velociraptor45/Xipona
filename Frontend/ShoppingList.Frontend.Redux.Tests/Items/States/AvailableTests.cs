using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Items.States;

public class AvailableTests
{
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
            var sut = AvailableFixture.CreateSut();

            // Act
            var result = sut.GetNotRegisteredStores(Enumerable.Empty<ItemStore>());

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetNotRegisteredStores_WithNoStoreRegistered_ShouldReturnAllStores()
        {
            // Arrange
            _fixture.SetupStores();
            var sut = AvailableFixture.CreateSut();

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
            var sut = AvailableFixture.CreateSut();
            sut = _fixture.AddAvailability(sut as AvailableDummy, 0);

            // Act
            var result = sut.GetNotRegisteredStores(_fixture.Stores).ToList();

            // Assert
            result.Should().HaveCount(1);
            result.First().Should().BeEquivalentTo(_fixture.ExpectedStore);
        }

        private class GetNotRegisteredStoresFixture : AvailableFixture
        {
            public ItemStore? ExpectedStore { get; private set; }

            public void SetupExpectedStore(int index)
            {
                var store = Stores.ElementAt(index);
                ExpectedStore = CreateItemStore(store);
            }
        }
    }

    private class AvailableFixture
    {
        public IReadOnlyCollection<ItemStore> Stores { get; private set; } = new List<ItemStore>();

        public static IAvailable CreateSut()
        {
            return new AvailableDummy(new List<EditedItemAvailability>());
        }

        public void SetupStores()
        {
            Stores = new List<ItemStore>
            {
                ItemStoreMother.GetValid(),
                ItemStoreMother.GetValid()
            };
        }

        public IAvailable AddAvailability(AvailableDummy available, int index)
        {
            var availabilities = available.Availabilities.ToList();
            var store = Stores.ElementAt(index);
            availabilities.Add(CreateAvailability(store));
            return available with { Availabilities = availabilities };
        }

        private static EditedItemAvailability CreateAvailability(ItemStore store)
        {
            return new EditedItemAvailability(
                store.Id,
                store.DefaultSectionId,
                1);
        }

        protected static ItemStore CreateItemStore(ItemStore store)
        {
            return store with
            {
                Sections = store.Sections
                    .Select(s => new ItemStoreSection(s.Id, s.Name, s.IsDefaultSection, s.SortingIndex))
                    .ToList()
            };
        }
    }

    private record AvailableDummy(IReadOnlyCollection<EditedItemAvailability> Availabilities) : IAvailable;
}