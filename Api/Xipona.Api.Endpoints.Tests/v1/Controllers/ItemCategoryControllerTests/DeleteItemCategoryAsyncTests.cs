using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.DeleteItemCategory;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
using ProjectHermes.Xipona.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Controllers.ItemCategoryControllerTests;

public class DeleteItemCategoryAsyncTests : EndpointCommandTestsBase<Guid, DeleteItemCategoryCommand,
    bool, DeleteItemCategoryAsyncTests.DeleteItemCategoryAsyncFixture>
{
    public DeleteItemCategoryAsyncTests() : base(new DeleteItemCategoryAsyncFixture())
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
        var unprocessableEntity = result as NotFound<ErrorContract>;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class DeleteItemCategoryAsyncFixture : EndpointCommandFixtureBase
    {
        private Guid? _id;

        public DeleteItemCategoryAsyncFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>
            {
                ErrorReasonCode.ItemNotFound
            }));
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override string RoutePattern => "/v1/item-categories/{id:guid}";

        public override async Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_id);
            return await MinimalItemCategoryController.DeleteItemCategory(
                _id.Value,
                CommandDispatcherMock.Object,
                ErrorConverterMock.Object,
                CommandConverterMock.Object,
                default);
        }

        public override void SetupParameters()
        {
            _id = new DomainTestBuilder<Guid>().Create();
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterItemCategoryEndpoints();
        }

        public override void SetupCommand()
        {
            Command = new DomainTestBuilder<DeleteItemCategoryCommand>().Create();
        }

        public override Guid GetCommandConverterInput()
        {
            TestPropertyNotSetException.ThrowIfNull(_id);
            return _id.Value;
        }
    }
}