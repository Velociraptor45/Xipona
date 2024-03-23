using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Common;

public abstract class DomainEventHandlerTestsBase<TDomainEvent, TDomainEventHandler>
    where TDomainEvent : IDomainEvent
    where TDomainEventHandler : IDomainEventHandler<TDomainEvent>
{
    private readonly DomainEventHandlerBaseFixture _fixture;

    protected DomainEventHandlerTestsBase(DomainEventHandlerBaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task HandleAsync_WithValidDomainEvent_ShouldCallService()
    {
        // Arrange
        _fixture.SetupDomainEvent();
        _fixture.SetupCallingService();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.DomainEvent);

        // Act
        await sut.HandleAsync(_fixture.DomainEvent, default);

        // Assert
        _fixture.VerifyCallingService();
    }

    protected abstract class DomainEventHandlerBaseFixture
    {
        public TDomainEvent? DomainEvent { get; private set; }

        public abstract TDomainEventHandler CreateSut();

        public void SetupDomainEvent()
        {
            DomainEvent = new DomainTestBuilder<TDomainEvent>().Create();
        }

        public abstract void SetupCallingService();

        public abstract void VerifyCallingService();
    }
}