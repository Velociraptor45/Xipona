using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Extensions
{
    public static class ShoppingListItemExtensions
    {
        public static ShoppingListItemReadModel ToReadModel(this IShoppingListItem model)
        {
            return new ShoppingListItemReadModel(
                model.Id.Actual,
                model.Name,
                model.IsDeleted,
                model.Comment,
                model.IsTemporary,
                model.PricePerQuantity,
                model.QuantityType.ToReadModel(),
                model.QuantityInPacket,
                model.QuantityTypeInPacket.ToReadModel(),
                model.ItemCategory?.ToReadModel(),
                model.Manufacturer?.ToReadModel(),
                model.IsInBasket, model.Quantity);
        }
    }
}