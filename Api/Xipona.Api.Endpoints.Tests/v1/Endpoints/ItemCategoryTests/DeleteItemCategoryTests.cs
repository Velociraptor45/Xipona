using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Xipona.Api.ApplicationServices.ItemCategories.Commands.DeleteItemCategory;
using Xipona.Api.Contracts.Common;
using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;
using Xipona.Api.TestTools.Exceptions;
using System.Net.Http;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.ItemCategoryEndpointTests;

public class DeleteItemCategoryTests : EndpointCommandTestsBase<Guid, DeleteItemCategoryCommand,
    bool, DeleteItemCategoryTests.DeleteItemCategoryFixture>
{
    public DeleteItemCategoryTests() : base(new DeleteItemCategoryFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.ItemCategoryNotFound)]
    public async Task EndpointCall_WithDomainException_ShouldReturnNotFound(ErrorReasonCode errorCode)
    {
        // Arrange
        Fixture.SetupCommand();
        Fixture.SetupParameters();
        Fixture.SetupCommandConverter();
        Fixture.SetupDomainException(errorCode);
        Fixture.SetupDomainExceptionInCommandDispatcher();
        Fixture.SetupExpectedErrorContract();
        Fixture.SetupErrorConversion();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<NotFound<ErrorContract>>();
        var notFound = result as NotFound<ErrorContract>;
        notFound!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class DeleteItemCategoryFixture : EndpointCommandFixtureBase
    {
        private Guid? _id;

        public DeleteItemCategoryFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>
            {
                ErrorReasonCode.ItemNotFound
            }));
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override string RoutePattern => "/v1/item-categories/{id:guid}";
        public override HttpMethod HttpMethod => HttpMethod.Delete;

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_id);
            return await ItemCategoryEndpoints.DeleteItemCategory(
                _id.Value,
                CommandDispatcherMock.Object,
                ErrorConverterMock.Object,
                CommandConverterMock.Object,
                TestContext.Current.CancellationToken);
        }

        public override void SetupParameters()
        {
            _id = new DomainTestBuilder<Guid>().Create();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterItemCategoryEndpoints();
        }

        public override Guid GetCommandConverterInput()
        {
            TestPropertyNotSetException.ThrowIfNull(_id);
            return _id.Value;
        }
    }
}