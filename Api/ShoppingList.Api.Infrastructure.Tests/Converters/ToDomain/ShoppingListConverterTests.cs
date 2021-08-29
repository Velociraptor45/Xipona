using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using System.Collections.Generic;

using Entities = ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain
{
    public class ShoppingListConverterTests : ToDomainConverterTestBase<Entities.ShoppingList, IShoppingList>
    {
        protected override (Entities.ShoppingList, IShoppingList) CreateTestObjects()
        {
            var destination = ShoppingListMother.ThreeSections().Create();
            var source = GetSource(destination);

            return (source, destination);
        }

        protected override void SetupServiceCollection()
        {
            AddDependencies(serviceCollection);
        }

        public static Entities.ShoppingList GetSource(IShoppingList destination)
        {
            List<ItemsOnList> itemsOnListMap = new List<ItemsOnList>();
            foreach (var section in destination.Sections)
            {
                foreach (var item in section.Items)
                {
                    var map = new ItemsOnList
                    {
                        SectionId = section.Id.Value,
                        ItemId = item.Id.Value,
                        InBasket = item.IsInBasket,
                        Quantity = item.Quantity
                    };
                    itemsOnListMap.Add(map);
                }
            }

            return new Entities.ShoppingList
            {
                Id = destination.Id.Value,
                StoreId = destination.StoreId.Value,
                ItemsOnList = itemsOnListMap,
                CompletionDate = destination.CompletionDate
            };
        }

        public static void AddDependencies(IServiceCollection serviceCollection)
        {
            serviceCollection.AddInstancesOfGenericType(typeof(ShoppingListConverter).Assembly, typeof(IToDomainConverter<,>));
            serviceCollection.AddInstancesOfNonGenericType(typeof(IShoppingListFactory).Assembly, typeof(IShoppingListFactory));
            serviceCollection.AddInstancesOfNonGenericType(typeof(IShoppingListSectionFactory).Assembly, typeof(IShoppingListSectionFactory));
            serviceCollection.AddInstancesOfNonGenericType(typeof(IShoppingListItemFactory).Assembly, typeof(IShoppingListItemFactory));
        }
    }
}