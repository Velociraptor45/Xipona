﻿using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.SearchItems;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Shared;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ItemControllerTests;

public class SearchItemsAsyncTests :
    ControllerEnumerableQueryTestsBase<ItemController, SearchItemQuery, SearchItemResultReadModel, SearchItemResultContract,
    SearchItemsAsyncTests.SearchItemsAsyncFixture>
{
    public SearchItemsAsyncTests() : base(new SearchItemsAsyncFixture())
    {
    }

    public sealed class SearchItemsAsyncFixture : ControllerEnumerableQueryFixtureBase
    {
        private string? _searchString;

        public SearchItemsAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override MethodInfo Method =>
            typeof(ItemController).GetMethod(nameof(ItemController.SearchItemsAsync))!;

        public override ItemController CreateSut()
        {
            return new ItemController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ItemController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_searchString);
            return await sut.SearchItemsAsync(_searchString);
        }

        public override void SetupParameters()
        {
            _searchString = new TestBuilder<string>().Create();
        }

        public override void SetupQuery()
        {
            TestPropertyNotSetException.ThrowIfNull(_searchString);

            Query = new SearchItemQuery(_searchString);
        }
    }
}