using AutoFixture;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Customizations;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;

public class DomainCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize(new ShoppingListItemIdCustomization());
        fixture.Customize(new SortedSetCustomization());
    }
}