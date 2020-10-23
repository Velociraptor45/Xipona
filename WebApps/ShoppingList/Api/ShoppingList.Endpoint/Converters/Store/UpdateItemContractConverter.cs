using ShoppingList.Contracts.Commands.UpdateItem;
using ShoppingList.Domain.Models;
using System.Linq;

namespace ShoppingList.Endpoint.Converters.Store
{
    public static class UpdateItemContractConverter
    {
        public static StoreItem ToDomain(this UpdateItemContract contract)
        {
            return new StoreItem(new StoreItemId(contract.Id),
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
    }
}