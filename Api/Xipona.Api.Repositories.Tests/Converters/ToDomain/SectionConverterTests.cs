using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.Xipona.Api.Repositories.Stores.Converters.ToDomain;
using Section = ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Converters.ToDomain;

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
            Id = destination.Id,
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