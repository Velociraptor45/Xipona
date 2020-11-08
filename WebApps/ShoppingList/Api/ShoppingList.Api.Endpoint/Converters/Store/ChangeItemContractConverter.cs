using ShoppingList.Api.Contracts.Commands.ChangeItem;
using ShoppingList.Api.Domain.Commands.ChangeItem;
using ShoppingList.Api.Domain.Models;
using System.Linq;

namespace ShoppingList.Api.Endpoint.Converters.Store
{
    public static class ChangeItemContractConverter
    {
        public static ItemChange ToDomain(this ChangeItemContract contract)
        {
            return new ItemChange(new StoreItemId(contract.Id),
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