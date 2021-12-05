using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModification;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems
{
    public class ModifyItemWithTypesConverter : IToDomainConverter<ModifyItemWithTypesContract, ItemWithTypesModification>
    {
        private readonly IToDomainConverter<ItemTypeContract, IItemType> _itemTypeConverter;

        public ModifyItemWithTypesConverter(IToDomainConverter<ItemTypeContract, IItemType> itemTypeConverter)
        {
            _itemTypeConverter = itemTypeConverter;
        }

        public ItemWithTypesModification ToDomain(ModifyItemWithTypesContract source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new ItemWithTypesModification(
                new ItemId(source.Id),
                source.Name,
                source.Comment,
                source.QuantityType.ToEnum<QuantityType>(),
                source.QuantityInPacket,
                source.QuantityTypeInPacket.ToEnum<QuantityTypeInPacket>(),
                new ItemCategoryId(source.ItemCategoryId),
                source.ManufacturerId.HasValue ?
                    new ManufacturerId(source.ManufacturerId.Value) :
                    null,
                _itemTypeConverter.ToDomain(source.ItemTypes));
        }
    }
}