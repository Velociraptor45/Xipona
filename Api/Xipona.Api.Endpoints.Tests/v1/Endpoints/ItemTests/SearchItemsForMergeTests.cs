using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Xipona.Api.ApplicationServices.Items.Queries.SearchItemsForMerge;
using Xipona.Api.Contracts.Items.Queries.SearchItemsForMerge;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Searches;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.ItemTests;

public class SearchItemsForMergeTests
{
    public class WithoutItemQuantityInPacket : EndpointEnumerableQueryNoConverterTestsBase<
        SearchItemsForMergeQuery, SearchItemsForMergeResult, SearchItemsForMergeResultContract,
        WithoutItemQuantityInPacket.SearchItemsForMergeFixture>
    {
        public WithoutItemQuantityInPacket() : base(new SearchItemsForMergeFixture())
        {
        }

        public sealed class SearchItemsForMergeFixture : EndpointEnumerableQueryNoConverterFixtureBase
        {
            private readonly Guid _itemCategoryId = Guid.NewGuid();
            private readonly Guid _manufacturerId = Guid.NewGuid();
            private readonly QuantityType _quantityType = QuantityType.Weight;
            private float? _quantity;
            private readonly QuantityTypeInPacket? _quantityTypeInPacket = null;
            private readonly Guid[] _excludedItemIds = new DomainTestBuilder<Guid>().CreateMany(3).ToArray();

            public SearchItemsForMergeFixture()
            {
                PossibleResultsList.Add(new OkStatusResult());
                PossibleResultsList.Add(new NoContentStatusResult());
                PossibleResultsList.Add(new BadRequestStatusResult());
                PossibleResultsList.Add(new UnprocessableEntityStatusResult());
            }
            
            public override string RoutePattern => "/v1/items/merge/search";
            
            public override async Task<IResult> ExecuteTestMethod()
            {
                return await ItemEndpoints.SearchItemsForMerge(
                    _itemCategoryId,
                    _manufacturerId,
                    _quantityType.ToInt(),
                    _quantity,
                    _quantityTypeInPacket?.ToInt(),
                    _excludedItemIds,
                    QueryDispatcherMock.Object,
                    ContractConverterMock.Object,
                    ErrorConverterMock.Object,
                    CancellationToken.None);
            }

            public override void SetupParameters()
            {
            }

            public override void RegisterEndpoints(WebApplication app)
            {
                app.RegisterItemEndpoints();
            }

            public override void SetupQuery()
            {
                Query = new SearchItemsForMergeQuery(
                    new(_itemCategoryId),
                    new(_manufacturerId),
                    new ItemQuantity(_quantityType, null),
                    _excludedItemIds.Select(i => new ItemId(i)).ToList());
            }

            public override void SetupParametersForBadRequest()
            {
                _quantity = new DomainTestBuilder<float>().Create();
            }

            public override void SetupExpectedBadRequestMessage()
            {
                ExpectedBadRequestMessage =
                    "Quantity and QuantityTypeInPacket must either both be null or both not be null";
            }
        }
    }

    public class WithItemQuantityInPacket : EndpointEnumerableQueryNoConverterTestsBase<
        SearchItemsForMergeQuery, SearchItemsForMergeResult, SearchItemsForMergeResultContract,
        WithItemQuantityInPacket.SearchItemsForMergeFixture>
    {
        public WithItemQuantityInPacket() : base(new SearchItemsForMergeFixture())
        {
        }

        public sealed class SearchItemsForMergeFixture : EndpointEnumerableQueryNoConverterFixtureBase
        {
            private readonly Guid _itemCategoryId = Guid.NewGuid();
            private readonly Guid _manufacturerId = Guid.NewGuid();
            private readonly QuantityType _quantityType = QuantityType.Unit;
            private float? _quantity = new DomainTestBuilder<float>().Create();
            private readonly QuantityTypeInPacket? _quantityTypeInPacket = QuantityTypeInPacket.Unit;
            private readonly Guid[] _excludedItemIds = new DomainTestBuilder<Guid>().CreateMany(3).ToArray();

            public SearchItemsForMergeFixture()
            {
                PossibleResultsList.Add(new OkStatusResult());
                PossibleResultsList.Add(new NoContentStatusResult());
                PossibleResultsList.Add(new BadRequestStatusResult());
                PossibleResultsList.Add(new UnprocessableEntityStatusResult());
            }
            
            public override string RoutePattern => "/v1/items/merge/search";
            
            public override async Task<IResult> ExecuteTestMethod()
            {
                return await ItemEndpoints.SearchItemsForMerge(
                    _itemCategoryId,
                    _manufacturerId,
                    _quantityType.ToInt(),
                    _quantity,
                    _quantityTypeInPacket?.ToInt(),
                    _excludedItemIds,
                    QueryDispatcherMock.Object,
                    ContractConverterMock.Object,
                    ErrorConverterMock.Object,
                    CancellationToken.None);
            }

            public override void SetupParameters()
            {
            }

            public override void RegisterEndpoints(WebApplication app)
            {
                app.RegisterItemEndpoints();
            }

            public override void SetupQuery()
            {
                Query = new SearchItemsForMergeQuery(
                    new(_itemCategoryId),
                    new(_manufacturerId),
                    new ItemQuantity(_quantityType,
                        new ItemQuantityInPacket(new Quantity(_quantity!.Value), _quantityTypeInPacket!.Value)),
                    _excludedItemIds.Select(i => new ItemId(i)).ToList());
            }

            public override void SetupParametersForBadRequest()
            {
                _quantity = null;
            }

            public override void SetupExpectedBadRequestMessage()
            {
                ExpectedBadRequestMessage =
                    "Quantity and QuantityTypeInPacket must either both be null or both not be null";
            }
        }
    }
}