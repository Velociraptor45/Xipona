using ShoppingList.Api.Domain.Models;
using System;

namespace ShoppingList.Api.Domain.Extensions
{
    public static class QuantityTypeInPacketExtensions
    {
        public static string ToPriceLabel(this QuantityTypeInPacket quantityTypeInPacket)
        {
            return quantityTypeInPacket switch
            {
                QuantityTypeInPacket.Unit => "€",
                QuantityTypeInPacket.Weight => "€/kg",
                QuantityTypeInPacket.Fluid => "€/l",
                _ => throw new InvalidOperationException(
                    $"{nameof(QuantityTypeInPacket)} value {quantityTypeInPacket} not recognized")
            };
        }
    }
}