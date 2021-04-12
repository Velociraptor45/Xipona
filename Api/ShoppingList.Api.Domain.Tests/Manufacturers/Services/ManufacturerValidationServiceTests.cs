using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Fixtures;
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
        public async Task ValidateAsync_WithManufacturerIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            // Act
            Func<Task> function = async () => await service.ValidateAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task ValidateAsync_WithInvalidManufacturerId_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            var manufacturerId = new ManufacturerId(local.CommonFixture.NextInt());

            local.ManufacturerRepositoryMock.SetupFindByAsync(manufacturerId, null);

            // Act
            Func<Task> function = async () => await service.ValidateAsync(manufacturerId, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ManufacturerNotFound);
            }
        }

        [Fact]
        public async Task ValidateAsync_WithValidManufacturerId_ShouldNotThrow()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            var manufacturer = local.CreateManufacturer();

            local.ManufacturerRepositoryMock.SetupFindByAsync(manufacturer.Id, manufacturer);

            // Act
            Func<Task> function = async () => await service.ValidateAsync(manufacturer.Id, default);

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
            public ManufacturerFixture ManufacturerFixture { get; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                ManufacturerRepositoryMock = new ManufacturerRepositoryMock(Fixture);

                ManufacturerFixture = new ManufacturerFixture(CommonFixture);
            }

            public ManufacturerValidationService CreateService()
            {
                return Fixture.Create<ManufacturerValidationService>();
            }

            public IManufacturer CreateManufacturer()
            {
                return ManufacturerFixture.Create();
            }
        }
    }
}