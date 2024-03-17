using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models.Factories;
using RecipeTag = ProjectHermes.Xipona.Api.Repositories.RecipeTags.Entities.RecipeTag;

namespace ProjectHermes.Xipona.Api.Repositories.RecipeTags.Converters.ToDomain;

public class RecipeTagConverter : IToDomainConverter<RecipeTag, IRecipeTag>
{
    private readonly IRecipeTagFactory _factory;

    public RecipeTagConverter(IRecipeTagFactory factory)
    {
        _factory = factory;
    }

    public IRecipeTag ToDomain(RecipeTag source)
    {
        var recipeTag = (AggregateRoot)_factory.Create(new RecipeTagId(source.Id), source.Name, source.CreatedAt);
        recipeTag.EnrichWithRowVersion(source.RowVersion);
        return (recipeTag as IRecipeTag)!;
    }
}