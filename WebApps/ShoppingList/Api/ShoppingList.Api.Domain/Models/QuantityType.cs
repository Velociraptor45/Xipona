using ShoppingList.Api.Core.Attributes;

namespace ShoppingList.Api.Domain.Models
{
    public enum QuantityType
    {
        [PriceLabel("€")]
        Unit = 0,

        [PriceLabel("€/kg")]
        Weight = 1
    }
}