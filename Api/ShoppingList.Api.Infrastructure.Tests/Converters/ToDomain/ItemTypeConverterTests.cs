using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Converters.ToDomain;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using System.Linq;
using Entities = ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain
{
    public class ItemTypeConverterTests : ToDomainConverterTestBase<Entities.ItemType, IItemType>
    {
        protected override (Entities.ItemType, IItemType) CreateTestObjects()
        {
            var destination = new ItemTypeBuilder().Create();
            var source = GetSource(destination);

            return (source, destination);
        }

        public static Entities.ItemType GetSource(IItemType destination)
        {
            var availabilities = destination.Availabilities
                .Select(av => ItemTypeAvailabilityConverterTests.GetSource(av))
                .ToList();

            return new Entities.ItemType
            {
                Id = destination.Id.Value,
                Name = destination.Name,
                AvailableAt = availabilities
            };
        }

        protected override void SetupServiceCollection()
        {
            AddDependencies(serviceCollection);
        }

        public static void AddDependencies(IServiceCollection serviceCollection)
        {
            serviceCollection.AddImplementationOfGenericType(typeof(ItemTypeConverter).Assembly, typeof(IToDomainConverter<,>));
            serviceCollection.AddImplementationOfNonGenericType(typeof(IItemTypeFactory).Assembly, typeof(IItemTypeFactory));

            ItemTypeAvailabilityConverterTests.AddDependencies(serviceCollection);
        }
    }
}