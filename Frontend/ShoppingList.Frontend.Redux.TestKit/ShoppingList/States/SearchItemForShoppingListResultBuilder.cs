using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.ShoppingList.States;

public class SearchItemForShoppingListResultBuilder : DomainTestBuilderBase<SearchItemForShoppingListResult>
{
    public SearchItemForShoppingListResultBuilder WithPriceLabel(string priceLabel)
    {
        FillConstructorWith(nameof(priceLabel), priceLabel);
        return this;
    }

    public SearchItemForShoppingListResultBuilder WithPrice(float price)
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