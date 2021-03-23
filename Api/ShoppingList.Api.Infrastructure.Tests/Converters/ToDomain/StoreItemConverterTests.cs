using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using System.Linq;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain
{
    public class StoreItemConverterTests : ToDomainConverterTestBase<Item, IStoreItem>
    {
        protected override (Item, IStoreItem) CreateTestObjects()
        {
            var commonFixture = new CommonFixture();
            var availabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            var storeItemFixture = new StoreItemFixture(availabilityFixture, commonFixture);

            var destination = storeItemFixture.CreateValid();
            var source = GetSource(destination, commonFixture);

            return (source, destination);
        }

        protected override void SetupServiceCollection()
        {
            AddDependencies(serviceCollection);
        }

        public static Item GetSource(IStoreItem destination, CommonFixture commonFixture)
        {
            var itemCategory = ItemCategoryConverterTests.GetSource(destination.ItemCategoryId);
            var manufacturer = ManufacturerConverterTests.GetSource(destination.ManufacturerId);
            Item predecessor = null;
            if (destination.Predecessor != null)
                predecessor = GetSource(destination.Predecessor, commonFixture);

            var availabilities = destination.Availabilities
                .Select(av => StoreItemAvailabilityConverterTests.GetSource(av, commonFixture))
                .ToList();

            return new Item
            {
                Id = destination.Id.Actual.Value,
                Name = destination.Name,
                Deleted = destination.IsDeleted,
                Comment = destination.Comment,
                IsTemporary = destination.IsTemporary,
                QuantityType = destination.QuantityType.ToInt(),
                QuantityInPacket = destination.QuantityInPacket,
                QuantityTypeInPacket = destination.QuantityTypeInPacket.ToInt(),
                ItemCategoryId = itemCategory.Id,
                ItemCategory = itemCategory,
                ManufacturerId = manufacturer.Id,
                Manufacturer = manufacturer,
                PredecessorId = predecessor?.Id,
                Predecessor = predecessor,
                AvailableAt = availabilities
            };
        }

        public static void AddDependencies(IServiceCollection serviceCollection)
        {
            serviceCollection.AddInstancesOfGenericType(typeof(StoreItemConverter).Assembly, typeof(IToDomainConverter<,>));
            serviceCollection.AddInstancesOfNonGenericType(typeof(IStoreItemFactory).Assembly, typeof(IStoreItemFactory));

            StoreItemAvailabilityConverterTests.AddDependencies(serviceCollection);
            ManufacturerConverterTests.AddDependencies(serviceCollection);
            ItemCategoryConverterTests.AddDependencies(serviceCollection);
        }
    }
}