using ProjectHermes.ShoppingList.Frontend.Models.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;

namespace ProjectHermes.ShoppingList.Frontend.Models.TestKit.Items.Models;

public class QuantityTypeBuilder : DomainTestBuilderBase<QuantityType>
{
    public QuantityTypeBuilder WithQuantityNormalizer(int quantityNormalizer)
    {
        FillConstructorWith("QuantityNormalizer", quantityNormalizer);
        return this;
    }
}