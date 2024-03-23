using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Services.Query;
using ProjectHermes.Xipona.Api.Domain.TestKit.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.RecipeTags.Ports;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.RecipeTags.Services;

public class RecipeTagQueryServiceTests
{
    public class GetAllAsync
    {
        private readonly GetAllAsyncFixture _fixture = new();

        [Fact]
        public async Task GetAllAsync_RepositoryReturnsExpectedResult_ResultShouldBeAsExpected()
        {
            // Arrange
            _fixture.Setup();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class GetAllAsyncFixture : RecipeTagQueryServiceFixture
        {
            public IReadOnlyCollection<IRecipeTag>? ExpectedResult { get; private set; }

            public void Setup()
            {
                CreateExpectedResult();
                SetupRepositoryReturningExpectedResult();
            }

            private void CreateExpectedResult()
            {
                ExpectedResult = new RecipeTagBuilder().CreateMany(2).ToList();
            }

            private void SetupRepositoryReturningExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                RepositoryMock.SetupFindAllAsync(ExpectedResult);
            }
        }
    }

    private abstract class RecipeTagQueryServiceFixture
    {
        protected readonly RecipeTagRepositoryMock RepositoryMock = new(MockBehavior.Strict);

        public RecipeTagQueryService CreateSut()
        {
            return new RecipeTagQueryService(RepositoryMock.Object);
        }
    }
}