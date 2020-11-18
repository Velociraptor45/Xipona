using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Domain.Extensions
{
    public static class ShoppingListItemExtensions
    {
        public static ShoppingListItemReadModel ToReadModel(this ShoppingListItem model)
        {
            return new ShoppingListItemReadModel(model.Id, model.Name, model.IsDeleted, model.Comment,
                model.IsTemporary, model.PricePerQuantity, model.QuantityType, model.QuantityInPacket,
                model.QuantityType.ToDefaultQuantity(), model.QuantityType.ToLabel(),
                model.QuantityTypeInPacket.ToPriceLabel(), model.QuantityTypeInPacket,
                model.ItemCategory.ToReadModel(), model.Manufacturer.ToReadModel(),
                model.IsInBasket, model.Quantity);
        }
    }
}