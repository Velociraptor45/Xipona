using ShoppingList.Api.Contracts.Commands.CreateItem;
using ShoppingList.Api.Contracts.Commands.SharedContracts;
using ShoppingList.Api.Domain.Commands.CreateItem;
using ShoppingList.Api.Domain.Models;
using System.Linq;

namespace ShoppingList.Api.Endpoint.Converters
{
    public static class CreateItemContractConverter
    {
        public static ItemCreation ToDomain(this CreateItemContract contract)
        {
            return new ItemCreation(
                contract.Name,
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