using ProjectHermes.ShoppingList.Api.Contracts.RecipeTags.Queries.GetAll;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class RecipeTagConverter : IToDomainConverter<RecipeTagContract, RecipeTag>
{
    public RecipeTag ToDomain(RecipeTagContract source)
    {
        return new RecipeTag(source.Id, source.Name);
    }
}