using ProjectHermes.ShoppingList.Api.Core.Attributes;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models
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