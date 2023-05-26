using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Contexts;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System;
using Xunit;
using Item = ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.Item;
using Manufacturer = ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Entities.Manufacturer;
using Models = ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.IntegrationTests.v1.Controllers;

public class ManufacturerControllerIntegrationTests
{
    [Collection(DockerCollection.Name)]
    public class DeleteManufacturerAsync
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
            var sut = _fixture.CreateSut();

            // Act
            var response = await sut.DeleteManufacturerAsync(_fixture.ManufacturerId!.Value);

            // Assert
            response.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task DeleteManufacturerAsync_WithValidData_ShouldDeleteManufacturer()
        {
            // Arrange
            _fixture.SetupManufacturerId();
            await _fixture.PrepareDatabaseAsync();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteManufacturerAsync(_fixture.ManufacturerId!.Value);

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
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteManufacturerAsync(_fixture.ManufacturerId!.Value);

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
            var sut = _fixture.CreateSut();

            // Act
            var response = await sut.DeleteManufacturerAsync(_fixture.ManufacturerId!.Value);

            // Assert
            response.Should().BeOfType<OkResult>();
        }

        private class DeleteManufacturerAsyncFixture : ManufacturerControllerFixture
        {
            public DeleteManufacturerAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public Models.ManufacturerId? ManufacturerId { get; private set; }

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

        public ManufacturerController CreateSut()
        {
            var scope = CreateServiceScope();
            return scope.ServiceProvider.GetRequiredService<ManufacturerController>();
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