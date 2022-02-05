using System.Linq;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Converters.ToEntity;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Stores.Models;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToEntity;

public class StoreConverterTests : ToEntityConverterTestBase<IStore, ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Store>
{
    protected override (IStore, ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Store) CreateTestObjects()
    {
        var source = StoreMother.Sections(3).Create();
        var destination = GetDestination(source);

        return (source, destination);
    }

    public static ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Store GetDestination(IStore source)
    {
        return new ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Store
        {
            Id = source.Id.Value,
            Name = source.Name,
            Deleted = source.IsDeleted,
            Sections = source.Sections.Select(s => SectionConverterTests.GetDestination(s)).ToList()
        };
    }

    protected override void SetupServiceCollection()
    {
        serviceCollection.AddImplementationOfGenericType(typeof(StoreConverter).Assembly, typeof(IToEntityConverter<,>));
    }
}