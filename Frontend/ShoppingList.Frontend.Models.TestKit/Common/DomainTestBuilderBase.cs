using ShoppingList.Frontend.Models.TestKit.ShoppingLists.Models;
using ShoppingList.Frontend.TestTools;

namespace ShoppingList.Frontend.Models.TestKit.Common;

public class DomainTestBuilderBase<TModel> : TestBuilderBase<TModel>
{
    public DomainTestBuilderBase()
    {
        Customize(new ShoppingListItemIdCustomization());
    }
}