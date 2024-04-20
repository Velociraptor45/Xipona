using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.RecipeTags.Queries.GetAll;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.RecipeTagControllerTests;

public class GetAllRecipeTagsAsyncTests : ControllerEnumerableQueryTestsBase<RecipeTagController,
    GetAllQuery, IRecipeTag, RecipeTagContract, GetAllRecipeTagsAsyncTests.GetAllRecipeTagsAsyncFixture>
{
    public GetAllRecipeTagsAsyncTests() : base(new GetAllRecipeTagsAsyncFixture())
    {
    }

    public sealed class GetAllRecipeTagsAsyncFixture : ControllerEnumerableQueryFixtureBase
    {
        public GetAllRecipeTagsAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override MethodInfo Method =>
            typeof(RecipeTagController).GetMethod(nameof(RecipeTagController.GetAllRecipeTagsAsync))!;

        public override RecipeTagController CreateSut()
        {
            return new RecipeTagController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(RecipeTagController sut)
        {
            return await sut.GetAllRecipeTagsAsync();
        }

        public override void SetupParameters()
        {
        }

        public override void SetupQuery()
        {
            Query = new GetAllQuery();
        }
    }
}