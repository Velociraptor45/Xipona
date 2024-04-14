using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Models;

public class RecipeBuilder : DomainTestBuilderBase<Recipe>
{
    public RecipeBuilder WithId(RecipeId id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public RecipeBuilder WithName(RecipeName name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public RecipeBuilder WithNumberOfServings(NumberOfServings numberOfServings)
    {
        FillConstructorWith(nameof(numberOfServings), numberOfServings);
        return this;
    }

    public RecipeBuilder WithIngredients(Ingredients ingredients)
    {
        FillConstructorWith(nameof(ingredients), ingredients);
        return this;
    }

    public RecipeBuilder WithSteps(PreparationSteps steps)
    {
        FillConstructorWith(nameof(steps), steps);
        return this;
    }

    public RecipeBuilder WithTags(Domain.Recipes.Models.RecipeTags tags)
    {
        FillConstructorWith(nameof(tags), tags);
        return this;
    }

    public RecipeBuilder WithCreatedAt(DateTimeOffset createdAt)
    {
        FillConstructorWith(nameof(createdAt), createdAt);
        return this;
    }
}