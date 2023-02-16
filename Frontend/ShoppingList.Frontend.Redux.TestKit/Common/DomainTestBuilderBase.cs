using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Customizations;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.TestTools;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;

public class DomainTestBuilderBase<TModel> : TestBuilderBase<TModel>
{
    public DomainTestBuilderBase()
    {
        Customize(new ShoppingListItemIdCustomization());
        Customize(new SortedSetCustomization());
    }
}