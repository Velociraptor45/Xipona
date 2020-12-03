using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ShoppingList.Api.Domain.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class CreateItemContractExtensions
    {
        public static ItemCreation ToDomain(this CreateItemContract contract)
        {
            return new ItemCreation(
                contract.Name,
                contract.Comment,
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