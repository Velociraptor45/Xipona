using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Converters.ToDomain;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Models;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain;

public class ManufacturerConverterTests : ToDomainConverterTestBase<ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Entities.Manufacturer, IManufacturer>
{
    protected override (ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Entities.Manufacturer, IManufacturer) CreateTestObjects()
    {
        var destination = new ManufacturerBuilder().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(ServiceCollection);
    }

    public static ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Entities.Manufacturer GetSource(IManufacturer destination)
    {
        return new ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Entities.Manufacturer()
        {
            Id = destination.Id.Value,
            Deleted = destination.IsDeleted,
            Name = destination.Name.Value
        };
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfGenericType(typeof(ManufacturerConverter).Assembly, typeof(IToDomainConverter<,>));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IManufacturerFactory).Assembly, typeof(IManufacturerFactory));
    }
}