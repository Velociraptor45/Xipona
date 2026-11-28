using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.ApplicationServices.Stores.Commands.CreateStore;
using Xipona.Api.ApplicationServices.Stores.Commands.ModifyStore;
using Xipona.Api.Contracts.Common;
using Xipona.Api.Contracts.Stores.Commands.CreateStore;
using Xipona.Api.Contracts.Stores.Commands.ModifyStore;
using Xipona.Api.Contracts.Stores.Queries.Get;
using Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using Xipona.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using Xipona.Api.Contracts.Stores.Queries.Shared;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.Stores.Models.Factories;
using Xipona.Api.Domain.Stores.Ports;
using Xipona.Api.Domain.TestKit.Stores.Models;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Repositories.Items.Contexts;
using Xipona.Api.Repositories.Items.Entities;
using Xipona.Api.Repositories.ShoppingLists.Contexts;
using Xipona.Api.Repositories.Stores.Contexts;
using Xipona.Api.Repositories.TestKit.Items.Entities;
using Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;
using Xipona.Api.Repositories.TestKit.Stores.Entities;
using Xipona.Api.TestTools.AutoFixture;
using Xipona.Api.TestTools.Exceptions;
using System;
using Xipona.Api.Repositories.ItemCategories.Contexts;
using Xipona.Api.Repositories.Manufacturers.Contexts;
using Xunit;
using Section = Xipona.Api.Repositories.Stores.Entities.Section;

namespace Xipona.Api.Endpoint.IntegrationTests.v1.Endpoints;

public class StoreEndpointsIntegrationTests
{
    public class GetStoreByIdAsync
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
                    TestContext.Current.CancellationToken);
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

                await transaction.CommitAsync(TestContext.Current.CancellationToken);
            }
        }
    }

    public class GetActiveStoresForShoppingAsync
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
                    TestContext.Current.CancellationToken);
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

                await transaction.CommitAsync(TestContext.Current.CancellationToken);
            }
        }
    }

    public class GetActiveStoresForItemAsync
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
                    TestContext.Current.CancellationToken);
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

                await transaction.CommitAsync(TestContext.Current.CancellationToken);
            }
        }
    }

    public class GetActiveStoresOverviewAsync
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
                    TestContext.Current.CancellationToken);
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

                await transaction.CommitAsync(TestContext.Current.CancellationToken);
            }
        }
    }

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
                    TestContext.Current.CancellationToken);
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedPersistedStore);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedShoppingList);

            // Act
            var response = await _fixture.ActAsync();

            // Assert
            using var assertionServiceScope = _fixture.CreateServiceScope();
            response.Should().BeOfType<NoContent>();

            var stores = (await _fixture.LoadAllStoresAsync(assertionServiceScope)).ToArray();
            stores.Should().HaveCount(1);
            stores.First().Should().BeEquivalentTo(_fixture.ExpectedPersistedStore,
                opt => opt
                    .Excluding(info => info.Path.EndsWith(".Store"))
                    .ExcludeRowVersion()
                    .WithCreatedAtPrecision());

            var items = (await _fixture.LoadAllItemsAsync(assertionServiceScope)).ToArray();
            items.Should().HaveCount(1);
            items.First().Should().BeEquivalentTo(_fixture.ExpectedItem,
                opt => opt
                    .Excluding(info => info.Path.EndsWith(".Item"))
                    .WithUpdatedOnPrecision()
                    .WithCreatedAtPrecision()
                    .ExcludeRowVersion());

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertionServiceScope)).ToArray();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.First().Should().BeEquivalentTo(_fixture.ExpectedShoppingList,
                opt => opt
                    .Excluding(info => info.Path.EndsWith(".ShoppingList"))
                    .ExcludeRowVersion()
                    .WithCreatedAtPrecision());
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
            using var assertionServiceScope = _fixture.CreateServiceScope();
            response.Should().BeOfType<NoContent>();

            var stores = (await _fixture.LoadAllStoresAsync(assertionServiceScope)).ToArray();
            stores.Should().HaveCount(1);
            stores.First().Should().BeEquivalentTo(_fixture.ExpectedPersistedStore,
                opt => opt
                    .Excluding(info => info.Path.EndsWith(".Store"))
                    .ExcludeRowVersion()
                    .WithCreatedAtPrecision());

            var items = (await _fixture.LoadAllItemsAsync(assertionServiceScope)).ToArray();
            items.Should().HaveCount(1);
            items.First().Should().BeEquivalentTo(_fixture.ExpectedItem,
                opt => opt
                    .Excluding(info => info.Path.EndsWith(".Item") || info.Path.EndsWith(".ItemType"))
                    .WithUpdatedOnPrecision()
                    .WithCreatedAtPrecision()
                    .ExcludeRowVersion());

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertionServiceScope)).ToArray();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.First().Should().BeEquivalentTo(_fixture.ExpectedShoppingList,
                opt => opt
                    .Excluding(info => info.Path.EndsWith(".ShoppingList"))
                    .ExcludeRowVersion()
                    .WithCreatedAtPrecision());
        }

        [Fact]
        public async Task UpdateStoreAsync_WithNewSectionHavingSameSortingIndexAsDeletedSection_ShouldUpdateStore()
        {
            // Arrange
            _fixture.SetupExistingStoreWithDeletedSection();
            _fixture.SetupExistingShoppingListEmpty();
            await _fixture.PrepareDatabaseAsync();
            _fixture.SetupContractWithSameSortingIndexAsDeletedSection();
            _fixture.SetupExpectedPersistedStoreWithSameSectionIds();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedPersistedStore);

            // Act
            var response = await _fixture.ActAsync();

            // Assert
            using var assertionServiceScope = _fixture.CreateServiceScope();
            response.Should().BeOfType<NoContent>();

            var stores = (await _fixture.LoadAllStoresAsync(assertionServiceScope)).ToArray();
            stores.Should().HaveCount(1);
            stores.First().Should().BeEquivalentTo(_fixture.ExpectedPersistedStore,
                opt => opt
                    .Excluding(info => info.Path.EndsWith(".Store"))
                    .ExcludeSectionIds()
                    .ExcludeRowVersion()
                    .WithCreatedAtPrecision());
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
                    TestContext.Current.CancellationToken);
            }

            public void SetupContractWithSameSortingIndexAsDeletedSection()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingStore);

                Contract = new ModifyStoreContract(ExistingStore.Id, "MyStore", new List<ModifySectionContract>()
                {
                    new(ExistingStore.Sections.ElementAt(0).Id, "mySection1", 0, true),
                    new(null, "mySection2", 1, false),
                    new(ExistingStore.Sections.ElementAt(2).Id, "mySection3", 2, false)
                });
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
                TestPropertyNotSetException.ThrowIfNull(ExistingShoppingList);

                await ApplyMigrationsAsync(SetupScope);

                using var transaction = await CreateTransactionAsync(SetupScope);
                await using var storeContext = GetContextInstance<StoreContext>(SetupScope);
                await using var itemContext = GetContextInstance<ItemContext>(SetupScope);
                await using var shoppingListContext = GetContextInstance<ShoppingListContext>(SetupScope);

                storeContext.Add(ExistingStore);
                await storeContext.SaveChangesAsync();

                if (ExistingItem is not null)
                {
                    itemContext.Add(ExistingItem);
                    await itemContext.SaveChangesAsync();
                }

                shoppingListContext.Add(ExistingShoppingList);
                await shoppingListContext.SaveChangesAsync();

                await transaction.CommitAsync(TestContext.Current.CancellationToken);
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

            public void SetupExistingShoppingListEmpty()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingStore);

                ExistingShoppingList = ShoppingListEntityMother.Empty()
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

            public void SetupExistingStoreWithDeletedSection()
            {
                ExistingStore = StoreEntityMother.ActiveAndDeletedSection().Create();
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
                        Id = section.Id ?? Guid.NewGuid(),
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
            yield return scope.ServiceProvider.GetRequiredService<ItemCategoryContext>();
            yield return scope.ServiceProvider.GetRequiredService<ManufacturerContext>();
            yield return scope.ServiceProvider.GetRequiredService<StoreContext>();
            yield return scope.ServiceProvider.GetRequiredService<ItemContext>();
        }

        public async Task<IList<IStore>> LoadPersistedStoresAsync()
        {
            using var scope = CreateServiceScope();
            var repo = scope.ServiceProvider.GetRequiredService<Func<CancellationToken, IStoreRepository>>()(TestContext.Current.CancellationToken);

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