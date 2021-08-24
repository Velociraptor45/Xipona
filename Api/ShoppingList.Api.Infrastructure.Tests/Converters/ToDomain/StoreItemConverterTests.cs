using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using System.Linq;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain
{
    public class StoreItemConverterTests : ToDomainConverterTestBase<Item, IStoreItem>
    {
        protected override (Item, IStoreItem) CreateTestObjects()
        {
            var destination = StoreItemMother.Initial().Create();
            var source = GetSource(destination);

            return (source, destination);
        }

        protected override void SetupServiceCollection()
        {
            AddDependencies(serviceCollection);
        }

        public static Item GetSource(IStoreItem destination)
        {
            Item predecessor = null;
            if (destination.Predecessor != null)
                predecessor = GetSource(destination.Predecessor);

            var availabilities = destination.Availabilities
                .Select(av => StoreItemAvailabilityConverterTests.GetSource(av))
                .ToList();

            return new Item
            {
                Id = destination.Id.Value,
                Name = destination.Name,
                Deleted = destination.IsDeleted,
                Comment = destination.Comment,
                IsTemporary = destination.IsTemporary,
                QuantityType = destination.QuantityType.ToInt(),
                QuantityInPacket = destination.QuantityInPacket,
                QuantityTypeInPacket = destination.QuantityTypeInPacket.ToInt(),
                ItemCategoryId = destination.ItemCategoryId?.Value,
                ManufacturerId = destination.ManufacturerId?.Value,
                PredecessorId = predecessor?.Id,
                Predecessor = predecessor,
                AvailableAt = availabilities,
                CreatedFrom = destination.TemporaryId?.Value
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