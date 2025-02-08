using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Net;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.Common;

public abstract class EndpointCommandWithReturnTypeTestsBase<TCommandConverterInputType, TCommand, TCommandReturnType, TReturnType, TFixture>
    : EndpointCommandTestsBase<TCommandConverterInputType, TCommand, TCommandReturnType, TFixture>
    where TCommand : ICommand<TCommandReturnType>
    where TFixture : EndpointCommandWithReturnTypeTestsBase<TCommandConverterInputType, TCommand, TCommandReturnType, TReturnType, TFixture>
    .EndpointCommandWithReturnTypeFixtureBase
{
    protected EndpointCommandWithReturnTypeTestsBase(TFixture fixture) : base(fixture)
    {
    }

    [SkippableFact]
    public override async Task EndpointCall_WithValidData_ShouldReturnOk()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.OK);

        Skip.If(statusResult is null, $"Status code 200 not relevant for endpoint {Fixture.RoutePattern}");

        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupCommand();
        Fixture.SetupCommandConverter();
        Fixture.SetupDispatcherSuccess();
        Fixture.SetupExpectedResult();
        Fixture.SetupResultConverter();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<Ok<TReturnType>>();
        var okResult = result as Ok<TReturnType>;
        okResult!.Value.Should().BeEquivalentTo(Fixture.ExpectedResult);
    }

    [SkippableFact]
    public async Task EndpointCall_WithValidData_ShouldReturnCreated()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.Created);

        Skip.If(statusResult is null, $"Status code 201 not relevant for endpoint {Fixture.RoutePattern}");

        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupCommand();
        Fixture.SetupCommandConverter();
        Fixture.SetupDispatcherSuccess();
        Fixture.SetupExpectedResult();
        Fixture.SetupResultConverter();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<CreatedAtRoute<TReturnType>>();
        var createdResult = result as CreatedAtRoute<TReturnType>;
        createdResult!.Value.Should().BeEquivalentTo(Fixture.ExpectedResult);
        createdResult.RouteName.Should().StartWith("Get");
    }

    [SkippableFact]
    public void EndpointCall_ShouldHaveCreatedResponseTypeAttribute()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.Created);

        Skip.If(statusResult is null, $"Status code 201 not relevant for endpoint {Fixture.RoutePattern}");

        // Act
        var result = Fixture.GetAllResponseTypeMetadata().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status201Created);

        var created = result.Single(attr => attr.StatusCode == StatusCodes.Status201Created);
        created.Type.Should().Be(typeof(TReturnType));
    }

    public abstract class EndpointCommandWithReturnTypeFixtureBase : EndpointCommandFixtureBase
    {
        protected Mock<IToContractConverter<TCommandReturnType, TReturnType>> ContractConverterMock =
            new(MockBehavior.Strict);

        public TReturnType? ExpectedResult { get; protected set; }
        public override Type OkResultReturnType => typeof(TReturnType);

        public void SetupExpectedResult()
        {
            ExpectedResult = new DomainTestBuilder<TReturnType>().Create();
        }

        public virtual void SetupResultConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(ExpectedCommandResult);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
            ContractConverterMock.Setup(x => x.ToContract(ExpectedCommandResult)).Returns(ExpectedResult);
        }
    }
}