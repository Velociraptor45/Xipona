using ProjectHermes.Xipona.Api.Core.Services;

namespace ProjectHermes.Xipona.Api.Domain.RecipeTags.Models.Factories;

internal class RecipeTagFactory : IRecipeTagFactory
{
    private readonly IDateTimeService _dateTimeService;

    public RecipeTagFactory(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public IRecipeTag Create(RecipeTagId id, string name, DateTimeOffset createdAt)
    {
        return new RecipeTag(id, new RecipeTagName(name), createdAt);
    }

    public IRecipeTag CreateNew(string name)
    {
        return new RecipeTag(RecipeTagId.New, new RecipeTagName(name), _dateTimeService.UtcNow);
    }
}