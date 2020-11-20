using System;

namespace ShoppingList.Api.Core.Attributes
{
    public class DefaultQuantityAttribute : Attribute
    {
        public DefaultQuantityAttribute(int defaultQuantity)
        {
            DefaultQuantity = defaultQuantity;
        }

        public int DefaultQuantity { get; }
    }
}