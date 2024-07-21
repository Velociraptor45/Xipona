using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;

namespace ProjectHermes.Xipona.Frontend.Redux.TestKit.ShoppingList.States;

public class SearchItemForShoppingListResultBuilder : DomainTestBuilderBase<SearchItemForShoppingListResult>
{
    public SearchItemForShoppingListResultBuilder WithPriceLabel(string priceLabel)
    {
        FillConstructorWith(nameof(priceLabel), priceLabel);
        return this;
    }

    public SearchItemForShoppingListResultBuilder WithPrice(decimal price)
    {
        FillConstructorWith(nameof(price), price);
        return this;
    }

    public SearchItemForShoppingListResultBuilder WithManufacturerName(string manufacturerName)
    {
        FillConstructorWith(nameof(manufacturerName), manufacturerName);
        return this;
    }

    public SearchItemForShoppingListResultBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }
}