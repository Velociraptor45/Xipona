namespace ProjectHermes.Xipona.Api.Domain.Recipes.Models;
public readonly record struct RecipeId
{
    public RecipeId()
    {
        throw new NotSupportedException("Use 'New' property to create initial value.");
    }

    public RecipeId(Guid value)
    {
        Value = value;
    }

    public static RecipeId New => new(Guid.NewGuid());
    public Guid Value { get; }

    public static implicit operator Guid(RecipeId recipeId)
    {
        return recipeId.Value;
    }

    public override string ToString()
    {
        return Value.ToString("D");
    }
}