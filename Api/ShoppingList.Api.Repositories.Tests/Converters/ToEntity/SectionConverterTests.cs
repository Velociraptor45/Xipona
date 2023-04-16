using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Converters.ToEntity;
using Section = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Converters.ToEntity;

public class SectionConverterTests : ToEntityConverterTestBase<(StoreId, ISection), Section>
{
    protected override ((StoreId, ISection), Section) CreateTestObjects()
    {
        var source = SectionMother.Default().Create();
        var storeId = StoreId.New;
        var destination = GetDestination(storeId, source);

        return ((storeId, source), destination);
    }

    public static Section GetDestination(StoreId storeId, ISection source)
    {
        return new Section
        {
            Id = source.Id,
            Name = source.Name,
            SortIndex = source.SortingIndex,
            IsDefaultSection = source.IsDefaultSection,
            IsDeleted = source.IsDeleted,
            StoreId = storeId.Value
        };
    }

    protected override void SetupServiceCollection()
    {
        ServiceCollection.AddImplementationOfGenericType(typeof(SectionConverter).Assembly, typeof(IToEntityConverter<,>));
    }
}