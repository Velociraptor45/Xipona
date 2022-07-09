using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ProjectHermes.ShoppingList.Frontend.Models.TestKit.Common;

namespace ProjectHermes.ShoppingList.Frontend.Models.TestKit.Items.Models;

public class QuantityTypeBuilder : DomainTestBuilderBase<QuantityType>
{
    public QuantityTypeBuilder WithQuantityNormalizer(int quantityNormalizer)
    {
        FillConstructorWith(nameof(quantityNormalizer), quantityNormalizer);
        return this;
    }
}