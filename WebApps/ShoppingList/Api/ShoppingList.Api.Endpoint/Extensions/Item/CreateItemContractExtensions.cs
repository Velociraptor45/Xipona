using ShoppingList.Api.Contracts.Commands.CreateItem;
using ShoppingList.Api.Domain.Commands.CreateItem;
using ShoppingList.Api.Domain.Models;
using System.Linq;

namespace ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class CreateItemContractExtensions
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
    }
}