using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using Xunit.Abstractions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.Services.Queries;

public class RecipeQueryServiceTests
{
    public sealed class SearchByNameAsync
    {
        private readonly SearchByNameAsyncFixture _fixture;

        public SearchByNameAsync(ITestOutputHelper output)
        {
            _fixture = new SearchByNameAsyncFixture(output);
        }

        [Fact]
        public async Task SearchByNameAsync_WithSearchInputEmpty_ShouldReturnEmptyCollection()
        {
            // Arrange
            _fixture.SetupSearchInputEmpty();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.SearchInput);

            // Act
            var results = await sut.SearchByNameAsync(_fixture.SearchInput);

            // Assert
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchByNameAsync_WithSearchInputNotEmpty_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupSearchInput();
            _fixture.SetupExpectedResult();
            _fixture.SetupFindingSearchResults();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.SearchInput);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var results = await sut.SearchByNameAsync(_fixture.SearchInput);

            // Assert
            results.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class SearchByNameAsyncFixture : RecipeQueryServiceFixture
        {
            public SearchByNameAsyncFixture(ITestOutputHelper output) : base(output)
            {
            }

            public string? SearchInput { get; private set; }
            public IEnumerable<RecipeSearchResult>? ExpectedResult { get; private set; }

            public void SetupSearchInputEmpty()
            {
                SearchInput = string.Empty;
            }

            public void SetupSearchInput()
            {
                SearchInput = new TestBuilder<string>().Create();
            }

            public void SetupExpectedResult()
            {
                ExpectedResult = new DomainTestBuilder<RecipeSearchResult>().CreateMany(3);
            }

            public void SetupFindingSearchResults()
            {
                TestPropertyNotSetException.ThrowIfNull(SearchInput);
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                RecipeRepositoryMock.SetupSearchByAsync(SearchInput, ExpectedResult);
            }
        }
    }

    public abstract class RecipeQueryServiceFixture
    {
        protected readonly RecipeRepositoryMock RecipeRepositoryMock = new(MockBehavior.Strict);
        private readonly ILogger<RecipeQueryService> _logger;

        protected RecipeQueryServiceFixture(ITestOutputHelper output)
        {
            _logger = output.BuildLoggerFor<RecipeQueryService>();
        }

        public RecipeQueryService CreateSut()
        {
            return new RecipeQueryService(_ => RecipeRepositoryMock.Object, _logger, default);
        }
    }
}