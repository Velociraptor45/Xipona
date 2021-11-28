using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Converters.ToDomain
{
    public class StoreItemConverter : IToDomainConverter<Item, IStoreItem>
    {
        private readonly IStoreItemFactory _storeItemFactory;
        private readonly IItemTypeFactory _itemTypeFactory;
        private readonly IToDomainConverter<AvailableAt, IStoreItemAvailability> _storeItemAvailabilityConverter;
        private readonly IToDomainConverter<ItemTypeAvailableAt, IStoreItemAvailability> _itemTypeAvailabilityConverter;

        public StoreItemConverter(IStoreItemFactory storeItemFactory,
            IItemTypeFactory itemTypeFactory,
            IToDomainConverter<AvailableAt, IStoreItemAvailability> storeItemAvailabilityConverter,
            IToDomainConverter<ItemTypeAvailableAt, IStoreItemAvailability> itemTypeAvailabilityConverter)
        {
            _storeItemFactory = storeItemFactory;
            _itemTypeFactory = itemTypeFactory;
            _storeItemAvailabilityConverter = storeItemAvailabilityConverter;
            _itemTypeAvailabilityConverter = itemTypeAvailabilityConverter;
        }

        public IStoreItem ToDomain(Item source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            IStoreItem? predecessor = null;
            if (source.PredecessorId != null)
            {
                var converter = new StoreItemConverter(_storeItemFactory, _itemTypeFactory,
                    _storeItemAvailabilityConverter, _itemTypeAvailabilityConverter);
                predecessor = converter.ToDomain(source.Predecessor!);
            }
            var itemCategoryId = source.ItemCategoryId.HasValue ? new ItemCategoryId(source.ItemCategoryId.Value) : null;
            var manufacturerId = source.ManufacturerId.HasValue ? new ManufacturerId(source.ManufacturerId.Value) : null;
            var temporaryId = source.CreatedFrom.HasValue ? new TemporaryItemId(source.CreatedFrom.Value) : null;

            if (source.ItemTypes.Any())
            {
                var itemTypes = new List<IItemType>();
                foreach (var type in source.ItemTypes)
                {
                    var typeAvailabilities = _itemTypeAvailabilityConverter.ToDomain(type.AvailableAt);
                    var domainType = _itemTypeFactory.Create(new ItemTypeId(type.Id), type.Name, typeAvailabilities);
                    itemTypes.Add(domainType);
                }

                return _storeItemFactory.Create(
                    new ItemId(source.Id),
                    source.Name,
                    source.Deleted,
                    source.Comment,
                    source.QuantityType.ToEnum<QuantityType>(),
                    source.QuantityInPacket,
                    source.QuantityTypeInPacket.ToEnum<QuantityTypeInPacket>(),
                    itemCategoryId!, //todo ensure this?
                    manufacturerId,
                    predecessor,
                    itemTypes);
            }

            List<IStoreItemAvailability> availabilities = _storeItemAvailabilityConverter.ToDomain(source.AvailableAt)
                .ToList();

            return _storeItemFactory.Create(
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