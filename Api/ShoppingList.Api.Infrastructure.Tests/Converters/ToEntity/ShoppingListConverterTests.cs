using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToEntity;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using System.Linq;

using Entities = ProjectHermes.ShoppingList.Api.Infrastructure.Entities;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToEntity
{
    public class ShoppingListConverterTests : ToEntityConverterTestBase<IShoppingList, Entities.ShoppingList>
    {
        protected override (IShoppingList, Entities.ShoppingList) CreateTestObjects()
        {
            var commonFixture = new CommonFixture();
            var shoppingListFixture = new ShoppingListFixture(commonFixture).AsModelFixture();

            var source = shoppingListFixture.CreateValid();
            var destination = GetDestination(source);

            return (source, destination);
        }

        public static Entities.ShoppingList GetDestination(IShoppingList source)
        {
            return new Entities.ShoppingList()
            {
                Id = source.Id.Value,
                CompletionDate = source.CompletionDate,
                StoreId = source.Store.Id.Value,
                ItemsOnList = source.Sections.SelectMany(section =>
                    section.Items.Select(item =>
                        new ItemsOnList()
                        {
                            ShoppingListId = source.Id.Value,
                            ItemId = item.Id.Actual.Value,
                            InBasket = item.IsInBasket,
                            Quantity = item.Quantity,
                            SectionId = section.Id.Value
                        })).ToList()
            };
        }

        protected override void SetupServiceCollection()
        {
            serviceCollection.AddInstancesOfGenericType(typeof(ShoppingListConverter).Assembly, typeof(IToEntityConverter<,>));
        }
    }
}