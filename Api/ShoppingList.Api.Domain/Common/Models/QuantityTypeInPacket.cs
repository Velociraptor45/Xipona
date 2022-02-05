using ProjectHermes.ShoppingList.Api.Core.Attributes;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models;

public enum QuantityTypeInPacket
{
    [QuantityLabel("x")]
    Unit = 0,

    [QuantityLabel("g")]
    Weight = 1,

    [QuantityLabel("ml")]
    Fluid = 2
}