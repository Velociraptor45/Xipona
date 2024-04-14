using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Commands;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Repositories.RecipeTags.Contexts;
using ProjectHermes.Xipona.Api.Repositories.RecipeTags.Entities;
using ProjectHermes.Xipona.Api.TestTools.AutoFixture;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System;
using Xunit;

namespace ProjectHermes.Xipona.Api.Endpoint.IntegrationTests.v1.Controllers;

public class RecipeTagControllerIntegrationTests
{
    public sealed class CreateRecipeTagAsync : IAssemblyFixture<DockerFixture>
    {
        private readonly CreateRecipeTagAsyncFixture _fixture;

        public CreateRecipeTagAsync(DockerFixture dockerFixture)
        {
            _fixture = new CreateRecipeTagAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task CreateRecipeTagAsync_WithValidContract_ShouldReturnCreatedResult()
        {
            // Arrange
            await _fixture.SetupDatabase();
            _fixture.SetupExpectedResult();
            _fixture.SetupContract();
            _fixture.SetupExpectedEntity();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedContract);

            // Act
            var result = await sut.CreateRecipeTagAsync(_fixture.Contract);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CreatedAtActionResult>();

            var createdResult = (CreatedAtActionResult)result;
            createdResult.Value.Should().NotBeNull();
            createdResult.Value.Should().BeOfType<RecipeTagContract>();

            var createdContract = (RecipeTagContract)createdResult.Value!;
            createdContract.Should().BeEquivalentTo(_fixture.ExpectedContract,
                opt => opt.Excluding(info => info.Path == "Id"));

            using var assertionScope = _fixture.CreateServiceScope();
            var allEntities = (await _fixture.LoadAllRecipeTagsAsync(assertionScope)).ToList();
            allEntities.Should().HaveCount(1);

            var entity = allEntities.Single();
            entity.Should().BeEquivalentTo(_fixture.ExpectedEntity,
                               opt => opt
                                   .ExcludeRowVersion()
                                   .Excluding(info => info.Path == "Id" || info.Path == "CreatedAt"));
            entity.Id.Should().Be(createdContract.Id);
            entity.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(30));
        }

        private sealed class CreateRecipeTagAsyncFixture : RecipeTagControllerFixture
        {
            public CreateRecipeTagAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public CreateRecipeTagContract? Contract { get; private set; }
            public RecipeTagContract? ExpectedContract { get; private set; }
            public RecipeTag? ExpectedEntity { get; private set; }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedContract);
                Contract = new CreateRecipeTagContract(ExpectedContract.Name);
            }

            public void SetupExpectedResult()
            {
                ExpectedContract = new DomainTestBuilder<RecipeTagContract>().Create();
            }

            public void SetupExpectedEntity()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedContract);

                ExpectedEntity = new RecipeTag()
                {
                    Name = ExpectedContract.Name
                };
            }

            public async Task SetupDatabase()
            {
                await ApplyMigrationsAsync(ArrangeScope);
            }
        }
    }

    private abstract class RecipeTagControllerFixture : DatabaseFixture
    {
        protected readonly IServiceScope ArrangeScope;

        protected RecipeTagControllerFixture(DockerFixture dockerFixture) : base(dockerFixture)
        {
            ArrangeScope = CreateServiceScope();
        }

        public RecipeTagController CreateSut()
        {
            var scope = CreateServiceScope();
            return scope.ServiceProvider.GetRequiredService<RecipeTagController>();
        }

        public override IEnumerable<DbContext> GetDbContexts(IServiceScope scope)
        {
            yield return scope.ServiceProvider.GetRequiredService<RecipeTagContext>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ArrangeScope.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}