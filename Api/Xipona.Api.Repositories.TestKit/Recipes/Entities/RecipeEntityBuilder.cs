using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.TestKit.Recipes.Entities;
public class RecipeEntityBuilder : TestBuilderBase<Recipe>
{
    public RecipeEntityBuilder()
    {
        WithIngredients(new IngredientEntityBuilder().CreateMany(3).ToArray());
        WithPreparationSteps(new PreparationStepEntityBuilder().CreateMany(3).ToArray());
        WithTags(new TagsForRecipeEntityBuilder().CreateMany(3).ToArray());
        WithoutSideDish();
    }

    // TCG keep
    public RecipeEntityBuilder WithIngredient(Ingredient ingredient)
    {
        return WithIngredients(new List<Ingredient> { ingredient });
    }

    public RecipeEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public RecipeEntityBuilder WithNumberOfServings(int numberOfServings)
    {
        FillPropertyWith(p => p.NumberOfServings, numberOfServings);
        return this;
    }

    public RecipeEntityBuilder WithCreatedAt(DateTimeOffset createdAt)
    {
        FillPropertyWith(p => p.CreatedAt, createdAt);
        return this;
    }

    public RecipeEntityBuilder WithSideDishId(Guid? sideDishId)
    {
        FillPropertyWith(p => p.SideDishId, sideDishId);
        return this;
    }

    public RecipeEntityBuilder WithoutSideDishId()
    {
        return WithSideDishId(null);
    }

    public RecipeEntityBuilder WithName(string name)
    {
        FillPropertyWith(p => p.Name, name);
        return this;
    }

    public RecipeEntityBuilder WithRowVersion(byte[] rowVersion)
    {
        FillPropertyWith(p => p.RowVersion, rowVersion);
        return this;
    }

    public RecipeEntityBuilder WithEmptyRowVersion()
    {
        return WithRowVersion(Array.Empty<byte>());
    }

    public RecipeEntityBuilder WithIngredients(ICollection<Ingredient> ingredients)
    {
        FillPropertyWith(p => p.Ingredients, ingredients);
        return this;
    }

    public RecipeEntityBuilder WithEmptyIngredients()
    {
        return WithIngredients(new List<Ingredient>());
    }

    public RecipeEntityBuilder WithPreparationSteps(ICollection<PreparationStep> preparationSteps)
    {
        FillPropertyWith(p => p.PreparationSteps, preparationSteps);
        return this;
    }

    public RecipeEntityBuilder WithEmptyPreparationSteps()
    {
        return WithPreparationSteps(new List<PreparationStep>());
    }

    public RecipeEntityBuilder WithTags(ICollection<TagsForRecipe> tags)
    {
        FillPropertyWith(p => p.Tags, tags);
        return this;
    }

    public RecipeEntityBuilder WithEmptyTags()
    {
        return WithTags(new List<TagsForRecipe>());
    }

    public RecipeEntityBuilder WithSideDish(Recipe? sideDish)
    {
        FillPropertyWith(p => p.SideDish, sideDish);
        return this;
    }

    public RecipeEntityBuilder WithoutSideDish()
    {
        return WithSideDish(null);
    }
}