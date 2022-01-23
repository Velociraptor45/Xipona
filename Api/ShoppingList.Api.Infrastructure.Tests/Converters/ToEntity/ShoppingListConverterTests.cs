using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Converters.ToEntity;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using System.Linq;

using Entities = ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToEntity
{
    public class ShoppingListConverterTests : ToEntityConverterTestBase<IShoppingList, Entities.ShoppingList>
    {
        protected override (IShoppingList, Entities.ShoppingList) CreateTestObjects()
        {
            var source = ShoppingListMother.ThreeSections().Create();
            var destination = GetDestination(source);

            return (source, destination);
        }

        public static Entities.ShoppingList GetDestination(IShoppingList source)
        {
            return new Entities.ShoppingList()
            {
                Id = source.Id.Value,
                CompletionDate = source.CompletionDate,
                StoreId = source.StoreId.Value,
                ItemsOnList = source.Sections.SelectMany(section =>
                    section.Items.Select(item =>
                        new Entities.ItemsOnList()
                        {
                            ShoppingListId = source.Id.Value,
                            ItemId = item.Id.Value,
                            ItemTypeId = item.TypeId?.Value,
                            InBasket = item.IsInBasket,
                            Quantity = item.Quantity,
                            SectionId = section.Id.Value
                        })).ToList()
            };
        }

        protected override void SetupServiceCollection()
        {
            serviceCollection.AddImplementationOfGenericType(typeof(ShoppingListConverter).Assembly, typeof(IToEntityConverter<,>));
        }
    }
}