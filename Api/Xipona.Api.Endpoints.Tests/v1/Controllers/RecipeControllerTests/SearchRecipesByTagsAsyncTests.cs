using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Queries.SearchByTagIds;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.RecipeControllerTests;

public class SearchRecipesByTagsAsyncTests : ControllerEnumerableQueryTestsBase<RecipeController, SearchRecipesByTagsQuery,
        RecipeSearchResult, RecipeSearchResultContract, SearchRecipesByTagsAsyncTests.SearchRecipesByTagsAsyncFixture>
{
    public SearchRecipesByTagsAsyncTests() : base(new SearchRecipesByTagsAsyncFixture())
    {
    }

    public sealed class SearchRecipesByTagsAsyncFixture : ControllerEnumerableQueryFixtureBase
    {
        private IReadOnlyCollection<Guid>? _tagIds;

        public SearchRecipesByTagsAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override MethodInfo Method =>
            typeof(RecipeController).GetMethod(nameof(RecipeController.SearchRecipesByTagsAsync))!;

        public override RecipeController CreateSut()
        {
            return new RecipeController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(RecipeController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_tagIds);
            return await sut.SearchRecipesByTagsAsync(_tagIds);
        }

        public override void SetupParameters()
        {
            _tagIds = new TestBuilder<Guid>().CreateMany(2).ToList();
        }

        public override void SetupQuery()
        {
            TestPropertyNotSetException.ThrowIfNull(_tagIds);
            Query = new SearchRecipesByTagsQuery(_tagIds.Select(t => new RecipeTagId(t)));
        }
    }
}