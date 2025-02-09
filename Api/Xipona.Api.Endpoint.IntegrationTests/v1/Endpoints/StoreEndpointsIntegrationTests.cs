using FluentAssertions;
using Force.DeepCloner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Commands.CreateStore;
using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Commands.ModifyStore;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Stores.Commands.CreateStore;
using ProjectHermes.Xipona.Api.Contracts.Stores.Commands.ModifyStore;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Get;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Shared;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Stores.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Repositories.Items.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Items.Entities;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Stores.Contexts;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Items.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Stores.Entities;
using ProjectHermes.Xipona.Api.TestTools.AutoFixture;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System;
using Xunit;
using Section = ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.Xipona.Api.Endpoint.IntegrationTests.v1.Endpoints;

public class StoreEndpointsIntegrationTests
{
    public class GetStoreByIdAsync : IAssemblyFixture<DockerFixture>
    {
        private readonly GetStoreByIdAsyncFixture _fixture;

        public GetStoreByIdAsync(DockerFixture dockerFixture)
        {
            _fixture = new GetStoreByIdAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task GetStoreByIdAsync_WithValidId_ShouldReturnStore()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupExistingStoreWithStoreId();
            _fixture.SetupExpectedResult();
            await _fixture.PrepareDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<StoreContract>>();

            var okResult = result as Ok<StoreContract>;
            okResult!.Value.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public async Task GetStoreByIdAsync_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupExistingStoreWithRandomStoreId();
            await _fixture.PrepareDatabaseAsync();

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFound<ErrorContract>>();
        }

        private sealed class GetStoreByIdAsyncFixture : StoreEndpointFixture
        {
            private Repositories.Stores.Entities.Store? _existingStore;

            public GetStoreByIdAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public StoreId? StoreId { get; private set; }
            public StoreContract? ExpectedResult { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                var scope = CreateServiceScope();
                return await StoreEndpoints.GetStoreById(
                    StoreId.Value,
                    scope.ServiceProvider.GetRequiredService<IQueryDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IStore, StoreContract>>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    default);
            }

            public void SetupStoreId()
            {
                StoreId = Domain.Stores.Models.StoreId.New;
            }

            public void SetupExistingStoreWithStoreId()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                _existingStore = StoreEntityMother
                    .ActiveAndDeletedSection()
                    .WithId(StoreId.Value)
                    .Create();
            }

            public void SetupExistingStoreWithRandomStoreId()
            {
                _existingStore = StoreEntityMother
                    .ActiveAndDeletedSection()
                    .Create();
            }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingStore);

                ExpectedResult = new StoreContract(
                    _existingStore.Id,
                    _existingStore.Name,
                    _existingStore.Sections
                        .Where(s => !s.IsDeleted)
                        .Select(s => new SectionContract(s.Id, s.Name, s.SortIndex, s.IsDefaultSection)));
            }

            public override async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingStore);

                await ApplyMigrationsAsync(SetupScope);

                using var transaction = await CreateTransactionAsync(SetupScope);
                await using var dbContext = GetContextInstance<StoreContext>(SetupScope);

                await dbContext.AddAsync(_existingStore);
                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync(default);
            }
        }
    }

    public class GetActiveStoresForShoppingAsync : IAssemblyFixture<DockerFixture>
    {
        private readonly GetActiveStoresForShoppingAsyncFixture _fixture;

        public GetActiveStoresForShoppingAsync(DockerFixture dockerFixture)
        {
            _fixture = new GetActiveStoresForShoppingAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task GetActiveStoresForShoppingAsync_WithDeletedStore_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupExistingStores();
            _fixture.SetupExpectedResult();
            await _fixture.PrepareDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<List<StoreForShoppingContract>>>();

            var okResult = result as Ok<List<StoreForShoppingContract>>;
            okResult!.Value.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class GetActiveStoresForShoppingAsyncFixture : StoreEndpointFixture
        {
            private IList<Repositories.Stores.Entities.Store>? _existingStores;

            public GetActiveStoresForShoppingAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public IReadOnlyCollection<StoreForShoppingContract>? ExpectedResult { get; private set; }

            public async Task<IResult> ActAsync()
            {
                var scope = CreateServiceScope();
                return await StoreEndpoints.GetActiveStoresForShopping(
                    scope.ServiceProvider.GetRequiredService<IQueryDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IStore, StoreForShoppingContract>>(),
                    default);
            }

            public void SetupExistingStores()
            {
                _existingStores = new List<Repositories.Stores.Entities.Store>
                {
                    StoreEntityMother.ActiveAndDeletedSection().Create(),
                    StoreEntityMother.ActiveAndDeletedSection().Create(),
                    StoreEntityMother.Deleted().Create()
                };
            }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingStores);

                ExpectedResult = _existingStores
                    .Where(s => !s.Deleted)
                    .Select(s => new StoreForShoppingContract(
                        s.Id,
                        s.Name,
                        s.Sections
                            .Where(sc => !sc.IsDeleted)
                            .Select(sc =>
                                new SectionForShoppingContract(sc.Id, sc.Name, sc.IsDefaultSection, sc.SortIndex))))
                    .ToList();
            }

            public override async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingStores);

                await ApplyMigrationsAsync(SetupScope);

                using var transaction = await CreateTransactionAsync(SetupScope);
                await using var dbContext = GetContextInstance<StoreContext>(SetupScope);

                foreach (var existingStore in _existingStores)
                {
                    dbContext.Add(existingStore);
                }

                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync(default);
            }
        }
    }

    public class GetActiveStoresForItemAsync : IAssemblyFixture<DockerFixture>
    {
        private readonly GetActiveStoresForItemAsyncFixture _fixture;

        public GetActiveStoresForItemAsync(DockerFixture dockerFixture)
        {
            _fixture = new GetActiveStoresForItemAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task GetActiveStoresForItemAsync_WithDeletedStore_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupExistingStores();
            _fixture.SetupExpectedResult();
            await _fixture.PrepareDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<List<StoreForItemContract>>>();

            var okResult = result as Ok<List<StoreForItemContract>>;
            okResult!.Value.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class GetActiveStoresForItemAsyncFixture : StoreEndpointFixture
        {
            private IList<Repositories.Stores.Entities.Store>? _existingStores;

            public GetActiveStoresForItemAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public IReadOnlyCollection<StoreForItemContract>? ExpectedResult { get; private set; }

            public async Task<IResult> ActAsync()
            {
                var scope = CreateServiceScope();
                return await StoreEndpoints.GetActiveStoresForItem(
                    scope.ServiceProvider.GetRequiredService<IQueryDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IStore, StoreForItemContract>>(),
                    default);
            }

            public void SetupExistingStores()
            {
                _existingStores = new List<Repositories.Stores.Entities.Store>
                {
                    StoreEntityMother.ActiveAndDeletedSection().Create(),
                    StoreEntityMother.ActiveAndDeletedSection().Create(),
                    StoreEntityMother.Deleted().Create()
                };
            }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingStores);

                ExpectedResult = _existingStores
                    .Where(s => !s.Deleted)
                    .Select(s => new StoreForItemContract(
                        s.Id,
                        s.Name,
                        s.Sections
                            .Where(sc => !sc.IsDeleted)
                            .Select(sc =>
                                new SectionForItemContract(sc.Id, sc.Name, sc.IsDefaultSection, sc.SortIndex))))
                    .ToList();
            }

            public override async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingStores);

                await ApplyMigrationsAsync(SetupScope);

                using var transaction = await CreateTransactionAsync(SetupScope);
                await using var dbContext = GetContextInstance<StoreContext>(SetupScope);

                foreach (var existingStore in _existingStores)
                {
                    dbContext.Add(existingStore);
                }

                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync(default);
            }
        }
    }

    public class GetActiveStoresOverviewAsync : IAssemblyFixture<DockerFixture>
    {
        private readonly GetActiveStoresOverviewAsyncFixture _fixture;

        public GetActiveStoresOverviewAsync(DockerFixture dockerFixture)
        {
            _fixture = new GetActiveStoresOverviewAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task GetActiveStoresOverviewAsync_WithDeletedStore_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupExistingStores();
            _fixture.SetupExpectedResult();
            await _fixture.PrepareDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<List<StoreSearchResultContract>>>();

            var okResult = result as Ok<List<StoreSearchResultContract>>;
            okResult!.Value.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class GetActiveStoresOverviewAsyncFixture : StoreEndpointFixture
        {
            private IList<Repositories.Stores.Entities.Store>? _existingStores;

            public GetActiveStoresOverviewAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public IReadOnlyCollection<StoreSearchResultContract>? ExpectedResult { get; private set; }

            public async Task<IResult> ActAsync()
            {
                var scope = CreateServiceScope();
                return await StoreEndpoints.GetActiveStoresOverview(
                    scope.ServiceProvider.GetRequiredService<IQueryDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IStore, StoreSearchResultContract>>(),
                    default);
            }

            public void SetupExistingStores()
            {
                _existingStores = new List<Repositories.Stores.Entities.Store>
                {
                    StoreEntityMother.Initial().Create(),
                    StoreEntityMother.Initial().Create(),
                    StoreEntityMother.Deleted().Create()
                };
            }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingStores);

                ExpectedResult = _existingStores
                    .Where(s => !s.Deleted)
                    .Select(s => new StoreSearchResultContract(s.Id, s.Name))
                    .ToList();
            }

            public override async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingStores);

                await ApplyMigrationsAsync(SetupScope);

                using var transaction = await CreateTransactionAsync(SetupScope);
                await using var dbContext = GetContextInstance<StoreContext>(SetupScope);

                foreach (var existingStore in _existingStores)
                {
                    dbContext.Add(existingStore);
                }

                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync(default);
            }
        }
    }

    public class CreateStoreAsync : IAssemblyFixture<DockerFixture>
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

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().BeOfType<CreatedAtRoute<StoreContract>>();
            var createdResult = result as CreatedAtRoute<StoreContract>;
            createdResult!.Value.Should().BeEquivalentTo(_fixture.ExpectedResultValue!,
                opts => opts
                    .Excluding(x => x.Path.EndsWith("Id"))
                    .ExcludeRowVersion());
        }

        [Fact]
        public async Task CreateStoreAsync_WithValidData_ShouldPersistStore()
        {
            // Arrange
            await _fixture.PrepareDatabaseAsync();
            _fixture.SetupContract();
            _fixture.SetupExpectedPersistedStore();

            // Act
            await _fixture.ActAsync();

            // Assert
            var stores = await _fixture.LoadPersistedStoresAsync();

            stores.Should().HaveCount(1);
            stores.First().Should().BeEquivalentTo(_fixture.ExpectedPersistedStore,
                opts => opts
                    .Excluding(x => x.Path.EndsWith("Id") || x.Path == "CreatedAt")
                    .ExcludeRowVersion());
            stores.First().CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(30));
        }

        private class CreateStoreAsyncFixture : StoreEndpointFixture
        {
            public CreateStoreAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public CreateStoreContract? Contract { get; private set; }
            public StoreContract? ExpectedResultValue { get; private set; }
            public Store? ExpectedPersistedStore { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);

                var scope = CreateServiceScope();
                return await StoreEndpoints.CreateStore(
                    Contract,
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToDomainConverter<CreateStoreContract, CreateStoreCommand>>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IStore, StoreContract>>(),
                    default);
            }

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

    public class UpdateStoreAsync : IAssemblyFixture<DockerFixture>
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedPersistedStore);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedShoppingList);

            // Act
            var response = await _fixture.ActAsync();

            // Assert
            response.Should().BeOfType<NoContent>();

            var stores = (await _fixture.LoadAllStoresAsync()).ToArray();
            stores.Should().HaveCount(1);
            stores.First().Should().BeEquivalentTo(_fixture.ExpectedPersistedStore,
                opt => opt
                    .Excluding(info => info.Path.EndsWith(".Store"))
                    .ExcludeRowVersion()
                    .WithCreatedAtPrecision());

            var items = (await _fixture.LoadAllItemsAsync()).ToArray();
            items.Should().HaveCount(1);
            items.First().Should().BeEquivalentTo(_fixture.ExpectedItem,
                opt => opt
                    .Excluding(info => info.Path.EndsWith(".Item"))
                    .WithUpdatedOnPrecision()
                    .WithCreatedAtPrecision());

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync()).ToArray();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.First().Should().BeEquivalentTo(_fixture.ExpectedShoppingList,
                opt => opt.Excluding(info => info.Path.EndsWith(".ShoppingList")).WithCreatedAtPrecision());
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedPersistedStore);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedShoppingList);

            // Act
            var response = await _fixture.ActAsync();

            // Assert
            response.Should().BeOfType<NoContent>();

            var stores = (await _fixture.LoadAllStoresAsync()).ToArray();
            stores.Should().HaveCount(1);
            stores.First().Should().BeEquivalentTo(_fixture.ExpectedPersistedStore,
                opt => opt
                    .Excluding(info => info.Path.EndsWith(".Store"))
                    .ExcludeRowVersion()
                    .WithCreatedAtPrecision());

            var items = (await _fixture.LoadAllItemsAsync()).ToArray();
            items.Should().HaveCount(1);
            items.First().Should().BeEquivalentTo(_fixture.ExpectedItem,
                opt => opt
                    .Excluding(info => info.Path.EndsWith(".Item") || info.Path.EndsWith(".ItemType"))
                    .WithUpdatedOnPrecision()
                    .WithCreatedAtPrecision());

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync()).ToArray();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.First().Should().BeEquivalentTo(_fixture.ExpectedShoppingList,
                opt => opt.Excluding(info => info.Path.EndsWith(".ShoppingList")).WithCreatedAtPrecision());
        }

        private class UpdateStoreAsyncFixture : StoreEndpointFixture
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

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);

                var scope = CreateServiceScope();
                return await StoreEndpoints.ModifyStore(
                    Contract,
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    scope.ServiceProvider.GetRequiredService<IToDomainConverter<ModifyStoreContract, ModifyStoreCommand>>(),
                    default);
            }

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
                    Sections = sections,
                    CreatedAt = ExistingStore.CreatedAt
                };
            }
        }
    }

    private abstract class StoreEndpointFixture : DatabaseFixture
    {
        protected readonly IServiceScope SetupScope;

        protected StoreEndpointFixture(DockerFixture dockerFixture) : base(dockerFixture)
        {
            SetupScope = CreateServiceScope();
        }

        public override IEnumerable<DbContext> GetDbContexts(IServiceScope scope)
        {
            yield return scope.ServiceProvider.GetRequiredService<ShoppingListContext>();
            yield return scope.ServiceProvider.GetRequiredService<ItemContext>();
            yield return scope.ServiceProvider.GetRequiredService<StoreContext>();
        }

        protected IStoreRepository CreateStoreRepository(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<Func<CancellationToken, IStoreRepository>>()(default);
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
                return (await repo.GetActiveAsync()).ToList();
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