using ShoppingList.Api.Contracts.Commands.UpdateItem;
using ShoppingList.Api.Domain.Commands.UpdateItem;
using ShoppingList.Api.Domain.Models;
using System.Linq;

namespace ShoppingList.Api.Endpoint.Converters.Store
{
    public static class UpdateItemContractConverter
    {
        public static ItemUpdate ToDomain(this UpdateItemContract contract)
        {
            return new ItemUpdate(new StoreItemId(contract.Id),
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