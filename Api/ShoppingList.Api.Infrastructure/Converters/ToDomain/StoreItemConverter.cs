using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain
{
    public class StoreItemConverter : IToDomainConverter<Item, IStoreItem>
    {
        private readonly IStoreItemFactory storeItemFactory;
        private readonly IItemCategoryFactory itemCategoryFactory;
        private readonly IManufacturerFactory manufacturerFactory;
        private readonly IStoreItemAvailabilityFactory storeItemAvailabilityFactory;
        private readonly IStoreItemStoreFactory storeItemStoreFactory;
        private readonly IStoreItemSectionFactory storeItemSectionFactory;

        public StoreItemConverter(IStoreItemFactory storeItemFactory,
            IItemCategoryFactory itemCategoryFactory,
            IManufacturerFactory manufacturerFactory,
            IStoreItemAvailabilityFactory storeItemAvailabilityFactory,
            IStoreItemStoreFactory storeItemStoreFactory,
            IStoreItemSectionFactory storeItemSectionFactory)
        {
            this.storeItemFactory = storeItemFactory;
            this.itemCategoryFactory = itemCategoryFactory;
            this.manufacturerFactory = manufacturerFactory;
            this.storeItemAvailabilityFactory = storeItemAvailabilityFactory;
            this.storeItemStoreFactory = storeItemStoreFactory;
            this.storeItemSectionFactory = storeItemSectionFactory;
        }

        public IStoreItem ToDomain(Item source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            IItemCategory itemCategory = null;
            if (source.ItemCategoryId != null)
            {
                var itemCategoryEntity = source.ItemCategory;
                itemCategory = itemCategoryFactory.Create(
                    new ItemCategoryId(itemCategoryEntity.Id),
                    itemCategoryEntity.Name,
                    itemCategoryEntity.Deleted);
            }

            IManufacturer manufacturer = null;
            if (source.ManufacturerId != null)
            {
                var manufacturerEntity = source.Manufacturer;
                manufacturer = manufacturerFactory.Create(
                    new ManufacturerId(manufacturerEntity.Id),
                    manufacturerEntity.Name,
                    manufacturerEntity.Deleted);
            }

            IStoreItem predecessor = null;
            if (source.PredecessorId != null)
            {
                var converter = new StoreItemConverter(storeItemFactory, itemCategoryFactory, manufacturerFactory,
                    storeItemAvailabilityFactory, storeItemStoreFactory, storeItemSectionFactory);
                predecessor = converter.ToDomain(source.Predecessor);
            }

            List<IStoreItemAvailability> availabilities = new List<IStoreItemAvailability>();
            foreach (var availabilityEntity in source.AvailableAt)
            {
                var sectionEntities = availabilityEntity.Store.Sections.ToList();

                List<IStoreItemSection> sections = sectionEntities
                    .Select(s => storeItemSectionFactory.Create(new StoreItemSectionId(s.Id), s.Name, s.SortIndex))
                    .ToList();

                IStoreItemStore store = storeItemStoreFactory.Create(
                    new StoreItemStoreId(availabilityEntity.StoreId),
                    availabilityEntity.Store.Name,
                    sections);

                var availabilityModel = storeItemAvailabilityFactory.Create(
                    store,
                    availabilityEntity.Price,
                    sections.Single(s => s.Id.Value == availabilityEntity.Store.DefaultSectionId));
                availabilities.Add(availabilityModel);
            }

            return storeItemFactory.Create(
                new StoreItemId(source.Id),
                source.Name,
                source.Deleted,
                source.Comment,
                source.IsTemporary,
                source.QuantityType.ToEnum<QuantityType>(),
                source.QuantityInPacket,
                source.QuantityTypeInPacket.ToEnum<QuantityTypeInPacket>(),
                itemCategory,
                manufacturer,
                predecessor,
                availabilities);
        }
    }
}