using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Converters.ToEntity;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Models;
using Entities = ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Entities;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToEntity
{
    public class ManufacturerConverterTests : ToEntityConverterTestBase<IManufacturer, Entities.Manufacturer>
    {
        protected override (IManufacturer, Entities.Manufacturer) CreateTestObjects()
        {
            var source = new ManufacturerBuilder().Create();
            var destination = ToDomain.ManufacturerConverterTests.GetSource(source);

            return (source, destination);
        }

        protected override void SetupServiceCollection()
        {
            serviceCollection.AddInstancesOfGenericType(typeof(ManufacturerConverter).Assembly, typeof(IToEntityConverter<,>));
        }
    }
}