using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.TestKit.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Converters.ToDomain;
using Store = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Store;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Converters.ToDomain;

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
            .Select(s => SectionConverterTests.GetSource(s))
            .ToList();

        return new Store
        {
            Id = destination.Id.Value,
            Name = destination.Name.Value,
            Deleted = destination.IsDeleted,
            Sections = sections
        };
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfGenericType(typeof(StoreConverter).Assembly, typeof(IToDomainConverter<,>));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IStoreFactory).Assembly, typeof(IStoreFactory));

        SectionConverterTests.AddDependencies(serviceCollection);
    }
}