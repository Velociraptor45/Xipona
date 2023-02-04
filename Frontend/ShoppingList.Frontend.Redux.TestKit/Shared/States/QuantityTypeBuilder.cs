using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
using ProjectHermes.ShoppingList.Frontend.TestTools;
using System;

namespace ShoppingList.Frontend.Redux.TestKit.ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
public class QuantityTypeBuilder : TestBuilderBase<QuantityType>
{
    public QuantityTypeBuilder WithId(int id)
    {
        FillConstructorWith("Id", id);
        return this;
    }

    public QuantityTypeBuilder WithName(string name)
    {
        FillConstructorWith("Name", name);
        return this;
    }

    public QuantityTypeBuilder WithDefaultQuantity(int defaultQuantity)
    {
        FillConstructorWith("DefaultQuantity", defaultQuantity);
        return this;
    }

    public QuantityTypeBuilder WithPriceLabel(string priceLabel)
    {
        FillConstructorWith("PriceLabel", priceLabel);
        return this;
    }

    public QuantityTypeBuilder WithQuantityLabel(string quantityLabel)
    {
        FillConstructorWith("QuantityLabel", quantityLabel);
        return this;
    }

    public QuantityTypeBuilder WithQuantityNormalizer(int quantityNormalizer)
    {
        FillConstructorWith("QuantityNormalizer", quantityNormalizer);
        return this;
    }
}