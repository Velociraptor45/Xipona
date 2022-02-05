using System;

namespace ProjectHermes.ShoppingList.Api.Core.Attributes;

public class QuantityLabelAttribute : Attribute
{
    public QuantityLabelAttribute(string quantityLabel)
    {
        QuantityLabel = quantityLabel;
    }

    public string QuantityLabel { get; }
}