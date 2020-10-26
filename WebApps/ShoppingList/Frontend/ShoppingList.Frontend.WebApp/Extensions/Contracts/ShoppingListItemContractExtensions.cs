using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Frontend.Models;

namespace ShoppingList.Frontend.WebApp.Extensions.Contracts
{
    public static class ShoppingListItemContractExtensions
    {
        public static ShoppingListItem ToModel(this ShoppingListItemContract contract)
        {
            return new ShoppingListItem(contract.Id, contract.Name, contract.IsTemporary, contract.Price,
                contract.QuantityInPacket, contract.ItemCategory.Name, contract.Manufacturer.Name,
                contract.IsInBasket, contract.Quantity);
        }
    }
}