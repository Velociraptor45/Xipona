using Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.RecipeTags.Models;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.RecipeTags;

public class RecipeTagContractConverter : IToContractConverter<IRecipeTag, RecipeTagContract>
{
    public RecipeTagContract ToContract(IRecipeTag source)
    {
        return new RecipeTagContract(source.Id.Value, source.Name);
    }
}