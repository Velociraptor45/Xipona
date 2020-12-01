using ShoppingList.Api.Contracts.Commands.ChangeItem;
using ShoppingList.Api.Domain.Commands.ChangeItem;
using ShoppingList.Api.Domain.Models;
using System.Linq;

namespace ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class ChangeItemContractExtensions
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
                contract.ManufacturerId.HasValue ?
                    new ManufacturerId(contract.ManufacturerId.Value) :
                    null,
                contract.Availabilities.Select(av => av.ToDomain()));
        }
    }
}