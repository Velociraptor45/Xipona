using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
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
        private readonly IToDomainConverter<AvailableAt, IStoreItemAvailability> storeItemAvailabilityConverter;
        private readonly IToDomainConverter<Entities.Manufacturer, IManufacturer> manufacturerConverter;
        private readonly IToDomainConverter<Entities.ItemCategory, IItemCategory> itemCategoryConverter;

        public StoreItemConverter(IStoreItemFactory storeItemFactory,
            IToDomainConverter<AvailableAt, IStoreItemAvailability> storeItemAvailabilityConverter,
            IToDomainConverter<Entities.Manufacturer, IManufacturer> manufacturerConverter,
            IToDomainConverter<Entities.ItemCategory, IItemCategory> itemCategoryConverter)
        {
            this.storeItemFactory = storeItemFactory;
            this.storeItemAvailabilityConverter = storeItemAvailabilityConverter;
            this.manufacturerConverter = manufacturerConverter;
            this.itemCategoryConverter = itemCategoryConverter;
        }

        public IStoreItem ToDomain(Item source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            IItemCategory itemCategory = null;
            if (source.ItemCategoryId != null)
            {
                itemCategory = itemCategoryConverter.ToDomain(source.ItemCategory);
            }

            IManufacturer manufacturer = null;
            if (source.ManufacturerId != null)
            {
                manufacturer = manufacturerConverter.ToDomain(source.Manufacturer);
            }

            IStoreItem predecessor = null;
            if (source.PredecessorId != null)
            {
                var converter = new StoreItemConverter(storeItemFactory, storeItemAvailabilityConverter,
                    manufacturerConverter, itemCategoryConverter);
                predecessor = converter.ToDomain(source.Predecessor);
            }

            List<IStoreItemAvailability> availabilities = storeItemAvailabilityConverter.ToDomain(source.AvailableAt)
                .ToList();

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