using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.Services;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Repositories.Manufacturers.Converters.ToDomain;
using Manufacturer = ProjectHermes.Xipona.Api.Repositories.Manufacturers.Entities.Manufacturer;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Converters.ToDomain;

public class ManufacturerConverterTests : ToDomainConverterTestBase<Manufacturer, IManufacturer>
{
    protected override (Manufacturer, IManufacturer) CreateTestObjects()
    {
        var destination = new ManufacturerBuilder().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(ServiceCollection);
    }

    public static Manufacturer GetSource(IManufacturer destination)
    {
        return new Manufacturer()
        {
            Id = destination.Id,
            Deleted = destination.IsDeleted,
            Name = destination.Name,
            CreatedAt = destination.CreatedAt
        };
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfGenericType(typeof(ManufacturerConverter).Assembly, typeof(IToDomainConverter<,>));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IManufacturerFactory).Assembly, typeof(IManufacturerFactory));
        serviceCollection.AddTransient<IDateTimeService, DateTimeService>();
    }
}