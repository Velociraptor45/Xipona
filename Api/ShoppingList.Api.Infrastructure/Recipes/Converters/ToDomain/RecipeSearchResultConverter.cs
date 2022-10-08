using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using Recipe = ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities.Recipe;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Converters.ToDomain;

public class RecipeSearchResultConverter : IToDomainConverter<Recipe, RecipeSearchResult>
{
    public RecipeSearchResult ToDomain(Recipe source)
    {
        return new RecipeSearchResult(new RecipeId(source.Id), new RecipeName(source.Name));
    }
}