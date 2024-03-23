using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Converters.ToEntity;
using ItemCategory = ProjectHermes.Xipona.Api.Repositories.ItemCategories.Entities.ItemCategory;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Converters.ToEntity;

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