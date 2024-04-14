using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.TestKit.Recipes.Entities;

public class TagsForRecipeEntityBuilder : TestBuilderBase<TagsForRecipe>
{
    public TagsForRecipeEntityBuilder()
    {
        WithoutRecipe();
    }

    public TagsForRecipeEntityBuilder WithRecipeId(Guid recipeId)
    {
        FillPropertyWith(p => p.RecipeId, recipeId);
        return this;
    }

    public TagsForRecipeEntityBuilder WithRecipeTagId(Guid recipeTagId)
    {
        FillPropertyWith(p => p.RecipeTagId, recipeTagId);
        return this;
    }

    public TagsForRecipeEntityBuilder WithRecipe(Recipe? recipe)
    {
        FillPropertyWith(p => p.Recipe, recipe);
        return this;
    }

    public TagsForRecipeEntityBuilder WithoutRecipe()
    {
        return WithRecipe(null);
    }
}