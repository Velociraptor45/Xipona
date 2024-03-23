using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.Services;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.Xipona.Api.Repositories.Stores.Converters.ToDomain;
using Store = ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Store;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Converters.ToDomain;

public class StoreConverterTests : ToDomainConverterTestBase<Store, IStore>
{
    protected override (Store, IStore) CreateTestObjects()
    {
        var destination = StoreMother.Sections(3).Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(ServiceCollection);
    }

    public static Store GetSource(IStore destination)
    {
        var sections = destination.Sections
            .Select(SectionConverterTests.GetSource)
            .ToList();

        return new Store
        {
            Id = destination.Id,
            Name = destination.Name,
            Deleted = destination.IsDeleted,
            Sections = sections,
            CreatedAt = destination.CreatedAt
        };
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfGenericType(typeof(StoreConverter).Assembly, typeof(IToDomainConverter<,>));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IStoreFactory).Assembly, typeof(IStoreFactory));
        serviceCollection.AddTransient<IDateTimeService, DateTimeService>();

        SectionConverterTests.AddDependencies(serviceCollection);
    }
}