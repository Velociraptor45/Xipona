using System;

namespace ProjectHermes.ShoppingList.Api.Core.Attributes
{
    public class QuantityNormalizerAttribute : Attribute
    {
        public QuantityNormalizerAttribute(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}