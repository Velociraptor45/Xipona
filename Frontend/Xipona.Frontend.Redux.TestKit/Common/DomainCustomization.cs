using AutoFixture;
using Xipona.Frontend.Redux.TestKit.Shared.Customizations;
using Xipona.Frontend.Redux.TestKit.ShoppingList.States;

namespace Xipona.Frontend.Redux.TestKit.Common;

public class DomainCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize(new ShoppingListItemIdCustomization());
        fixture.Customize(new SortedSetCustomization());
    }
}