using AutoFixture;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Customizations;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.ShoppingList.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;

public class DomainCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize(new ShoppingListItemIdCustomization());
        fixture.Customize(new SortedSetCustomization());
    }
}