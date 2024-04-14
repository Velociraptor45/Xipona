using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Services.Query;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.RecipeTags.Services;

public class RecipeTagQueryServiceMock : Mock<IRecipeTagQueryService>
{
    public RecipeTagQueryServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupGetAllAsync(IEnumerable<IRecipeTag> returnValue)
    {
        Setup(x => x.GetAllAsync()).ReturnsAsync(returnValue);
    }
}