using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.Services;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Converters.ToDomain;
using ItemCategory = ProjectHermes.Xipona.Api.Repositories.ItemCategories.Entities.ItemCategory;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Converters.ToDomain;

public class ItemCategoryConverterTests : ToDomainConverterTestBase<ItemCategory, IItemCategory>
{
    protected override (ItemCategory, IItemCategory) CreateTestObjects()
    {
        var destination = new ItemCategoryBuilder().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(ServiceCollection);
    }

    public static ItemCategory GetSource(IItemCategory destination)
    {
        return new ItemCategory()
        {
            Id = destination.Id,
            Deleted = destination.IsDeleted,
            Name = destination.Name,
            CreatedAt = destination.CreatedAt
        };
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfGenericType(typeof(ItemCategoryConverter).Assembly, typeof(IToDomainConverter<,>));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IItemCategoryFactory).Assembly, typeof(IItemCategoryFactory));
        serviceCollection.AddTransient<IDateTimeService, DateTimeService>();
    }
}