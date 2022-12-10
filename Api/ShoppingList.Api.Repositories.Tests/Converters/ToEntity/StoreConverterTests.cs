using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Converters.ToEntity;
using Store = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Store;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Converters.ToEntity;

public class StoreConverterTests : ToEntityConverterTestBase<IStore, Store>
{
    protected override (IStore, Store) CreateTestObjects()
    {
        var source = StoreMother.Sections(3).Create();
        var destination = GetDestination(source);

        return (source, destination);
    }

    public static Store GetDestination(IStore source)
    {
        return new Store
        {
            Id = source.Id.Value,
            Name = source.Name,
            Deleted = source.IsDeleted,
            Sections = source.Sections.Select(s => SectionConverterTests.GetDestination(s)).ToList()
        };
    }

    protected override void SetupServiceCollection()
    {
        ServiceCollection.AddImplementationOfGenericType(typeof(StoreConverter).Assembly, typeof(IToEntityConverter<,>));
    }
}