using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;
using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Net.Http;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ItemCategoryEndpointTests;

public class CreateItemCategoryTests : EndpointCommandWithReturnTypeTestsBase<string,
    CreateItemCategoryCommand, IItemCategory, ItemCategoryContract,
    CreateItemCategoryTests.CreateItemCategoryFixture>
{
    public CreateItemCategoryTests() : base(new CreateItemCategoryFixture())
    {
    }

    public sealed class CreateItemCategoryFixture : EndpointCommandWithReturnTypeFixtureBase
    {
        private string? _name;

        public CreateItemCategoryFixture()
        {
            PossibleResultsList.Add(new CreatedStatusResult());
        }

        public override string RoutePattern => "/v1/item-categories";
        public override HttpMethod HttpMethod => HttpMethod.Post;

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_name);
            return await ItemCategoryEndpoints.CreateItemCategory(
                _name,
                CommandDispatcherMock.Object,
                ContractConverterMock.Object,
                CommandConverterMock.Object,
                default);
        }

        public override void SetupCommand()
        {
            TestPropertyNotSetException.ThrowIfNull(_name);
            Command = new CreateItemCategoryCommand(new ItemCategoryName(_name));
        }

        public override void SetupParameters()
        {
            _name = new DomainTestBuilder<string>().Create();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterItemCategoryEndpoints();
        }

        public override string GetCommandConverterInput()
        {
            TestPropertyNotSetException.ThrowIfNull(_name);
            return _name;
        }
    }
}