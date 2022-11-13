using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Converters.ToDomain;
using ItemCategory = ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Entities.ItemCategory;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Converters.ToDomain;

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
            Id = destination.Id.Value,
            Deleted = destination.IsDeleted,
            Name = destination.Name.Value
        };
    }

    public static void AddDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddImplementationOfGenericType(typeof(ItemCategoryConverter).Assembly, typeof(IToDomainConverter<,>));
        serviceCollection.AddImplementationOfNonGenericType(typeof(IItemCategoryFactory).Assembly, typeof(IItemCategoryFactory));
    }
}