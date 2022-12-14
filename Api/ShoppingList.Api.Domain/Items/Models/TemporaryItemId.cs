using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;

public readonly record struct TemporaryItemId
{
    public TemporaryItemId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public TemporaryItemId(Guid value)
    {
        Value = value;
    }

    public static TemporaryItemId New => new(Guid.NewGuid());

    public Guid Value { get; }

    public static implicit operator Guid(TemporaryItemId temporaryItemId)
    {
        return temporaryItemId.Value;
    }
}