using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Converters.ToEntity;
using ItemCategory = ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Entities.ItemCategory;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Converters.ToEntity;

public class ItemCategoryConverterTests : ToEntityConverterTestBase<IItemCategory, ItemCategory>
{
    protected override (IItemCategory, ItemCategory) CreateTestObjects()
    {
        var source = new ItemCategoryBuilder().Create();
        var destination = ToDomain.ItemCategoryConverterTests.GetSource(source);

        return (source, destination);
    }

    protected override void SetupServiceCollection()
    {
        ServiceCollection.AddImplementationOfGenericType(typeof(ItemCategoryConverter).Assembly, typeof(IToEntityConverter<,>));
    }
}