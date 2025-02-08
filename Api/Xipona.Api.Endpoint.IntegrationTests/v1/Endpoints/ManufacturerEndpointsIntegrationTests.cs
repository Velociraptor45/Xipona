using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.DeleteManufacturer;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Repositories.Items.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Manufacturers.Contexts;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System;
using Xunit;
using Item = ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item;
using Manufacturer = ProjectHermes.Xipona.Api.Repositories.Manufacturers.Entities.Manufacturer;
using Models = ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.IntegrationTests.v1.Endpoints;

public class ManufacturerEndpointsIntegrationTests
{
    public class DeleteManufacturerAsync : IAssemblyFixture<DockerFixture>
    {
        private readonly DeleteManufacturerAsyncFixture _fixture;

        public DeleteManufacturerAsync(DockerFixture dockerFixture)
        {
            _fixture = new DeleteManufacturerAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task DeleteManufacturerAsync_WithValidData_ShouldReturnOkResult()
        {
            // Arrange
            _fixture.SetupManufacturerId();
            await _fixture.PrepareDatabaseAsync();

            // Act
            var response = await _fixture.ActAsync();

            // Assert
            response.Should().BeOfType<NoContent>();
        }

        [Fact]
        public async Task DeleteManufacturerAsync_WithValidData_ShouldDeleteManufacturer()
        {
            // Arrange
            _fixture.SetupManufacturerId();
            await _fixture.PrepareDatabaseAsync();

            // Act
            await _fixture.ActAsync();

            // Assert
            var manufacturers = await _fixture.LoadPersistedManufacturersAsync();

            manufacturers.Should().HaveCount(1);
            var manufacturer = manufacturers.First();
            manufacturer.Deleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteManufacturerAsync_WithValidData_ShouldRemoveManufacturerIdFromItems()
        {
            // Arrange
            _fixture.SetupManufacturerId();
            await _fixture.PrepareDatabaseAsync();

            // Act
            await _fixture.ActAsync();

            // Assert
            var items = await _fixture.LoadPersistedItemsAsync();

            items.Should().HaveCount(2);
            foreach (var item in items)
            {
                item.ManufacturerId.Should().BeNull();
            }
        }

        [Fact]
        public async Task DeleteManufacturerAsync_WithManufacturerNotExisting_ShouldReturnOk()
        {
            // Arrange
            _fixture.SetupManufacturerId();
            await _fixture.PrepareDatabaseForManufacturerNotExistingAsync();

            // Act
            var response = await _fixture.ActAsync();

            // Assert
            response.Should().BeOfType<NoContent>();
        }

        private class DeleteManufacturerAsyncFixture : ManufacturerControllerFixture
        {
            public DeleteManufacturerAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public Models.ManufacturerId? ManufacturerId { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(ManufacturerId);

                var scope = CreateServiceScope();
                return await ManufacturerEndpoints.DeleteManufacturer(
                    ManufacturerId.Value,
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    scope.ServiceProvider.GetRequiredService<IToDomainConverter<Guid, DeleteManufacturerCommand>>(),
                    default);
            }

            public void SetupManufacturerId()
            {
                ManufacturerId = Models.ManufacturerId.New;
            }

            public async Task PrepareDatabaseForManufacturerNotExistingAsync()
            {
                await ApplyMigrationsAsync(SetupScope);
            }

            public override async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(ManufacturerId);

                using var transaction = await CreateTransactionAsync(SetupScope);
                await ApplyMigrationsAsync(SetupScope);

                // manufacturer
                var manufacturerRepository = CreateManufacturerRepository(SetupScope);
                var manufacturer = new ManufacturerBuilder()
                    .WithIsDeleted(false)
                    .WithId(ManufacturerId.Value)
                    .Create();

                await manufacturerRepository.StoreAsync(manufacturer);

                // items
                var itemRepository = CreateItemRepository(SetupScope);
                var items = new List<IItem>()
                {
                    ItemMother.Initial()
                        .WithManufacturerId(ManufacturerId)
                        .AsItem()
                        .Create(),
                    ItemMother.InitialWithTypes()
                        .WithManufacturerId(ManufacturerId)
                        .Create()
                };

                foreach (var item in items)
                {
                    await itemRepository.StoreAsync(item);
                }

                await transaction.CommitAsync(default);
            }
        }
    }

    private abstract class ManufacturerControllerFixture : DatabaseFixture
    {
        protected readonly IServiceScope SetupScope;

        protected ManufacturerControllerFixture(DockerFixture dockerFixture) : base(dockerFixture)
        {
            SetupScope = CreateServiceScope();
        }

        public override IEnumerable<DbContext> GetDbContexts(IServiceScope scope)
        {
            yield return scope.ServiceProvider.GetRequiredService<ItemContext>();
            yield return scope.ServiceProvider.GetRequiredService<ManufacturerContext>();
        }

        protected ManufacturerContext CreateManufacturerContext(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<ManufacturerContext>();
        }

        protected ItemContext CreateItemContext(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<ItemContext>();
        }

        protected IManufacturerRepository CreateManufacturerRepository(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<Func<CancellationToken, IManufacturerRepository>>()(default);
        }

        protected IItemRepository CreateItemRepository(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<Func<CancellationToken, IItemRepository>>()(default);
        }

        public async Task<IList<Manufacturer>> LoadPersistedManufacturersAsync()
        {
            using var scope = CreateServiceScope();
            var ctx = CreateManufacturerContext(scope);

            using (await CreateTransactionAsync(scope))
            {
                var entities = await ctx.Manufacturers.AsNoTracking()
                    .ToListAsync();

                return entities;
            }
        }

        public async Task<IList<Item>> LoadPersistedItemsAsync()
        {
            using var scope = CreateServiceScope();
            var ctx = CreateItemContext(scope);

            using (await CreateTransactionAsync(scope))
            {
                var entities = await ctx.Items.AsNoTracking()
                    .ToListAsync();

                return entities;
            }
        }

        public abstract Task PrepareDatabaseAsync();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SetupScope.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}