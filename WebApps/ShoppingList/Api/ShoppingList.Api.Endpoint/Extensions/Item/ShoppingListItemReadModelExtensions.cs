using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Api.Domain.Queries.SharedModels;
using ShoppingList.Api.Endpoint.Extensions.ItemCategory;
using ShoppingList.Api.Endpoint.Extensions.Manufacturer;

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
                (int)readModel.QuantityType,
                readModel.QuantityInPacket,
                (int)readModel.QuantityTypeInPacket,
                readModel.DefaultQuantity,
                readModel.QuantityLable,
                readModel.PriceLabel,
                readModel.ItemCategory.ToContract(),
                readModel.Manufacturer.ToContract(),
                readModel.IsInBasket,
                readModel.Quantity);
        }
    }
}