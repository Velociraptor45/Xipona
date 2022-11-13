using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.TestKit.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Converters.ToEntity;
using Section = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Converters.ToEntity;

public class SectionConverterTests : ToEntityConverterTestBase<ISection, Section>
{
    protected override (ISection, Section) CreateTestObjects()
    {
        var source = SectionMother.Default().Create();
        var destination = GetDestination(source);

        return (source, destination);
    }

    public static Section GetDestination(ISection source)
    {
        return new Section
        {
            Id = source.Id.Value,
            Name = source.Name.Value,
            SortIndex = source.SortingIndex,
            IsDefaultSection = source.IsDefaultSection,
            IsDeleted = source.IsDeleted,
        };
    }

    protected override void SetupServiceCollection()
    {
        ServiceCollection.AddImplementationOfGenericType(typeof(SectionConverter).Assembly, typeof(IToEntityConverter<,>));
    }
}