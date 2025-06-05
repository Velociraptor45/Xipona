using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Common.Models;
using Xipona.Api.Domain.RecipeTags.Models;
using RecipeTag = Xipona.Api.Repositories.RecipeTags.Entities.RecipeTag;

namespace Xipona.Api.Repositories.RecipeTags.Converters.ToContract;

public class RecipeTagConverter : IToContractConverter<IRecipeTag, RecipeTag>
{
    public RecipeTag ToContract(IRecipeTag source)
    {
        return new RecipeTag
        {
            Id = source.Id.Value,
            Name = source.Name,
            CreatedAt = source.CreatedAt,
            RowVersion = ((AggregateRoot)source).RowVersion
        };
    }
}