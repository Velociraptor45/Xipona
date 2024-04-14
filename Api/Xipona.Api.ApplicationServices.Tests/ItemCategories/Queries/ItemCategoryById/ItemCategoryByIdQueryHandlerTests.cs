﻿using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Queries.ItemCategoryById;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Services.Queries;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.ItemCategories.Queries.ItemCategoryById;

public class ItemCategoryByIdQueryHandlerTests : QueryHandlerTestsBase<ItemCategoryByIdQueryHandler,
    ItemCategoryByIdQuery, IItemCategory>
{
    public ItemCategoryByIdQueryHandlerTests() : base(new ItemCategoryByIdQueryHandlerFixture())
    {
    }

    private class ItemCategoryByIdQueryHandlerFixture : IQueryHandlerBaseFixture
    {
        private readonly ItemCategoryQueryServiceMock _serviceMock = new(MockBehavior.Strict);

        public ItemCategoryByIdQuery? Query { get; private set; }
        public IItemCategory? ExpectedResult { get; private set; }

        public ItemCategoryByIdQueryHandler CreateSut()
        {
            return new ItemCategoryByIdQueryHandler(_ => _serviceMock.Object);
        }

        public void Setup()
        {
            CreateQuery();
            CreateExpectedResult();
            SetupServiceReturningExpectedResult();
        }

        private void CreateQuery()
        {
            Query = new DomainTestBuilder<ItemCategoryByIdQuery>().Create();
        }

        private void CreateExpectedResult()
        {
            ExpectedResult = new ItemCategoryBuilder().Create();
        }

        private void SetupServiceReturningExpectedResult()
        {
            TestPropertyNotSetException.ThrowIfNull(Query);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

            _serviceMock.SetupGetAsync(Query.ItemCategoryId, ExpectedResult);
        }
    }
}