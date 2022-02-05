using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Converters.ToEntity;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToEntity;

public class ItemCategoryConverterTests : ToEntityConverterTestBase<IItemCategory, ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities.ItemCategory>
{
    protected override (IItemCategory, ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities.ItemCategory) CreateTestObjects()
    {
        var source = new ItemCategoryBuilder().Create();
        var destination = ToDomain.ItemCategoryConverterTests.GetSource(source);

        return (source, destination);
    }

    protected override void SetupServiceCollection()
    {
        serviceCollection.AddImplementationOfGenericType(typeof(ItemCategoryConverter).Assembly, typeof(IToEntityConverter<,>));
    }
}