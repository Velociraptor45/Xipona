using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Converters.ToDomain;
using Section = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Converters.ToDomain;

public class SectionConverterTests : ToDomainConverterTestBase<Section, ISection>
{
    protected override (Section, ISection) CreateTestObjects()
    {
        var destination = SectionMother.Default().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(ServiceCollection);
    }

    public static Section GetSource(ISection destination)
    {
        return new Section()
        {
            Id = destination.Id.Value,
            Name = destination.Name,
            SortIndex = destination.SortingIndex,
            IsDefaultSection = destination.IsDefaultSection,
            IsDeleted = destination.IsDeleted
        };
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfGenericType(typeof(SectionConverter).Assembly, typeof(IToDomainConverter<,>));
        serviceCollection.AddImplementationOfNonGenericType(typeof(ISectionFactory).Assembly, typeof(ISectionFactory));
    }
}