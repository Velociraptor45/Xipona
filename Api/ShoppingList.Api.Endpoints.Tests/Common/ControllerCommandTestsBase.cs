using AutoFixture.Kernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.TestKit.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.TestKit.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.TestKit.v1.Converters;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System.Net;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;

public abstract class ControllerCommandTestsBase<TController, TCommand, TCommandReturnType, TFixture>
    where TController : ControllerBase
    where TCommand : ICommand<TCommandReturnType>
    where TFixture : ControllerCommandTestsBase<TController, TCommand, TCommandReturnType, TFixture>
    .ControllerCommandFixtureBase
{
    protected readonly TFixture Fixture;

    protected ControllerCommandTestsBase(TFixture fixture)
    {
        Fixture = fixture;
    }

    [SkippableFact]
    public async Task EndpointCall_WithValidData_ShouldReturnOk()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.OK);

        Skip.If(statusResult is null, $"Status code 200 not relevant for endpoint {Fixture.Method.Name}");

        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupCommand();
        Fixture.SetupCommandConverter();
        Fixture.SetupCommandResult(HttpStatusCode.OK);
        Fixture.SetupDispatcherSuccess();
        var sut = Fixture.CreateSut();

        // Act
        var result = await Fixture.ExecuteTestMethod(sut);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [SkippableFact]
    public async Task EndpointCall_WithValidData_ShouldReturnNoContent()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.NoContent);

        Skip.If(statusResult is null, $"Status code 204 not relevant for endpoint {Fixture.Method.Name}");

        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupCommand();
        Fixture.SetupCommandConverter();
        Fixture.SetupCommandResult(HttpStatusCode.OK);
        Fixture.SetupDispatcherSuccess();
        var sut = Fixture.CreateSut();

        // Act
        var result = await Fixture.ExecuteTestMethod(sut);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [SkippableFact]
    public async Task EndpointCall_WithMalformedInput_ShouldReturnBadRequest()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.BadRequest);

        Skip.If(statusResult is null, $"Status code 400 not relevant for endpoint {Fixture.Method.Name}");

        // Arrange
        Fixture.SetupParametersForBadRequest();
        Fixture.SetupExpectedBadRequestMessage();
        var sut = Fixture.CreateSut();

        // Act
        var result = await Fixture.ExecuteTestMethod(sut);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().BeEquivalentTo(Fixture.ExpectedBadRequestMessage);
    }

    [SkippableFact]
    public async Task EndpointCall_WithDomainException_ShouldReturnUnprocessableEntity()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.UnprocessableEntity);

        Skip.If(statusResult is null, $"Status code 422 not relevant for endpoint {Fixture.Method.Name}");

        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupCommand();
        Fixture.SetupCommandConverter();
        Fixture.SetupDomainException(statusResult.ExcludedErrorCodes);
        Fixture.SetupDomainExceptionInCommandDispatcher();
        Fixture.SetupExpectedErrorContract();
        Fixture.SetupErrorConversion();
        var sut = Fixture.CreateSut();

        // Act
        var result = await Fixture.ExecuteTestMethod(sut);

        // Assert
        result.Should().BeOfType<UnprocessableEntityObjectResult>();
        var unprocessableEntity = result as UnprocessableEntityObjectResult;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    #region ResponseTypes

    [SkippableFact]
    public void EndpointCall_ShouldHaveUnprocessableEntityResponseTypeAttribute()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.UnprocessableEntity);

        Skip.If(statusResult is null, $"Status code 422 not relevant for endpoint {Fixture.Method.Name}");

        // Act
        var result = Fixture.GetAllResponseTypeAttributes().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status422UnprocessableEntity);

        var unprocessableEntityAttribute =
            result.Single(attr => attr.StatusCode == StatusCodes.Status422UnprocessableEntity);
        unprocessableEntityAttribute.Type.Should().Be(typeof(ErrorContract));
    }

    [SkippableFact]
    public void EndpointCall_ShouldHaveOkResponseTypeAttribute()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.OK);

        Skip.If(statusResult is null, $"Status code 200 not relevant for endpoint {Fixture.Method.Name}");

        // Act
        var result = Fixture.GetAllResponseTypeAttributes().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status200OK);

        var okEntityAttribute =
            result.Single(attr => attr.StatusCode == StatusCodes.Status200OK);
        okEntityAttribute.Type.Should().Be(typeof(void));
    }

    [SkippableFact]
    public void EndpointCall_ShouldHaveNotFoundResponseTypeAttribute()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.NotFound);

        Skip.If(statusResult is null, $"Status code 404 not relevant for endpoint {Fixture.Method.Name}");

        // Act
        var result = Fixture.GetAllResponseTypeAttributes().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status404NotFound);

        var notFoundEntityAttribute =
            result.Single(attr => attr.StatusCode == StatusCodes.Status404NotFound);
        notFoundEntityAttribute.Type.Should().Be(typeof(ErrorContract));
    }

    [SkippableFact]
    public void EndpointCall_ShouldHaveBadRequestResponseTypeAttribute()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.BadRequest);

        Skip.If(statusResult is null, $"Status code 400 not relevant for endpoint {Fixture.Method.Name}");

        // Act
        var result = Fixture.GetAllResponseTypeAttributes().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status400BadRequest);

        var unprocessableEntityAttribute =
            result.Single(attr => attr.StatusCode == StatusCodes.Status400BadRequest);
        unprocessableEntityAttribute.Type.Should().Be(typeof(string));
    }

    #endregion ResponseTypes

    public abstract class ControllerCommandFixtureBase
    {
        protected QueryDispatcherMock QueryDispatcherMock = new(MockBehavior.Strict);
        protected CommandDispatcherMock CommandDispatcherMock = new(MockBehavior.Strict);
        protected EndpointConvertersMock EndpointConvertersMock = new(MockBehavior.Strict);
        protected List<IStatusResult> PossibleResultsList = new();
        public abstract MethodInfo Method { get; }

        public TCommand? Command { get; protected set; }
        public TCommandReturnType? ExpectedCommandResult { get; protected set; }
        public DomainException? DomainException { get; private set; }
        public ErrorContract? ExpectedErrorContract { get; private set; }
        public string ExpectedBadRequestMessage { get; protected set; } = string.Empty;
        public IReadOnlyCollection<IStatusResult> PossibleResults => PossibleResultsList;

        public abstract TController CreateSut();

        public abstract Task<IActionResult> ExecuteTestMethod(TController sut);

        public abstract void SetupParameters();

        public virtual void SetupParametersForBadRequest()
        {
        }

        public void SetupDispatcherSuccess()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            TestPropertyNotSetException.ThrowIfNull(ExpectedCommandResult);
            CommandDispatcherMock.SetupDispatchAsync(Command, ExpectedCommandResult);
        }

        public abstract void SetupCommand();

        public void SetupDomainExceptionInCommandDispatcher()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainException);
            TestPropertyNotSetException.ThrowIfNull(Command);
            CommandDispatcherMock.SetupDispatchAsync(Command).ThrowsAsync(DomainException);
        }

        public virtual void SetupCommandConverter()
        {
        }

        public IEnumerable<ProducesResponseTypeAttribute> GetAllResponseTypeAttributes()
        {
            TestPropertyNotSetException.ThrowIfNull(Method);

            return Method
                .GetCustomAttributes(typeof(ProducesResponseTypeAttribute), true)
                .OfType<ProducesResponseTypeAttribute>();
        }

        public void SetupExpectedErrorContract()
        {
            ExpectedErrorContract = new DomainTestBuilder<ErrorContract>().Create();
        }

        public virtual void SetupExpectedBadRequestMessage()
        {
        }

        public void SetupDomainException(IEnumerable<ErrorReasonCode> exclude)
        {
            var builder = new DomainTestBuilder<DomainException>();
            builder.Customize(new EnumExclusionCustomization<ErrorReasonCode>(exclude));
            builder.Customizations.Add(new TypeRelay(typeof(IReason), typeof(DummyReason)));

            DomainException = builder.Create();
        }

        public void SetupDomainException(ErrorReasonCode errorCode)
        {
            IReason reason = new TestBuilder<DummyReason>()
                .FillConstructorWith("errorCode", errorCode)
                .Create();
            DomainException = new DomainTestBuilder<DomainException>()
                .FillConstructorWith("reason", reason)
                .Create();
        }

        public void SetupErrorConversion()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainException);
            EndpointConvertersMock.SetupToContract(DomainException.Reason, ExpectedErrorContract);
        }

        public virtual void SetupCommandResult(HttpStatusCode statusCode)
        {
            if (statusCode is HttpStatusCode.OK or HttpStatusCode.Created)
                ExpectedCommandResult = new DomainTestBuilder<TCommandReturnType>().Create();
        }
    }
}