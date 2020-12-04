using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Shared;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class ShoppingListItemContractExtensions
    {
        public static ShoppingListItem ToModel(this ShoppingListItemContract contract)
        {
            return new ShoppingListItem(
                    new ItemId(contract.Id),
                    contract.Name,
                    contract.IsTemporary,
                    contract.PricePerQuantity,
                    contract.QuantityType.ToModel(),
                    contract.QuantityInPacket,
                    contract.QuantityTypeInPacket.ToModel(),
                    contract.ItemCategory?.Name ?? "",
                    contract.Manufacturer?.Name ?? "",
                    contract.IsInBasket,
                    contract.Quantity);
        }
    }
}