using ProjectHermes.ShoppingList.Api.Contracts.RecipeTags.Queries.GetAll;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.RecipeTags;

public class RecipeTagContractConverter : IToContractConverter<IRecipeTag, RecipeTagContract>
{
    public RecipeTagContract ToContract(IRecipeTag source)
    {
        return new RecipeTagContract(source.Id.Value, source.Name);
    }
}