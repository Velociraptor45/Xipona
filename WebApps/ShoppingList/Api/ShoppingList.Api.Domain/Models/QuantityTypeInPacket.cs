using ShoppingList.Api.Core.Attributes;

namespace ShoppingList.Api.Domain.Models
{
    public enum QuantityTypeInPacket
    {
        [QuantityLabel("x")]
        Unit = 0,

        [QuantityLabel("g")]
        Weight = 1,

        [QuantityLabel("ml")]
        Fluid = 2
    }
}