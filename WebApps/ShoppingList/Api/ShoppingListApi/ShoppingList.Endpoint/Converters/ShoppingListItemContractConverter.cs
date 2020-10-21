using ShoppingList.Contracts.SharedContracts;
using ShoppingList.Domain.Queries.SharedModels;

namespace ShoppingList.Endpoint.Converters
{
    public static class ShoppingListItemContractConverter
    {
        public static ShoppingListItemContract ToContract(this ShoppingListItemReadModel readModel)
        {
            return new ShoppingListItemContract(readModel.Id.Value, readModel.Name, readModel.IsDeleted, readModel.Comment,
                readModel.IsTemporary, readModel.Price, readModel.QuantityType.ToString(), readModel.QuantityInPacket,
                readModel.QuantityTypeInPacket.ToString(), readModel.ItemCategory.ToContract(),
                readModel.Manufacturer.ToContract(), readModel.IsInBasket, readModel.Quantity);
        }
    }
}