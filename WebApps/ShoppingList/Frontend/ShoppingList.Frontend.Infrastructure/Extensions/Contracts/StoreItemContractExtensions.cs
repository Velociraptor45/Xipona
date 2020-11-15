using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Frontend.Models.Items;
using System.Linq;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class StoreItemContractExtensions
    {
        public static StoreItem ToModel(this StoreItemContract contract)
        {
            return new StoreItem(contract.Id, contract.Name, contract.IsDeleted, contract.Comment,
                contract.IsTemporary, contract.QuantityType, contract.QuantityInPacket,
                contract.QuantityTypeInPacket, contract.ItemCategory.Id, contract.Manufacturer.Id,
                contract.Availabilities.Select(av => av.ToModel()));
        }
    }
}