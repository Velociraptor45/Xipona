using ProjectHermes.ShoppingList.Frontend.Models.TestKit.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Frontend.TestTools;

namespace ProjectHermes.ShoppingList.Frontend.Models.TestKit.Common;

public class DomainTestBuilderBase<TModel> : TestBuilderBase<TModel>
{
    public DomainTestBuilderBase()
    {
        Customize(new ShoppingListItemIdCustomization());
    }
}