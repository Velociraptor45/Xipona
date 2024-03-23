using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Net;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.Common;

public abstract class ControllerCommandWithReturnTypeTestsBase<TController, TCommand, TCommandReturnType, TReturnType, TFixture>
    : ControllerCommandTestsBase<TController, TCommand, TCommandReturnType, TFixture>
    where TController : ControllerBase
    where TCommand : ICommand<TCommandReturnType>
    where TFixture : ControllerCommandWithReturnTypeTestsBase<TController, TCommand, TCommandReturnType, TReturnType, TFixture>
    .ControllerCommandWithReturnTypeFixtureBase
{
    protected ControllerCommandWithReturnTypeTestsBase(TFixture fixture) : base(fixture)
    {
    }

    [SkippableFact]
    public async Task EndpointCall_WithValidData_ShouldReturnCreated()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.Created);

        Skip.If(statusResult is null, $"Status code 201 not relevant for endpoint {Fixture.Method.Name}");

        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupCommand();
        Fixture.SetupCommandConverter();
        Fixture.SetupCommandResult(HttpStatusCode.Created);
        Fixture.SetupDispatcherSuccess();
        Fixture.SetupExpectedResult();
        Fixture.SetupResultConverter();
        var sut = Fixture.CreateSut();

        // Act
        var result = await Fixture.ExecuteTestMethod(sut);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult!.Value.Should().BeEquivalentTo(Fixture.ExpectedResult);
        createdResult.ActionName.Should().StartWith("Get");
    }

    [SkippableFact]
    public void EndpointCall_ShouldHaveCreatedResponseTypeAttribute()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.Created);

        Skip.If(statusResult is null, $"Status code 201 not relevant for endpoint {Fixture.Method.Name}");

        // Act
        var result = Fixture.GetAllResponseTypeAttributes().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status201Created);

        var createdEntityAttribute =
            result.Single(attr => attr.StatusCode == StatusCodes.Status201Created);
        createdEntityAttribute.Type.Should().Be(typeof(TReturnType));
    }

    public abstract class ControllerCommandWithReturnTypeFixtureBase
        : ControllerCommandTestsBase<TController, TCommand, TCommandReturnType, TFixture>.ControllerCommandFixtureBase
    {
        public TReturnType? ExpectedResult { get; protected set; }

        public void SetupExpectedResult()
        {
            ExpectedResult = new DomainTestBuilder<TReturnType>().Create();
        }

        public virtual void SetupResultConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(ExpectedCommandResult);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
            EndpointConvertersMock.SetupToContract(ExpectedCommandResult, ExpectedResult);
        }
    }
}