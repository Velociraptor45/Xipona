using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models.Factories;
using RecipeTag = ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Entities.RecipeTag;

namespace ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Converters.ToDomain;

public class RecipeTagConverter : IToDomainConverter<RecipeTag, IRecipeTag>
{
    private readonly IRecipeTagFactory _factory;

    public RecipeTagConverter(IRecipeTagFactory factory)
    {
        _factory = factory;
    }

    public IRecipeTag ToDomain(RecipeTag source)
    {
        var recipeTag = (AggregateRoot)_factory.Create(new RecipeTagId(source.Id), source.Name);
        recipeTag.EnrichWithRowVersion(source.RowVersion);
        return (recipeTag as IRecipeTag)!;
    }
}