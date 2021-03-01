using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Converters
{
    public class ShoppingListConverter : IToDomainConverter<Entities.ShoppingList, IShoppingList>
    {
        private readonly IShoppingListFactory shoppingListFactory;
        private readonly IShoppingListSectionFactory shoppingListSectionFactory;
        private readonly IShoppingListStoreFactory shoppingListStoreFactory;
        private readonly IShoppingListItemFactory shoppingListItemFactory;
        private readonly IItemCategoryFactory itemCategoryFactory;
        private readonly IManufacturerFactory manufacturerFactory;

        public ShoppingListConverter(IShoppingListFactory shoppingListFactory,
            IShoppingListSectionFactory shoppingListSectionFactory,
            IShoppingListStoreFactory shoppingListStoreFactory,
            IShoppingListItemFactory shoppingListItemFactory,
            IItemCategoryFactory itemCategoryFactory,
            IManufacturerFactory manufacturerFactory)
        {
            this.shoppingListFactory = shoppingListFactory;
            this.shoppingListSectionFactory = shoppingListSectionFactory;
            this.shoppingListStoreFactory = shoppingListStoreFactory;
            this.shoppingListItemFactory = shoppingListItemFactory;
            this.itemCategoryFactory = itemCategoryFactory;
            this.manufacturerFactory = manufacturerFactory;
        }

        public IShoppingList ToDomain(Entities.ShoppingList source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var store = shoppingListStoreFactory.Create(
                new ShoppingListStoreId(source.Store.Id),
                source.Store.Name,
                source.Store.Deleted);

            var itemMapsPerSection = source.ItemsOnList.GroupBy(
                map => map.SectionId.Value,
                map => map,
                (sectionId, maps) => new
                {
                    SectionId = sectionId,
                    Maps = maps
                })
                .ToDictionary(t => t.SectionId, t => t.Maps);

            List<IShoppingListSection> sectionModels = new List<IShoppingListSection>();
            foreach (var section in source.Store.Sections)
            {
                var isDefault = source.Store.DefaultSectionId == section.Id;
                var items = itemMapsPerSection[section.Id].Select(map => CreateItem(map, source));
                var sectionModel = CreateSection(section, isDefault, items);
                sectionModels.Add(sectionModel);
            }

            return shoppingListFactory.Create(
                store,
                sectionModels,
                source.CompletionDate);
        }

        public IShoppingListSection CreateSection(Section section, bool isDefaultSection,
            IEnumerable<IShoppingListItem> items)
        {
            return shoppingListSectionFactory.Create(
                new ShoppingListSectionId(section.Id),
                section.Name,
                items,
                section.SortIndex,
                isDefaultSection);
        }

        public IShoppingListItem CreateItem(ItemsOnList map, Entities.ShoppingList source)
        {
            var itemCategoryEntity = map.Item.ItemCategory;
            var manufacturerEntity = map.Item.Manufacturer;

            IItemCategory itemCategory = null;
            if (itemCategoryEntity != null)
            {
                itemCategory = itemCategoryFactory.Create(
                    new ItemCategoryId(itemCategoryEntity.Id),
                    itemCategoryEntity.Name,
                    itemCategoryEntity.Deleted);
            }

            IManufacturer manufacturer = null;
            if (manufacturerEntity != null)
            {
                manufacturer = manufacturerFactory.Create(
                    new ManufacturerId(manufacturerEntity.Id),
                    manufacturerEntity.Name,
                    manufacturerEntity.Deleted);
            }

            return shoppingListItemFactory.Create(
                new ShoppingListItemId(source.Id),
                map.Item.Name,
                map.Item.Deleted,
                map.Item.Comment,
                map.Item.IsTemporary,
                map.Item.AvailableAt.Single(av => av.StoreId == source.Store.Id).Price,
                map.Item.QuantityType.ToEnum<QuantityType>(),
                map.Item.QuantityInPacket,
                map.Item.QuantityTypeInPacket.ToEnum<QuantityTypeInPacket>(),
                itemCategory,
                manufacturer,
                map.InBasket,
                map.Quantity);
        }
    }
}