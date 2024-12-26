using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;

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
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public ItemTypeBuilder WithName(ItemTypeName name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public ItemTypeBuilder WithAvailabilities(IEnumerable<ItemAvailability> availabilities)
    {
        FillConstructorWith(nameof(availabilities), availabilities);
        return this;
    }

    public ItemTypeBuilder WithEmptyAvailabilities()
    {
        return WithAvailabilities(Enumerable.Empty<ItemAvailability>());
    }

    public ItemTypeBuilder WithPredecessorId(ItemTypeId? predecessorId)
    {
        FillConstructorWith(nameof(predecessorId), predecessorId);
        return this;
    }

    public ItemTypeBuilder WithoutPredecessorId()
    {
        return WithPredecessorId(null);
    }

    public ItemTypeBuilder WithIsDeleted(bool isDeleted)
    {
        FillConstructorWith(nameof(isDeleted), isDeleted);
        return this;
    }

    public ItemTypeBuilder WithCreatedAt(DateTimeOffset createdAt)
    {
        FillConstructorWith(nameof(createdAt), createdAt);
        return this;
    }

    // tcg keep
    public ItemTypeBuilder WithAvailability(ItemAvailability availability)
    {
        return WithAvailabilities([availability]);
    }
}