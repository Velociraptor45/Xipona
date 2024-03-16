using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Services;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Converters.ToEntity;
using Manufacturer = ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Entities.Manufacturer;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Converters.ToEntity;

public class ManufacturerConverterTests : ToEntityConverterTestBase<IManufacturer, Manufacturer>
{
    protected override (IManufacturer, Manufacturer) CreateTestObjects()
    {
        var source = new ManufacturerBuilder().Create();
        var destination = ToDomain.ManufacturerConverterTests.GetSource(source);

        return (source, destination);
    }

    protected override void SetupServiceCollection()
    {
        ServiceCollection.AddImplementationOfGenericType(typeof(ManufacturerConverter).Assembly, typeof(IToEntityConverter<,>));
        ServiceCollection.AddTransient<IDateTimeService, DateTimeService>();
    }
}