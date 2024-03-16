using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using RecipeTag = ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Entities.RecipeTag;

namespace ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Converters.ToContract;

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