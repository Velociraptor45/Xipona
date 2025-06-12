using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.ApplicationServices.RecipeTags.Commands.CreateRecipeTag;
using Xipona.Api.Contracts.Common;
using Xipona.Api.Contracts.RecipeTags.Commands;
using Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.RecipeTags.Models;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Repositories.RecipeTags.Contexts;
using Xipona.Api.TestTools.AutoFixture;
using Xipona.Api.TestTools.Exceptions;
using System;
using Xunit;
using RecipeTag = Xipona.Api.Repositories.RecipeTags.Entities.RecipeTag;

namespace Xipona.Api.Endpoint.IntegrationTests.v1.Endpoints;

public class RecipeTagEndpointsIntegrationTests
{
    public sealed class CreateRecipeTagAsync
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedContract);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CreatedAtRoute<RecipeTagContract>>();

            var createdResult = (CreatedAtRoute<RecipeTagContract>)result;
            createdResult.Value.Should().NotBeNull();

            var createdContract = createdResult.Value!;
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

        private sealed class CreateRecipeTagAsyncFixture : RecipeTagEndpointFixture
        {
            public CreateRecipeTagAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public CreateRecipeTagContract? Contract { get; private set; }
            public RecipeTagContract? ExpectedContract { get; private set; }
            public RecipeTag? ExpectedEntity { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);

                var scope = CreateServiceScope();
                return await RecipeTagEndpoints.CreateRecipeTag(
                    Contract,
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    scope.ServiceProvider.GetRequiredService<IToDomainConverter<CreateRecipeTagContract, CreateRecipeTagCommand>>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IRecipeTag, RecipeTagContract>>(),
                    default);
            }

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

    private abstract class RecipeTagEndpointFixture : DatabaseFixture
    {
        protected readonly IServiceScope ArrangeScope;

        protected RecipeTagEndpointFixture(DockerFixture dockerFixture) : base(dockerFixture)
        {
            ArrangeScope = CreateServiceScope();
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