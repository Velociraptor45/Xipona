namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public class Ingredients : IEnumerable<IIngredient>
{
    private readonly IDictionary<IngredientId, IIngredient> _ingredients;

    public Ingredients(IEnumerable<IIngredient> ingredients)
    {
        _ingredients = ingredients.ToDictionary(i => i.Id);
    }

    public IReadOnlyCollection<IIngredient> AsReadOnly()
    {
        return _ingredients.Values.ToList().AsReadOnly();
    }

    public IEnumerator<IIngredient> GetEnumerator()
    {
        return _ingredients.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}