using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Manufacturers.Services;

public class ManufacturerValidationServiceTests
{
    private readonly LocalFixture _fixture;

    public ManufacturerValidationServiceTests()
    {
        _fixture = new LocalFixture();
    }

    [Fact]
    public async Task ValidateAsync_WithInvalidManufacturerId_ShouldThrowDomainException()
    {
        // Arrange
        var service = _fixture.CreateSut();

        _fixture.SetupManufacturer();
        _fixture.SetupFindingNoManufacturer();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Manufacturer);

        // Act
        Func<Task> function = async () => await service.ValidateAsync(_fixture.Manufacturer.Id);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ManufacturerNotFound);
        }
    }

    [Fact]
    public async Task ValidateAsync_WithValidManufacturerId_ShouldNotThrow()
    {
        // Arrange
        var service = _fixture.CreateSut();

        _fixture.SetupManufacturer();
        _fixture.SetupFindingManufacturer();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Manufacturer);

        // Act
        Func<Task> function = async () => await service.ValidateAsync(_fixture.Manufacturer.Id);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().NotThrowAsync();
        }
    }

    private class LocalFixture
    {
        public ManufacturerRepositoryMock ManufacturerRepositoryMock { get; } = new(MockBehavior.Strict);

        public Manufacturer? Manufacturer { get; private set; }

        public ManufacturerValidationService CreateSut()
        {
            return new ManufacturerValidationService(ManufacturerRepositoryMock.Object);
        }

        public void SetupManufacturer()
        {
            Manufacturer = new ManufacturerBuilder().Create();
        }

        #region Mock Setup

        public void SetupFindingManufacturer()
        {
            TestPropertyNotSetException.ThrowIfNull(Manufacturer);
            ManufacturerRepositoryMock.SetupFindActiveByAsync(Manufacturer.Id, Manufacturer);
        }

        public void SetupFindingNoManufacturer()
        {
            TestPropertyNotSetException.ThrowIfNull(Manufacturer);
            ManufacturerRepositoryMock.SetupFindActiveByAsync(Manufacturer.Id, null);
        }

        #endregion Mock Setup
    }
}