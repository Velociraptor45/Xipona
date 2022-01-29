using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;
using ShoppingList.Api.Core.TestKit.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Models;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Ports;
using ShoppingList.Api.Domain.TestKit.Shared;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Manufacturers.Services
{
    public class ManufacturerValidationServiceTests
    {
        [Fact]
        public async Task ValidateAsync_WithInvalidManufacturerId_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateSut();

            local.SetupManufacturer();
            local.SetupFindingNoManufacturer();

            // Act
            Func<Task> function = async () => await service.ValidateAsync(local.Manufacturer.Id, default);

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
            var local = new LocalFixture();
            var service = local.CreateSut();

            local.SetupManufacturer();
            local.SetupFindingManufacturer();

            // Act
            Func<Task> function = async () => await service.ValidateAsync(local.Manufacturer.Id, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().NotThrowAsync();
            }
        }

        private class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public ManufacturerRepositoryMock ManufacturerRepositoryMock { get; }

            public Manufacturer Manufacturer { get; private set; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                ManufacturerRepositoryMock = new ManufacturerRepositoryMock(MockBehavior.Strict);
            }

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
                ManufacturerRepositoryMock.SetupFindByAsync(Manufacturer.Id, Manufacturer);
            }

            public void SetupFindingNoManufacturer()
            {
                ManufacturerRepositoryMock.SetupFindByAsync(Manufacturer.Id, null);
            }

            #endregion Mock Setup
        }
    }
}