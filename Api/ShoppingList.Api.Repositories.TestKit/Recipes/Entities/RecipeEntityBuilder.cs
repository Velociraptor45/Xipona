using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.TestKit.Recipes.Entities;

public class RecipeEntityBuilder : TestBuilderBase<Recipe>
{
    public RecipeEntityBuilder()
    {
        WithIngredients(new IngredientEntityBuilder().CreateMany(3).ToArray());
        WithPreparationSteps(new PreparationStepEntityBuilder().CreateMany(3).ToArray());
        WithTags(new TagsForRecipeEntityBuilder().CreateMany(3).ToArray());
    }

    public RecipeEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public RecipeEntityBuilder WithName(string name)
    {
        FillPropertyWith(p => p.Name, name);
        return this;
    }

    // TCG keep
    public RecipeEntityBuilder WithIngredient(Ingredient ingredient)
    {
        return WithIngredients(new List<Ingredient> { ingredient });
    }

    public RecipeEntityBuilder WithIngredients(ICollection<Ingredient> ingredients)
    {
        FillPropertyWith(p => p.Ingredients, ingredients);
        return this;
    }

    public RecipeEntityBuilder WithPreparationSteps(ICollection<PreparationStep> preparationSteps)
    {
        FillPropertyWith(p => p.PreparationSteps, preparationSteps);
        return this;
    }

    public RecipeEntityBuilder WithTags(ICollection<TagsForRecipe> tags)
    {
        FillPropertyWith(p => p.Tags, tags);
        return this;
    }

    public RecipeEntityBuilder WithNumberOfServings(int numberOfServings)
    {
        FillPropertyWith(p => p.NumberOfServings, numberOfServings);
        return this;
    }
}