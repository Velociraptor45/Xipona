using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Fixtures;
using ShoppingList.Api.Domain.TestKit.Shared;
using Entities = ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Entities;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain
{
    public class ManufacturerConverterTests : ToDomainConverterTestBase<Entities.Manufacturer, IManufacturer>
    {
        protected override (Entities.Manufacturer, IManufacturer) CreateTestObjects()
        {
            var commonFixture = new CommonFixture();
            var domainFixture = new ManufacturerFixture(commonFixture);

            var destination = domainFixture.Create();
            var source = GetSource(destination);

            return (source, destination);
        }

        protected override void SetupServiceCollection()
        {
            AddDependencies(serviceCollection);
        }

        public static Entities.Manufacturer GetSource(IManufacturer destination)
        {
            return new Entities.Manufacturer()
            {
                Id = destination.Id.Value,
                Deleted = destination.IsDeleted,
                Name = destination.Name
            };
        }

        public static void AddDependencies(IServiceCollection serviceCollection)
        {
            serviceCollection.AddInstancesOfGenericType(typeof(ManufacturerConverter).Assembly, typeof(IToDomainConverter<,>));
            serviceCollection.AddInstancesOfNonGenericType(typeof(IManufacturerFactory).Assembly, typeof(IManufacturerFactory));
        }
    }
}