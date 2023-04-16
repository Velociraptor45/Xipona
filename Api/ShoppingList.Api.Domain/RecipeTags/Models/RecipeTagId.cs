namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

public record struct RecipeTagId
{
    public RecipeTagId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public RecipeTagId(Guid value)
    {
        Value = value;
    }

    public static RecipeTagId New => new(Guid.NewGuid());

    public Guid Value { get; }

    public static implicit operator Guid(RecipeTagId itemId)
    {
        return itemId.Value;
    }
}