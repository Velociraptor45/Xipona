using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class UpdateItemContractExtensions
    {
        public static ItemUpdate ToItemUpdate(this UpdateItemContract contract)
        {
            return new ItemUpdate(new StoreItemId(contract.OldId), contract.Name, contract.Comment,
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