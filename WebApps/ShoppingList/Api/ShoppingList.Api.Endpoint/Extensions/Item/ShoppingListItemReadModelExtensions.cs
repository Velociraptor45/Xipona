using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Api.Domain.Queries.SharedModels;
using ShoppingList.Api.Endpoint.Extensions.ItemCategory;
using ShoppingList.Api.Endpoint.Extensions.Manufacturer;
using ShoppingList.Api.Endpoint.Extensions.ShoppingList;

namespace ShoppingList.Api.Endpoint.Extensions.Item
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
                readModel.ItemCategory.ToContract(),
                readModel.Manufacturer.ToContract(),
                readModel.IsInBasket,
                readModel.Quantity);
        }
    }
}