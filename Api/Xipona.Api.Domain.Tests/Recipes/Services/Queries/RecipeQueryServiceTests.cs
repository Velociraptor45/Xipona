﻿using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Ports;
using ProjectHermes.Xipona.Api.Domain.Tests.Recipes.Services.Shared;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using Xunit.Abstractions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Recipes.Services.Queries;

public class RecipeQueryServiceTests
{
    public sealed class GetAsync
    {
        private readonly GetAsyncFixture _fixture;

        public GetAsync(ITestOutputHelper output)
        {
            _fixture = new GetAsyncFixture(output);
        }

        [Fact]
        public async Task GetAsync_WithValidRecipeId_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupRecipeId();
            _fixture.SetupFindingRecipe();
            _fixture.SetupConvertingRecipe();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.RecipeId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.GetAsync(_fixture.RecipeId.Value);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public async Task GetAsync_WithInvalidRecipeId_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupRecipeId();
            _fixture.SetupNotFindingRecipe();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.RecipeId);

            // Act
            var func = async () => await sut.GetAsync(_fixture.RecipeId.Value);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.RecipeNotFound);
        }

        private sealed class GetAsyncFixture : RecipeQueryServiceFixture
        {
            private Recipe? _foundRecipe;

            public GetAsyncFixture(ITestOutputHelper output) : base(output)
            {
            }

            public RecipeReadModel? ExpectedResult { get; private set; }
            public RecipeId? RecipeId { get; private set; }

            public void SetupRecipeId()
            {
                RecipeId = Domain.Recipes.Models.RecipeId.New;
            }

            public void SetupFindingRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(RecipeId);
                _foundRecipe = new RecipeBuilder().Create();

                RecipeRepositoryMock.SetupFindByAsync(RecipeId.Value, _foundRecipe);
            }

            public void SetupNotFindingRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(RecipeId);

                RecipeRepositoryMock.SetupFindByAsync(RecipeId.Value, null);
            }

            public void SetupConvertingRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(_foundRecipe);

                ExpectedResult = new DomainTestBuilder<RecipeReadModel>().Create();

                RecipeConversionServiceMock.SetupToReadModel(_foundRecipe, ExpectedResult);
            }
        }
    }

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

    public sealed class SearchByTagIdsAsync
    {
        private readonly SearchByTagIdsAsyncFixture _fixture;

        public SearchByTagIdsAsync(ITestOutputHelper output)
        {
            _fixture = new SearchByTagIdsAsyncFixture(output);
        }

        [Fact]
        public async Task SearchByTagIdsAsync_WithRecipeTagIdsEmpty_ShouldReturnEmptyCollection()
        {
            // Arrange
            _fixture.SetupRecipeTagIdsEmpty();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.RecipeTagIds);

            // Act
            var results = await sut.SearchByTagIdsAsync(_fixture.RecipeTagIds);

            // Assert
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchByTagIdsAsync_WithRecipeTagIdsNotEmpty_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupRecipeTagIds();
            _fixture.SetupExpectedResult();
            _fixture.SetupFindingSearchResults();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.RecipeTagIds);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var results = await sut.SearchByTagIdsAsync(_fixture.RecipeTagIds);

            // Assert
            results.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class SearchByTagIdsAsyncFixture : RecipeQueryServiceFixture
        {
            public SearchByTagIdsAsyncFixture(ITestOutputHelper output) : base(output)
            {
            }

            public IEnumerable<RecipeSearchResult>? ExpectedResult { get; private set; }
            public IReadOnlyCollection<RecipeTagId>? RecipeTagIds { get; private set; }

            public void SetupExpectedResult()
            {
                ExpectedResult = new DomainTestBuilder<RecipeSearchResult>().CreateMany(3);
            }

            public void SetupRecipeTagIds()
            {
                RecipeTagIds = new DomainTestBuilder<RecipeTagId>().CreateMany(3).ToList();
            }

            public void SetupRecipeTagIdsEmpty()
            {
                RecipeTagIds = new List<RecipeTagId>();
            }

            public void SetupFindingSearchResults()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                TestPropertyNotSetException.ThrowIfNull(RecipeTagIds);

                var results = ExpectedResult
                    .Select(r => new RecipeBuilder()
                        .WithId(r.Id)
                        .WithName(r.Name)
                        .Create())
                    .ToList<IRecipe>();

                RecipeRepositoryMock.SetupFindByContainingAllAsync(RecipeTagIds, results);
            }
        }
    }

    public abstract class RecipeQueryServiceFixture
    {
        protected readonly RecipeRepositoryMock RecipeRepositoryMock = new(MockBehavior.Strict);
        protected readonly ItemRepositoryMock ItemRepositoryMock = new(MockBehavior.Strict);
        protected readonly StoreRepositoryMock StoreRepositoryMock = new(MockBehavior.Strict);
        protected readonly RecipeConversionServiceMock RecipeConversionServiceMock = new(MockBehavior.Strict);
        private readonly ILogger<RecipeQueryService> _logger;

        protected RecipeQueryServiceFixture(ITestOutputHelper output)
        {
            _logger = output.BuildLoggerFor<RecipeQueryService>();
        }

        public RecipeQueryService CreateSut()
        {
            return new RecipeQueryService(
                RecipeRepositoryMock.Object,
                ItemRepositoryMock.Object,
                RecipeConversionServiceMock.Object,
                StoreRepositoryMock.Object,
                new QuantityTranslationService(),
                _logger);
        }
    }
}