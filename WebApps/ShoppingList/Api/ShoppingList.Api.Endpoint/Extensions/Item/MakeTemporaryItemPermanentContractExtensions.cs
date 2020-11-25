using ShoppingList.Api.Contracts.Commands.MakeTemporaryItemPermanent;
using ShoppingList.Api.Domain.Commands.MakeTemporaryItemPermanent;
using ShoppingList.Api.Domain.Models;
using System.Linq;

namespace ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class MakeTemporaryItemPermanentContractExtensions
    {
        public static PermanentItem ToDomain(this MakeTemporaryItemPermanentContract contract)
        {
            return new PermanentItem(
                new StoreItemId(contract.Id),
                contract.Name,
                contract.Comment,
                (QuantityType)contract.QuantityType,
                contract.QuantityInPacket,
                (QuantityTypeInPacket)contract.QuantityTypeInPacket,
                new ItemCategoryId(contract.ItemCategoryId),
                new ManufacturerId(contract.ManufacturerId),
                contract.Availabilities.Select(av => av.ToDomain()));
        }
    }
}