using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Converters.ToDomain;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain;

public class ItemCategoryConverterTests : ToDomainConverterTestBase<ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities.ItemCategory, IItemCategory>
{
    protected override (ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities.ItemCategory, IItemCategory) CreateTestObjects()
    {
        var destination = new ItemCategoryBuilder().Create();
        var source = GetSource(destination);

        return (source, destination);
    }

    protected override void SetupServiceCollection()
    {
        AddDependencies(ServiceCollection);
    }

    public static ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities.ItemCategory GetSource(IItemCategory destination)
    {
        return new ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities.ItemCategory()
        {
            Id = destination.Id.Value,
            Deleted = destination.IsDeleted,
            Name = destination.Name
        };
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfGenericType(typeof(ItemCategoryConverter).Assembly, typeof(IToDomainConverter<,>));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IItemCategoryFactory).Assembly, typeof(IItemCategoryFactory));
    }
}