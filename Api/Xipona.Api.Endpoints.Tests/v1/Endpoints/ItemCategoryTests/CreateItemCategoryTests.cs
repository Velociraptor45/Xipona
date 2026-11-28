using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Xipona.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;
using Xipona.Api.Contracts.Common.Queries;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;
using Xipona.Api.TestTools.Exceptions;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.ItemCategoryEndpointTests;

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
                TestContext.Current.CancellationToken);
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