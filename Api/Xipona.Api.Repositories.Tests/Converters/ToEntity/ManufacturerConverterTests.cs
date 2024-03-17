using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.Services;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Repositories.Manufacturers.Converters.ToEntity;
using Manufacturer = ProjectHermes.Xipona.Api.Repositories.Manufacturers.Entities.Manufacturer;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Converters.ToEntity;

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