using ShoppingList.Api.Domain.Models;
using System;

namespace ShoppingList.Api.Domain.Extensions
{
    public static class QuantityTypeExtensions
    {
        public static string ToLabel(this QuantityType quantityType)
        {
            return quantityType switch
            {
                QuantityType.Unit => "",
                QuantityType.Weight => "g",
                _ => throw new InvalidOperationException(
                    $"{nameof(QuantityTypeInPacket)} value {quantityType} not recognized")
            };
        }

        public static int ToDefaultQuantity(this QuantityType quantityType)
        {
            return quantityType switch
            {
                QuantityType.Unit => 1,
                QuantityType.Weight => 100,
                _ => throw new InvalidOperationException(
                    $"{nameof(QuantityTypeInPacket)} value {quantityType} not recognized")
            };
        }
    }
}