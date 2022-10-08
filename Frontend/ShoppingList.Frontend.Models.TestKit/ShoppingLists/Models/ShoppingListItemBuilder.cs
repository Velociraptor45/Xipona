using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Frontend.Models.TestKit.Common;

namespace ProjectHermes.ShoppingList.Frontend.Models.TestKit.ShoppingLists.Models;

public class ShoppingListItemBuilder : DomainTestBuilderBase<ShoppingListItem>
{
    public ShoppingListItemBuilder WithQuantity(float quantity)
    {
        FillConstructorWith(nameof(quantity), quantity);
        return this;
    }
}