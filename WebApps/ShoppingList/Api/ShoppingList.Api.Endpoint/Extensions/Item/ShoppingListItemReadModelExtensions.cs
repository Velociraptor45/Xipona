using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Item;
using ProjectHermes.ShoppingList.Api.Endpoint.Extensions.ItemCategory;
using ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Manufacturer;
using ProjectHermes.ShoppingList.Api.Endpoint.Extensions.ShoppingList;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class ShoppingListItemReadModelExtensions
    {
        public static ShoppingListItemContract ToContract(this ShoppingListItemReadModel readModel)
        {
            return new ShoppingListItemContract(
                readModel.Id.Value,
                readModel.Name,
                readModel.IsDeleted,
                readModel.Comment,
                readModel.IsTemporary,
                readModel.PricePerQuantity,
                readModel.QuantityType.ToContract(),
                readModel.QuantityInPacket,
                readModel.QuantityTypeInPacket.ToContract(),
                readModel.ItemCategory?.ToContract(),
                readModel.Manufacturer?.ToContract(),
                readModel.IsInBasket,
                readModel.Quantity);
        }
    }
}