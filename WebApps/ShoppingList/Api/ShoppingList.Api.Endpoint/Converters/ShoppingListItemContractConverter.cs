using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Endpoint.Converters
{
    public static class ShoppingListItemContractConverter
    {
        public static ShoppingListItemContract ToContract(this ShoppingListItemReadModel readModel)
        {
            return new ShoppingListItemContract(readModel.Id.Value, readModel.Name, readModel.IsDeleted, readModel.Comment,
                readModel.IsTemporary, readModel.PricePerQuantity, (int)readModel.QuantityType, readModel.QuantityInPacket,
                (int)readModel.QuantityTypeInPacket, readModel.DefaultQuantity, readModel.QuantityLable,
                readModel.PriceLabel, readModel.ItemCategory.ToContract(),
                readModel.Manufacturer.ToContract(), readModel.IsInBasket, readModel.Quantity);
        }
    }
}