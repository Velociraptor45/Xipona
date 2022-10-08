using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Contexts;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Endpoint.IntegrationTests.v1.Controllers;

public class StoreControllerIntegrationTests
{
    [Collection(DockerCollection.Name)]
    public class CreateStoreAsync
    {
        private readonly CreateStoreAsyncFixture _fixture;

        public CreateStoreAsync(DockerFixture dockerFixture)
        {
            _fixture = new CreateStoreAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task CreateStoreAsync_WithValidData_ShouldReturnCorrectResult()
        {
            // Arrange
            await _fixture.PrepareDatabaseAsync();
            _fixture.SetupContract();
            _fixture.SetupExpectedResultValue();
            var sut = _fixture.CreateSut();

            // Act
            var result = await sut.CreateStoreAsync(_fixture.Contract!);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            var createdResult = result as CreatedResult;
            createdResult!.Value.Should().BeOfType<StoreContract>();
            createdResult.Value.Should().BeEquivalentTo(_fixture.ExpectedResultValue!,
                opts => opts.Excluding(x => x.Path.EndsWith("Id")));
        }

        [Fact]
        public async Task CreateStoreAsync_WithValidData_ShouldPersistStore()
        {
            // Arrange
            await _fixture.PrepareDatabaseAsync();
            _fixture.SetupContract();
            _fixture.SetupExpectedPersistedStore();
            var sut = _fixture.CreateSut();

            // Act
            await sut.CreateStoreAsync(_fixture.Contract!);

            // Assert
            var stores = await _fixture.LoadPersistedStoresAsync();

            stores.Should().HaveCount(1);
            stores.First().Should().BeEquivalentTo(_fixture.ExpectedPersistedStore,
                opts => opts.Excluding(x => x.Path.EndsWith("Id")));
        }

        private class CreateStoreAsyncFixture : LocalFixture
        {
            public CreateStoreAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public CreateStoreContract? Contract { get; private set; }
            public StoreContract? ExpectedResultValue { get; private set; }
            public Store? ExpectedPersistedStore { get; private set; }

            public void SetupContract()
            {
                Contract = new CreateStoreContract("MyCoolStore",
                    new List<CreateSectionContract> { new("MyCoolSection", 0, true) });
            }

            public void SetupExpectedResultValue()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);

                var sections = Contract.Sections.Select(section =>
                        new SectionContract(Guid.Empty, section.Name, section.SortingIndex,
                            section.IsDefaultSection))
                    .ToList();

                ExpectedResultValue = new StoreContract(Guid.Empty, Contract.Name, sections);
            }

            public void SetupExpectedPersistedStore()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);

                var sections = new List<ISection>();
                foreach (var section in Contract.Sections)
                {
                    sections.Add(new SectionBuilder()
                        .WithName(new SectionName(section.Name))
                        .WithIsDefaultSection(section.IsDefaultSection)
                        .WithSortingIndex(section.SortingIndex)
                        .Create());
                }

                var factory = SetupScope.ServiceProvider.GetRequiredService<ISectionFactory>();

                ExpectedPersistedStore = new StoreBuilder()
                    .WithName(new StoreName(Contract.Name))
                    .WithIsDeleted(false)
                    .WithSections(new Sections(sections, factory))
                    .Create();
            }

            public override async Task PrepareDatabaseAsync()
            {
                await ApplyMigrationsAsync(SetupScope);
            }
        }
    }

    [Collection(DockerCollection.Name)]
    public class UpdateStoreAsync
    {
        private readonly UpdateStoreAsyncFixture _fixture;

        public UpdateStoreAsync(DockerFixture dockerFixture)
        {
            _fixture = new UpdateStoreAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task UpdateStoreAsync_WithEquivalentSectionId_ShouldUpdateStore()
        {
            // Arrange
            _fixture.SetupExistingStore();
            await _fixture.PrepareDatabaseAsync();
            _fixture.SetupContract();
            _fixture.SetupExpectedPersistedStoreWithSameSectionIds();
            var sut = _fixture.CreateSut();

            // Act
            var response = await sut.UpdateStoreAsync(_fixture.Contract!);

            // Assert
            response.Should().BeOfType<OkResult>();

            var stores = await _fixture.LoadPersistedStoresAsync();

            stores.Should().HaveCount(1);
            stores.First().Should().BeEquivalentTo(_fixture.ExpectedPersistedStore);
        }

        private class UpdateStoreAsyncFixture : LocalFixture
        {
            public UpdateStoreAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public UpdateStoreContract? Contract { get; private set; }
            public Store? ExistingStore { get; private set; }
            public Store? ExpectedPersistedStore { get; private set; }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingStore);

                Contract = new UpdateStoreContract(ExistingStore.Id.Value, "MyStore", new List<UpdateSectionContract>()
                {
                    new(ExistingStore.Sections.First().Id.Value, "mySection", 0, true)
                });
            }

            public override async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingStore);

                using var scope = CreateServiceScope();
                await ApplyMigrationsAsync(scope);

                using var transaction = await CreateTransactionAsync(scope);

                var repo = CreateStoreRepository(scope);
                await repo.StoreAsync(ExistingStore, default);

                await transaction.CommitAsync(default);
            }

            public void SetupExistingStore()
            {
                ExistingStore = StoreMother.Initial().Create();
            }

            public void SetupExpectedPersistedStoreWithSameSectionIds()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);

                var sections = new List<ISection>();
                foreach (var section in Contract.Sections)
                {
                    sections.Add(new Section(
                        new SectionId(section.Id!.Value),
                        new SectionName(section.Name),
                        section.SortingIndex,
                        section.IsDefaultSection));
                }

                var factory = SetupScope.ServiceProvider.GetRequiredService<ISectionFactory>();

                ExpectedPersistedStore = new Store(
                    new StoreId(Contract.Id),
                    new StoreName(Contract.Name),
                    false,
                    new Sections(sections, factory));
            }
        }
    }

    private abstract class LocalFixture : DatabaseFixture
    {
        protected readonly IServiceScope SetupScope;

        protected LocalFixture(DockerFixture dockerFixture) : base(dockerFixture)
        {
            SetupScope = CreateServiceScope();
        }

        public StoreController CreateSut()
        {
            var scope = CreateServiceScope();
            return scope.ServiceProvider.GetRequiredService<StoreController>();
        }

        public override IEnumerable<DbContext> GetDbContexts(IServiceScope scope)
        {
            yield return scope.ServiceProvider.GetRequiredService<ShoppingListContext>();
            yield return scope.ServiceProvider.GetRequiredService<StoreContext>();
        }

        protected IStoreRepository CreateStoreRepository(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<IStoreRepository>();
        }

        public async Task<IList<IStore>> LoadPersistedStoresAsync()
        {
            using var scope = CreateServiceScope();
            var repo = CreateStoreRepository(scope);

            using (await CreateTransactionAsync(scope))
            {
                return (await repo.GetAsync(default)).ToList();
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