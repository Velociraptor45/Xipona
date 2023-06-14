namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

public class RecipeTag
{
    public RecipeTag(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
}