using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;

public class ShoppingListSectionBuilder : DomainTestBuilderBase<ShoppingListSection>
{
    public ShoppingListSectionBuilder()
    {
        Customize(new QuantityInBasketCustomization());
    }

    public ShoppingListSectionBuilder(ShoppingListSection section) : this()
    {
        WithId(section.Id);
        WithItems(section.Items);
    }

    public ShoppingListSectionBuilder WithId(SectionId id)
    {
        FillConstructorWith("id", id);
        return this;
    }

    public ShoppingListSectionBuilder WithItems(IEnumerable<ShoppingListItem> items)
    {
        FillConstructorWith("shoppingListItems", items);
        return this;
    }

    public ShoppingListSectionBuilder WithItem(ShoppingListItem item)
    {
        return WithItems(item.ToMonoList());
    }

    public ShoppingListSectionBuilder WithoutItems()
    {
        return WithItems(Enumerable.Empty<ShoppingListItem>());
    }
}