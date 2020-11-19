using ShoppingList.Api.Core.Attributes;
using ShoppingList.Api.Core.Extensions;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.AllQuantityTypes;

namespace ShoppingList.Api.Domain.Extensions
{
    public static class QuantityTypeExtensions
    {
        public static QuantityTypeReadModel ToReadModel(this QuantityType quantityType)
        {
            return new QuantityTypeReadModel(
                (int)quantityType,
                quantityType.ToString(),
                quantityType.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                    quantityType.GetAttribute<PriceLabelAttribute>().PriceLabel);
        }
    }
}