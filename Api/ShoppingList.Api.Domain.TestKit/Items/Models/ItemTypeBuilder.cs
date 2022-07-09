using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;

public class ItemTypeBuilder : DomainTestBuilderBase<ItemType>
{
    public ItemTypeBuilder()
    {
        Customize(new PriceCustomization());
    }

    public ItemTypeBuilder(IItemType itemType)
    {
        WithId(itemType.Id);
        WithName(itemType.Name);
        WithAvailabilities(itemType.Availabilities);
    }

    public ItemTypeBuilder WithId(ItemTypeId id)
    {
        FillConstructorWith("id", id);
        return this;
    }

    public ItemTypeBuilder WithName(ItemTypeName name)
    {
        FillConstructorWith("name", name);
        return this;
    }

    public ItemTypeBuilder WithAvailabilities(IEnumerable<IItemAvailability> availabilities)
    {
        FillConstructorWith("availabilities", availabilities);
        return this;
    }
}