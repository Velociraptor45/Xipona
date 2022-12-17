using FluentAssertions;
using FluentAssertions.Extensions;
using Force.DeepCloner;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Commands.ModifyStore;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.Items.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.ShoppingLists.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.Stores.Entities;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System;
using Xunit;
using Section = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Section;

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
                        .WithIsDeleted(false)
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
            _fixture.SetupExistingItem();
            _fixture.SetupExistingShoppingList();
            await _fixture.PrepareDatabaseAsync();
            _fixture.SetupContract();
            _fixture.SetupExpectedPersistedStoreWithSameSectionIds();
            _fixture.SetupExpectedItem();
            _fixture.SetupExpectedShoppingList();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedPersistedStore);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedShoppingList);

            // Act
            var response = await sut.ModifyStoreAsync(_fixture.Contract!);

            // Assert
            response.Should().BeOfType<OkResult>();

            var stores = (await _fixture.LoadAllStoresAsync()).ToArray();
            stores.Should().HaveCount(1);
            stores.First().Should().BeEquivalentTo(_fixture.ExpectedPersistedStore,
                opt => opt.Excluding(info => info.Path.EndsWith(".Store")));

            var items = (await _fixture.LoadAllItemsAsync()).ToArray();
            items.Should().HaveCount(1);
            items.First().Should().BeEquivalentTo(_fixture.ExpectedItem,
                opt => opt
                    .Excluding(info => info.Path.EndsWith(".Item"))
                    .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1.Milliseconds()))
                    .When(info => info.Path == "UpdatedOn"));

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync()).ToArray();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.First().Should().BeEquivalentTo(_fixture.ExpectedShoppingList,
                opt => opt.Excluding(info => info.Path.EndsWith(".ShoppingList")));
        }

        [Fact]
        public async Task UpdateStoreAsync_WithEquivalentSectionIdAndItemTypes_ShouldUpdateStore()
        {
            // Arrange
            _fixture.SetupExistingStore();
            _fixture.SetupExistingItemWithTypes();
            _fixture.SetupExistingShoppingListWithItemType();
            await _fixture.PrepareDatabaseAsync();
            _fixture.SetupContract();
            _fixture.SetupExpectedPersistedStoreWithSameSectionIds();
            _fixture.SetupExpectedItemWithTypes();
            _fixture.SetupExpectedShoppingList();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedPersistedStore);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedShoppingList);

            // Act
            var response = await sut.ModifyStoreAsync(_fixture.Contract!);

            // Assert
            response.Should().BeOfType<OkResult>();

            var stores = (await _fixture.LoadAllStoresAsync()).ToArray();
            stores.Should().HaveCount(1);
            stores.First().Should().BeEquivalentTo(_fixture.ExpectedPersistedStore,
                opt => opt.Excluding(info => info.Path.EndsWith(".Store")));

            var items = (await _fixture.LoadAllItemsAsync()).ToArray();
            items.Should().HaveCount(1);
            items.First().Should().BeEquivalentTo(_fixture.ExpectedItem,
                opt => opt
                    .Excluding(info => info.Path.EndsWith(".Item") || info.Path.EndsWith(".ItemType"))
                    .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1.Milliseconds()))
                    .When(info => info.Path == "UpdatedOn"));

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync()).ToArray();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.First().Should().BeEquivalentTo(_fixture.ExpectedShoppingList,
                opt => opt.Excluding(info => info.Path.EndsWith(".ShoppingList")));
        }

        private class UpdateStoreAsyncFixture : LocalFixture
        {
            public UpdateStoreAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public ModifyStoreContract? Contract { get; private set; }
            public Repositories.Stores.Entities.Store? ExistingStore { get; private set; }
            public Repositories.Stores.Entities.Store? ExpectedPersistedStore { get; private set; }
            public Item? ExistingItem { get; private set; }
            public Item? ExpectedItem { get; private set; }
            public Repositories.ShoppingLists.Entities.ShoppingList? ExistingShoppingList { get; private set; }
            public Repositories.ShoppingLists.Entities.ShoppingList? ExpectedShoppingList { get; private set; }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingStore);

                Contract = new ModifyStoreContract(ExistingStore.Id, "MyStore", new List<ModifySectionContract>()
                {
                    new(ExistingStore.Sections.First().Id, "mySection", 0, true)
                });
            }

            public override async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingStore);
                TestPropertyNotSetException.ThrowIfNull(ExistingItem);
                TestPropertyNotSetException.ThrowIfNull(ExistingShoppingList);

                await ApplyMigrationsAsync(SetupScope);

                using var transaction = await CreateTransactionAsync(SetupScope);
                await using var storeContext = GetContextInstance<StoreContext>(SetupScope);
                await using var itemContext = GetContextInstance<ItemContext>(SetupScope);
                await using var shoppingListContext = GetContextInstance<ShoppingListContext>(SetupScope);

                storeContext.Add(ExistingStore);
                await storeContext.SaveChangesAsync();

                itemContext.Add(ExistingItem);
                await itemContext.SaveChangesAsync();

                shoppingListContext.Add(ExistingShoppingList);
                await shoppingListContext.SaveChangesAsync();

                await transaction.CommitAsync(default);
            }

            public void SetupExistingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingStore);

                var availability = new AvailableAtEntityBuilder()
                    .WithStoreId(ExistingStore.Id)
                    .WithDefaultSectionId(ExistingStore.Sections.ElementAt(1).Id)
                    .CreateMany(1)
                    .ToArray();

                ExistingItem = ItemEntityMother.Initial()
                    .WithAvailableAt(availability)
                    .Create();
            }

            public void SetupExistingItemWithTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingStore);

                var availability = new ItemTypeAvailableAtEntityBuilder()
                    .WithStoreId(ExistingStore.Id)
                    .WithDefaultSectionId(ExistingStore.Sections.ElementAt(1).Id)
                    .CreateMany(1)
                    .ToArray();

                var types = ItemTypeEntityMother.Initial()
                    .WithAvailableAt(availability)
                    .CreateMany(1)
                    .ToArray();

                ExistingItem = ItemEntityMother.InitialWithTypes()
                    .WithItemTypes(types)
                    .Create();
            }

            public void SetupExistingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingItem);
                TestPropertyNotSetException.ThrowIfNull(ExistingStore);

                ExistingShoppingList = ShoppingListEntityMother
                    .InitialWithOneItem(ExistingItem.Id, null, ExistingStore.Sections.ElementAt(1).Id)
                    .WithStoreId(ExistingStore.Id)
                    .Create();
            }

            public void SetupExistingShoppingListWithItemType()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingItem);
                TestPropertyNotSetException.ThrowIfNull(ExistingStore);

                ExistingShoppingList = ShoppingListEntityMother
                    .InitialWithOneItem(ExistingItem.Id, ExistingItem.ItemTypes.First().Id,
                        ExistingStore.Sections.ElementAt(1).Id)
                    .WithStoreId(ExistingStore.Id)
                    .Create();
            }

            public void SetupExpectedShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingShoppingList);
                TestPropertyNotSetException.ThrowIfNull(Contract);

                ExpectedShoppingList = ExistingShoppingList.DeepClone();
                ExpectedShoppingList.ItemsOnList.First().SectionId = Contract.Sections.First().Id!.Value;
            }

            public void SetupExpectedItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingItem);
                TestPropertyNotSetException.ThrowIfNull(ExistingStore);

                ExpectedItem = ExistingItem.DeepClone();
                ExpectedItem.AvailableAt.First().DefaultSectionId = ExistingStore.Sections.First().Id;
            }

            public void SetupExpectedItemWithTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingItem);
                TestPropertyNotSetException.ThrowIfNull(ExistingStore);

                ExpectedItem = ExistingItem.DeepClone();
                ExpectedItem.ItemTypes.First().AvailableAt.First().DefaultSectionId = ExistingStore.Sections.First().Id;
            }

            public void SetupExistingStore()
            {
                ExistingStore = StoreEntityMother.Initial().Create();
            }

            public void SetupExpectedPersistedStoreWithSameSectionIds()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);
                TestPropertyNotSetException.ThrowIfNull(ExistingStore);

                var sections = new List<Section>();
                foreach (var section in Contract.Sections)
                {
                    sections.Add(new Section
                    {
                        Id = section.Id!.Value,
                        Name = section.Name,
                        SortIndex = section.SortingIndex,
                        IsDefaultSection = section.IsDefaultSection,
                        IsDeleted = false,
                        StoreId = ExistingStore.Id
                    });
                }

                foreach (var section in ExistingStore.Sections)
                {
                    if (Contract.Sections.Any(s => s.Id == section.Id))
                        continue;

                    sections.Add(new Section
                    {
                        Id = section.Id,
                        Name = section.Name,
                        SortIndex = section.SortIndex,
                        IsDefaultSection = section.IsDefaultSection,
                        IsDeleted = true,
                        StoreId = ExistingStore.Id
                    });
                }

                ExpectedPersistedStore = new Repositories.Stores.Entities.Store
                {
                    Id = ExistingStore.Id,
                    Name = Contract.Name,
                    Deleted = false,
                    Sections = sections
                };
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
            yield return scope.ServiceProvider.GetRequiredService<ItemContext>();
            yield return scope.ServiceProvider.GetRequiredService<StoreContext>();
        }

        protected IStoreRepository CreateStoreRepository(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<IStoreRepository>();
        }

        public async Task<IEnumerable<Repositories.Stores.Entities.Store>> LoadAllStoresAsync()
        {
            using var assertScope = CreateServiceScope();
            var storeContext = GetContextInstance<StoreContext>(assertScope);

            return await storeContext.Stores.AsNoTracking()
                .Include(s => s.Sections)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Repositories.ShoppingLists.Entities.ShoppingList>> LoadAllShoppingListsAsync()
        {
            using var assertScope = CreateServiceScope();
            var shoppingListContext = GetContextInstance<ShoppingListContext>(assertScope);

            return await shoppingListContext.ShoppingLists.AsNoTracking()
                .Include(l => l.ItemsOnList)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Item>> LoadAllItemsAsync()
        {
            using var assertScope = CreateServiceScope();
            var itemContext = GetContextInstance<ItemContext>(assertScope);

            return await itemContext.Items.AsNoTracking()
                .Include(item => item.AvailableAt)
                .Include(item => item.ItemTypes)
                .ThenInclude(itemType => itemType.AvailableAt)
                .Include(item => item.ItemTypes)
                .ToArrayAsync();
        }

        public async Task<IList<IStore>> LoadPersistedStoresAsync()
        {
            using var scope = CreateServiceScope();
            var repo = CreateStoreRepository(scope);

            using (await CreateTransactionAsync(scope))
            {
                return (await repo.GetActiveAsync(default)).ToList();
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