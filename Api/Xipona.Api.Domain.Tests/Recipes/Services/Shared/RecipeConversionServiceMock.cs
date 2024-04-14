using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Shared;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Recipes.Services.Shared;

public class RecipeConversionServiceMock : Mock<IRecipeConversionService>
{
    public RecipeConversionServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupToReadModel(IRecipe recipe, RecipeReadModel returnValue)
    {
        Setup(m => m.ToReadModelAsync(recipe))
            .ReturnsAsync(returnValue);
    }
}