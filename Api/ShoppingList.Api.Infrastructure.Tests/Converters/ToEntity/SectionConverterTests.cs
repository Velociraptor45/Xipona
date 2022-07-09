using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Converters.ToEntity;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Stores.Models;
using Section = ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Section;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToEntity;

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
            IsDefaultSection = source.IsDefaultSection
        };
    }

    protected override void SetupServiceCollection()
    {
        ServiceCollection.AddImplementationOfGenericType(typeof(SectionConverter).Assembly, typeof(IToEntityConverter<,>));
    }
}