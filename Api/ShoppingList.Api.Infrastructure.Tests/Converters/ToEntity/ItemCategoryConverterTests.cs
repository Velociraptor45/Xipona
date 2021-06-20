using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Converters.ToEntity;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Fixtures;
using ShoppingList.Api.Domain.TestKit.Shared;

using Entities = ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToEntity
{
    public class ItemCategoryConverterTests : ToEntityConverterTestBase<IItemCategory, Entities.ItemCategory>
    {
        protected override (IItemCategory, Entities.ItemCategory) CreateTestObjects()
        {
            var commonFixture = new CommonFixture();
            var domainFixture = new ItemCategoryFixture(commonFixture);

            var source = domainFixture.GetItemCategory();
            var destination = ToDomain.ItemCategoryConverterTests.GetSource(source);

            return (source, destination);
        }

        protected override void SetupServiceCollection()
        {
            serviceCollection.AddInstancesOfGenericType(typeof(ItemCategoryConverter).Assembly, typeof(IToEntityConverter<,>));
        }
    }
}