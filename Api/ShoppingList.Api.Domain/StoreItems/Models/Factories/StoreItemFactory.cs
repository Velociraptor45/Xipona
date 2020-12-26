using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public class StoreItemFactory : IStoreItemFactory
    {
        public IStoreItem Create(ItemCreation itemCreation, IItemCategory itemCategory,
            IManufacturer manufacturer)
        {
            return new StoreItem(new StoreItemId(0),
                itemCreation.Name,
                false,
                itemCreation.Comment,
                false,
                itemCreation.QuantityType,
                itemCreation.QuantityInPacket,
                itemCreation.QuantityTypeInPacket,
                itemCategory,
                manufacturer,
                itemCreation.Availabilities);
        }

        public IStoreItem Create(TemporaryItemCreation model)
        {
            return new StoreItem(
                new StoreItemId(model.ClientSideId),
                model.Name,
                false,
                string.Empty,
                true,
                QuantityType.Unit,
                1,
                QuantityTypeInPacket.Unit,
                null,
                null,
                new List<StoreItemAvailability>() { model.Availability });
        }
    }
}