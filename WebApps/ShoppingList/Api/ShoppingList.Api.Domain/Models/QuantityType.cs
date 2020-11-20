using ShoppingList.Api.Core.Attributes;

namespace ShoppingList.Api.Domain.Models
{
    public enum QuantityType
    {
        [PriceLabel("€")]
        [DefaultQuantity(1)]
        Unit = 0,

        [PriceLabel("€/kg")]
        [DefaultQuantity(100)]
        Weight = 1
    }
}