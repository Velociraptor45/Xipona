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

        public StoreItemConverter(IStoreItemFactory storeItemFactory,
            IToDomainConverter<AvailableAt, IStoreItemAvailability> storeItemAvailabilityConverter)
        {
            this.storeItemFactory = storeItemFactory;
            this.storeItemAvailabilityConverter = storeItemAvailabilityConverter;
        }

        public IStoreItem ToDomain(Item source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            IStoreItem predecessor = null;
            if (source.PredecessorId != null)
            {
                var converter = new StoreItemConverter(storeItemFactory, storeItemAvailabilityConverter);
                predecessor = converter.ToDomain(source.Predecessor);
            }

            List<IStoreItemAvailability> availabilities = storeItemAvailabilityConverter.ToDomain(source.AvailableAt)
                .ToList();
            var itemCategoryId = source.ItemCategoryId.HasValue ? new ItemCategoryId(source.ItemCategoryId.Value) : null;
            var manufacturerId = source.ManufacturerId.HasValue ? new ManufacturerId(source.ManufacturerId.Value) : null;
            var temporaryId = source.CreatedFrom.HasValue ? new TemporaryItemId(source.CreatedFrom.Value) : null;

            return storeItemFactory.Create(
                new ItemId(source.Id),
                source.Name,
                source.Deleted,
                source.Comment,
                source.IsTemporary,
                source.QuantityType.ToEnum<QuantityType>(),
                source.QuantityInPacket,
                source.QuantityTypeInPacket.ToEnum<QuantityTypeInPacket>(),
                itemCategoryId,
                manufacturerId,
                predecessor,
                availabilities,
                temporaryId);
        }
    }
}