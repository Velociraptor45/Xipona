using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.RecipeTags;

public class RecipeTagContractConverter : IToContractConverter<IRecipeTag, RecipeTagContract>
{
    public RecipeTagContract ToContract(IRecipeTag source)
    {
        return new RecipeTagContract(source.Id.Value, source.Name);
    }
}