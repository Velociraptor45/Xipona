using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Models
{
    public static class ShoppingListItemExtensions
    {
        public static Infrastructure.Entities.Item ToEntity(this IShoppingListItem model)
        {
            return new Infrastructure.Entities.Item()
            {
                Id = model.Id.Actual.Value,
                Name = model.Name,
                Deleted = model.IsDeleted,
                Comment = model.Comment,
                IsTemporary = model.IsTemporary,
                QuantityType = (int)model.QuantityType,
                QuantityInPacket = model.QuantityInPacket,
                QuantityTypeInPacket = (int)model.QuantityTypeInPacket,
                ItemCategoryId = model.ItemCategory?.Id.Value,
                ManufacturerId = model.Manufacturer?.Id.Value
            };
        }
    }
}