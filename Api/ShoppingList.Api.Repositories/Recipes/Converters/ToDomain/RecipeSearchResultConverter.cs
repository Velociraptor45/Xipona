using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using Recipe = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.Recipe;

namespace ProjectHermes.ShoppingList.Api.Repositories.Recipes.Converters.ToDomain;

public class RecipeSearchResultConverter : IToDomainConverter<Entities.Recipe, RecipeSearchResult>
{
    public RecipeSearchResult ToDomain(Recipe source)
    {
        return new RecipeSearchResult(new RecipeId(source.Id), new RecipeName(source.Name));
    }
}