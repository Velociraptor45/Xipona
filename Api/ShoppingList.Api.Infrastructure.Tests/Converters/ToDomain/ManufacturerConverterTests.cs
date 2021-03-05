using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Fixtures;
using ShoppingList.Api.Domain.TestKit.Shared;
using Entities = ProjectHermes.ShoppingList.Api.Infrastructure.Entities;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain
{
    public class ManufacturerConverterTests : ToDomainConverterTestBase<Entities.Manufacturer, IManufacturer>
    {
        protected override (Entities.Manufacturer, IManufacturer) CreateTestObjects()
        {
            var commonFixture = new CommonFixture();
            var domainFixture = new ManufacturerFixture(commonFixture);

            var destination = domainFixture.Create();
            var source = new Entities.Manufacturer()
            {
                Id = destination.Id.Value,
                Deleted = destination.IsDeleted,
                Name = destination.Name
            };

            return (source, destination);
        }

        protected override void SetupServiceCollection()
        {
            serviceCollection.AddInstancesOfGenericType(typeof(ManufacturerConverter).Assembly, typeof(IToDomainConverter<,>));
            serviceCollection.AddInstancesOfNonGenericType(typeof(IManufacturerFactory).Assembly, typeof(IManufacturerFactory));
        }
    }
}