using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.Xipona.Api.Repositories.Stores.Converters.ToEntity;
using Store = ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Store;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Converters.ToEntity;

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
            Id = source.Id,
            Name = source.Name,
            Deleted = source.IsDeleted,
            Sections = source.Sections.Select(s => SectionConverterTests.GetDestination(source.Id, s)).ToList(),
            CreatedAt = source.CreatedAt
        };
    }

    protected override void SetupServiceCollection()
    {
        ServiceCollection.AddImplementationOfGenericType(typeof(StoreConverter).Assembly, typeof(IToEntityConverter<,>));
    }
}