using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain
{
    public class ShoppingListConverter : IToDomainConverter<Entities.ShoppingList, IShoppingList>
    {
        private readonly IShoppingListFactory shoppingListFactory;
        private readonly IShoppingListSectionFactory shoppingListSectionFactory;
        private readonly IShoppingListItemFactory shoppingListItemFactory;
        private readonly IToDomainConverter<Store, IShoppingListStore> shoppingListStoreConverter;
        private readonly IToDomainConverter<Entities.Manufacturer, IManufacturer> manufacturerConverter;
        private readonly IToDomainConverter<Entities.ItemCategory, IItemCategory> itemCategoryConverter;

        public ShoppingListConverter(IShoppingListFactory shoppingListFactory,
            IShoppingListSectionFactory shoppingListSectionFactory,
            IShoppingListItemFactory shoppingListItemFactory,
            IToDomainConverter<Entities.Store, IShoppingListStore> shoppingListStoreConverter,
            IToDomainConverter<Entities.Manufacturer, IManufacturer> manufacturerConverter,
            IToDomainConverter<Entities.ItemCategory, IItemCategory> itemCategoryConverter)
        {
            this.shoppingListFactory = shoppingListFactory;
            this.shoppingListSectionFactory = shoppingListSectionFactory;
            this.shoppingListItemFactory = shoppingListItemFactory;
            this.shoppingListStoreConverter = shoppingListStoreConverter;
            this.manufacturerConverter = manufacturerConverter;
            this.itemCategoryConverter = itemCategoryConverter;
        }

        public IShoppingList ToDomain(Entities.ShoppingList source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var store = shoppingListStoreConverter.ToDomain(source.Store);

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
                var items = itemMapsPerSection[section.Id].Select(map => CreateItem(map, source));
                var sectionModel = CreateSection(section, items);
                sectionModels.Add(sectionModel);
            }

            return shoppingListFactory.Create(
                new ShoppingListId(source.Id),
                store,
                sectionModels,
                source.CompletionDate);
        }

        public IShoppingListSection CreateSection(Section section, IEnumerable<IShoppingListItem> items)
        {
            return shoppingListSectionFactory.Create(
                new ShoppingListSectionId(section.Id),
                section.Name,
                items,
                section.SortIndex,
                section.IsDefaultSection);
        }

        public IShoppingListItem CreateItem(ItemsOnList map, Entities.ShoppingList source)
        {
            var itemCategoryEntity = map.Item.ItemCategory;
            var manufacturerEntity = map.Item.Manufacturer;

            IItemCategory itemCategory = null;
            if (itemCategoryEntity != null)
            {
                itemCategory = itemCategoryConverter.ToDomain(itemCategoryEntity);
            }

            IManufacturer manufacturer = null;
            if (manufacturerEntity != null)
            {
                manufacturer = manufacturerConverter.ToDomain(manufacturerEntity);
            }

            return shoppingListItemFactory.Create(
                new ShoppingListItemId(map.Item.Id),
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