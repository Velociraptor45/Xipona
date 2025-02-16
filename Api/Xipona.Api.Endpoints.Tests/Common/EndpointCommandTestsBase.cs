using AutoFixture.Kernel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.TestKit.Common.Commands;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.Common;

public abstract class EndpointCommandTestsBase<TCommandConverterInputType, TCommand, TCommandReturnType, TFixture>
    where TCommand : ICommand<TCommandReturnType>
    where TFixture : EndpointCommandTestsBase<TCommandConverterInputType, TCommand, TCommandReturnType, TFixture>.EndpointCommandFixtureBase
{
    protected readonly TFixture Fixture;

    protected EndpointCommandTestsBase(TFixture fixture)
    {
        Fixture = fixture;
    }

    [SkippableFact]
    public virtual async Task EndpointCall_WithValidData_ShouldReturnOk()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.OK);

        Skip.If(statusResult is null, $"Status code 200 not relevant for endpoint {Fixture.RoutePattern}");

        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupCommand();
        Fixture.SetupCommandConverter();
        Fixture.SetupDispatcherSuccess();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<Ok>();
    }

    [SkippableFact]
    public async Task EndpointCall_WithValidData_ShouldReturnNoContent()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.NoContent);

        Skip.If(statusResult is null, $"Status code 204 not relevant for endpoint {Fixture.RoutePattern}");

        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupCommand();
        Fixture.SetupCommandConverter();
        Fixture.SetupDispatcherSuccess();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<NoContent>();
    }

    [SkippableFact]
    public async Task EndpointCall_WithMalformedInput_ShouldReturnBadRequest()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.BadRequest);

        Skip.If(statusResult is null, $"Status code 400 not relevant for endpoint {Fixture.RoutePattern}");

        // Arrange
        Fixture.SetupParametersForBadRequest();
        Fixture.SetupExpectedBadRequestMessage();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<BadRequest<string>>();
        var badRequestResult = result as BadRequest<string>;
        badRequestResult!.Value.Should().BeEquivalentTo(Fixture.ExpectedBadRequestMessage);
    }

    [SkippableFact]
    public async Task EndpointCall_WithDomainException_ShouldReturnUnprocessableEntity()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.UnprocessableEntity);

        Skip.If(statusResult is null, $"Status code 422 not relevant for endpoint {Fixture.RoutePattern}");

        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupCommand();
        Fixture.SetupCommandConverter();
        Fixture.SetupDomainException(statusResult.ExcludedErrorCodes);
        Fixture.SetupDomainExceptionInCommandDispatcher();
        Fixture.SetupExpectedErrorContract();
        Fixture.SetupErrorConversion();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<UnprocessableEntity<ErrorContract>>();
        var unprocessableEntity = result as UnprocessableEntity<ErrorContract>;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    #region ResponseTypes

    [SkippableFact]
    public void EndpointCall_ShouldHaveUnprocessableEntityResponseTypeMetadata()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.UnprocessableEntity);

        Skip.If(statusResult is null, $"Status code 422 not relevant for endpoint {Fixture.RoutePattern}");

        // Act
        var result = Fixture.GetAllResponseTypeMetadata().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status422UnprocessableEntity);

        var unprocessableEntity = result.Single(attr => attr.StatusCode == StatusCodes.Status422UnprocessableEntity);
        unprocessableEntity.Type.Should().Be<ErrorContract>();
    }

    [SkippableFact]
    public void EndpointCall_ShouldHaveOkResponseTypeMetadata()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.OK);

        Skip.If(statusResult is null, $"Status code 200 not relevant for endpoint {Fixture.RoutePattern}");

        // Act
        var result = Fixture.GetAllResponseTypeMetadata().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status200OK);

        var ok = result.Single(attr => attr.StatusCode == StatusCodes.Status200OK);
        ok.Type.Should().Be(Fixture.OkResultReturnType);
    }

    [SkippableFact]
    public void EndpointCall_ShouldHaveNotFoundResponseTypeMetadata()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.NotFound);

        Skip.If(statusResult is null, $"Status code 404 not relevant for endpoint {Fixture.RoutePattern}");

        // Act
        var result = Fixture.GetAllResponseTypeMetadata().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status404NotFound);

        var notFound = result.Single(attr => attr.StatusCode == StatusCodes.Status404NotFound);
        notFound.Type.Should().Be<ErrorContract>();
    }

    [SkippableFact]
    public void EndpointCall_ShouldHaveBadRequestResponseTypeMetadata()
    {
        var statusResult =
            Fixture.PossibleResults.SingleOrDefault(r => r.StatusCode == HttpStatusCode.BadRequest);

        Skip.If(statusResult is null, $"Status code 400 not relevant for endpoint {Fixture.RoutePattern}");

        // Act
        var result = Fixture.GetAllResponseTypeMetadata().ToList();

        // Assert
        result.Should().ContainSingle(attr => attr.StatusCode == StatusCodes.Status400BadRequest);

        var badRequest = result.Single(attr => attr.StatusCode == StatusCodes.Status400BadRequest);
        badRequest.Type.Should().Be<string>();
    }

    #endregion ResponseTypes

    public abstract class EndpointCommandFixtureBase
    {
        protected CommandDispatcherMock CommandDispatcherMock = new(MockBehavior.Strict);

        protected Mock<IToContractConverter<IReason, ErrorContract>> ErrorConverterMock = new(MockBehavior.Strict);

        protected Mock<IToDomainConverter<TCommandConverterInputType, TCommand>> CommandConverterMock =
            new(MockBehavior.Strict);

        protected List<IStatusResult> PossibleResultsList = [];
        public abstract string RoutePattern { get; }
        public abstract HttpMethod HttpMethod { get; }

        public TCommand? Command { get; protected set; }
        public TCommandReturnType? ExpectedCommandResult = new DomainTestBuilder<TCommandReturnType>().Create();
        public DomainException? DomainException { get; private set; }
        public ErrorContract? ExpectedErrorContract { get; private set; }
        public string ExpectedBadRequestMessage { get; protected set; } = string.Empty;
        public IReadOnlyCollection<IStatusResult> PossibleResults => PossibleResultsList;

        public virtual Type OkResultReturnType => typeof(void);

        public abstract Task<IResult> ExecuteTestMethod();

        public abstract void SetupParameters();
        public abstract void RegisterEndpoints(WebApplication app);

        public abstract TCommandConverterInputType GetCommandConverterInput();

        public virtual void SetupParametersForBadRequest()
        {
        }

        public void SetupDispatcherSuccess()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            CommandDispatcherMock.SetupDispatchAsync(Command, ExpectedCommandResult);
        }

        public virtual void SetupCommand()
        {
            Command = new DomainTestBuilder<TCommand>().Create();
        }

        public void SetupDomainExceptionInCommandDispatcher()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainException);
            TestPropertyNotSetException.ThrowIfNull(Command);
            CommandDispatcherMock.SetupDispatchAsync(Command).ThrowsAsync(DomainException);
        }

        public virtual void SetupCommandConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            CommandConverterMock.Setup(x => x.ToDomain(GetCommandConverterInput())).Returns(Command);
        }

        public IEnumerable<ProducesResponseTypeMetadata> GetAllResponseTypeMetadata()
        {
            var builder = WebApplication.CreateSlimBuilder();
            using var app = builder.Build();
            RegisterEndpoints(app);

            var field = typeof(WebApplication).GetField("_dataSources", BindingFlags.Instance | BindingFlags.NonPublic)!;
            var value = (List<EndpointDataSource>)field.GetValue(app)!;

            var httpMethod = HttpMethod.ToString().ToUpper();

            return value[0].Endpoints
                .First(e => ((RouteEndpoint)e).RoutePattern.RawText == RoutePattern && e.DisplayName!.Contains(httpMethod))
                .Metadata
                .Where(m => m is ProducesResponseTypeMetadata rtm && (rtm.StatusCode != 200 || rtm.Type != typeof(IResult)))
                .Cast<ProducesResponseTypeMetadata>()
                .ToList();
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
            TestPropertyNotSetException.ThrowIfNull(ExpectedErrorContract);
            ErrorConverterMock.Setup(x => x.ToContract(DomainException.Reason)).Returns(ExpectedErrorContract);
        }
    }
}