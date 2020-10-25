using ShoppingList.Contracts.Commands.CreateItem;
using ShoppingList.Contracts.Commands.SharedContracts.CreateItem;
using ShoppingList.Domain.Models;
using System.Linq;

namespace ShoppingList.Endpoint.Converters
{
    public static class CreateItemContractConverter
    {
        public static StoreItem ToDomain(this CreateItemContract contract)
        {
            return new StoreItem(new StoreItemId(0),
                contract.Name,
                false,
                contract.Comment,
                contract.IsTemporary,
                (QuantityType)contract.QuantityType,
                contract.QuantityInPacket,
                (QuantityTypeInPacket)contract.QuantityTypeInPacket,
                new ItemCategoryId(contract.ItemCategoryId),
                new ManufacturerId(contract.ManufacturerId),
                contract.Availabilities.Select(av => av.ToDomain()));
        }

        public static StoreItemAvailability ToDomain(this ItemAvailabilityContract contract)
        {
            return new StoreItemAvailability(new StoreId(contract.StoreId), contract.Price);
        }
    }
}