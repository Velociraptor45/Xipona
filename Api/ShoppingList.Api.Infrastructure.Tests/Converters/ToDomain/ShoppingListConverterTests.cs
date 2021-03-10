using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ShoppingList.Api.Core.TestKit.Converter;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using System.Collections.Generic;
using System.Linq;

using Entities = ProjectHermes.ShoppingList.Api.Infrastructure.Entities;

namespace ShoppingList.Api.Infrastructure.Tests.Converters.ToDomain
{
    public class ShoppingListConverterTests : ToDomainConverterTestBase<Entities.ShoppingList, IShoppingList>
    {
        protected override (Entities.ShoppingList, IShoppingList) CreateTestObjects()
        {
            var commonFixture = new CommonFixture();
            var shoppingListFixture = new ShoppingListFixture(commonFixture).AsModelFixture();

            var destination = shoppingListFixture.CreateValid();
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
            List<Section> sectionEntities = new List<Section>();
            foreach (var section in destination.Sections)
            {
                var sectionEntity = new Section
                {
                    Id = section.Id.Value,
                    Name = section.Name,
                    SortIndex = section.SortingIndex
                };

                foreach (var item in section.ShoppingListItems)
                {
                    var manufacturer = item.Manufacturer == null ? null : ManufacturerConverterTests.GetSource(item.Manufacturer);
                    var itemCategory = item.ItemCategory == null ? null : ItemCategoryConverterTests.GetSource(item.ItemCategory);

                    var itemEntity = new Item
                    {
                        Id = item.Id.Actual.Value,
                        Name = item.Name,
                        Deleted = item.IsDeleted,
                        Comment = item.Comment,
                        IsTemporary = item.IsTemporary,
                        QuantityType = item.QuantityType.ToInt(),
                        QuantityInPacket = item.QuantityInPacket,
                        QuantityTypeInPacket = item.QuantityTypeInPacket.ToInt(),
                        ItemCategoryId = itemCategory?.Id,
                        ItemCategory = itemCategory,
                        ManufacturerId = manufacturer?.Id,
                        Manufacturer = manufacturer,
                        AvailableAt = new AvailableAt
                        {
                            StoreId = destination.Store.Id.Value,
                            Price = item.PricePerQuantity
                        }.ToMonoList()
                    };

                    var map = new ItemsOnList
                    {
                        SectionId = section.Id.Value,
                        ItemId = item.Id.Actual.Value,
                        Item = itemEntity,
                        InBasket = item.IsInBasket,
                        Quantity = item.Quantity
                    };
                    itemsOnListMap.Add(map);
                }
                sectionEntities.Add(sectionEntity);
            }

            var store = new Store
            {
                Id = destination.Store.Id.Value,
                Name = destination.Store.Name,
                Deleted = destination.Store.IsDeleted,
                Sections = sectionEntities,
                DefaultSectionId = destination.Sections.Single(s => s.IsDefaultSection).Id.Value
            };

            return new Entities.ShoppingList
            {
                Id = destination.Id.Value,
                Store = store,
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

            ManufacturerConverterTests.AddDependencies(serviceCollection);
            ItemCategoryConverterTests.AddDependencies(serviceCollection);
            ShoppingListStoreConverterTests.AddDependencies(serviceCollection);
        }
    }
}