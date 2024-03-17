using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using RecipeTag = ProjectHermes.Xipona.Api.Repositories.RecipeTags.Entities.RecipeTag;

namespace ProjectHermes.Xipona.Api.Repositories.RecipeTags.Converters.ToContract;

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