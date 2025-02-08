using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using Force.DeepCloner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.ItemUpdateWithTypes;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.ModifyItem;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.UpdateItem;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.CreateItem;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.CreateItemWithTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.ModifyItem;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItem;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItemPrice;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItemWithTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.GetItemTypePrices;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsForShoppingLists;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Shared;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Shared;
using ProjectHermes.Xipona.Api.Contracts.TestKit.Common.Queries;
using ProjectHermes.Xipona.Api.Contracts.TestKit.Items.Queries.Get;
using ProjectHermes.Xipona.Api.Core.Attributes;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Creations;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Shared;
using ProjectHermes.Xipona.Api.Endpoint.IntegrationTests.Common;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Items.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Items.Entities;
using ProjectHermes.Xipona.Api.Repositories.Manufacturers.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Manufacturers.Entities;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Entities;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Stores.Contexts;
using ProjectHermes.Xipona.Api.Repositories.TestKit.ItemCategories.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Items.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Manufacturers.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Recipes.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Stores.Entities;
using ProjectHermes.Xipona.Api.TestTools.AutoFixture;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using ProjectHermes.Xipona.Api.TestTools.Extensions;
using System;
using System.Text.RegularExpressions;
using Xunit;
using Item = ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item;
using ItemAvailabilityContract = ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared.ItemAvailabilityContract;
using ItemCategory = ProjectHermes.Xipona.Api.Repositories.ItemCategories.Entities.ItemCategory;
using ItemType = ProjectHermes.Xipona.Api.Repositories.Items.Entities.ItemType;
using ItemTypeAvailableAt = ProjectHermes.Xipona.Api.Repositories.Items.Entities.ItemTypeAvailableAt;
using Section = ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Section;
using Store = ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Store;

namespace ProjectHermes.Xipona.Api.Endpoint.IntegrationTests.v1.Controllers;

public class ItemControllerIntegrationTests
{
    public sealed class GetItemTypePricesAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly GetItemTypePricesAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task GetItemTypePricesAsync_WithNoItem_ShouldReturnNotFound()
        {
            // Arrange
            await _fixture.SetupDatabaseWithoutItemAsync();

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFound<ErrorContract>>();
            var notFound = (NotFound<ErrorContract>)result;
            notFound.Value.Should().NotBeNull();
            notFound.Value.ErrorCode.Should().Be((int)ErrorReasonCode.ItemNotFound);
        }

        [Fact]
        public async Task GetItemTypePricesAsync_WithItemNotAvailableAtStore_ShouldReturnUnprocessableEntity()
        {
            // Arrange
            _fixture.SetupItemNotAvailableAtStore();
            await _fixture.SetupDatabaseAsync();

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UnprocessableEntity<ErrorContract>>();
            var unprocessableEntity = (UnprocessableEntity<ErrorContract>)result;
            unprocessableEntity.Value.Should().NotBeNull();
            unprocessableEntity.Value.ErrorCode.Should().Be((int)ErrorReasonCode.ItemAtStoreNotAvailable);
        }

        [Fact]
        public async Task GetItemTypePricesAsync_WithItemWithoutTypes_ShouldReturnUnprocessableEntity()
        {
            // Arrange
            _fixture.SetupItemWithoutTypes();
            await _fixture.SetupDatabaseAsync();

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UnprocessableEntity<ErrorContract>>();
            var unprocessableEntity = (UnprocessableEntity<ErrorContract>)result;
            unprocessableEntity.Value.Should().NotBeNull();
            unprocessableEntity.Value.ErrorCode.Should().Be((int)ErrorReasonCode.ItemHasNoItemTypes);
        }

        [Fact]
        public async Task GetItemTypePricesAsync_WithItem_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupItem();
            await _fixture.SetupDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<ItemTypePricesContract>>();
            var okResult = (Ok<ItemTypePricesContract>)result;
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class GetItemTypePricesAsyncFixture(DockerFixture dockerFixture) : ItemControllerFixture(dockerFixture)
        {
            private Item? _item;
            public ItemTypePricesContract? ExpectedResult { get; private set; }
            public Guid ItemId { get; } = Guid.NewGuid();
            public Guid StoreId { get; } = Guid.NewGuid();

            public async Task<IResult> ActAsync()
            {
                var scope = CreateServiceScope();

                return await ItemEndpoints.GetItemTypePrices(
                    ItemId,
                    StoreId,
                    scope.ServiceProvider.GetRequiredService<IQueryDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<
                        IToContractConverter<ItemTypePricesReadModel, ItemTypePricesContract>>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    default);
            }

            public void SetupExpectedResult()
            {
                ExpectedResult = new TestBuilder<ItemTypePricesContract>()
                    .FillPropertyWith(x => x.ItemId, ItemId)
                    .FillPropertyWith(x => x.StoreId, StoreId)
                    .Create();
            }

            public void SetupItemWithoutTypes()
            {
                _item = ItemEntityMother.InitialForStore(StoreId).WithId(ItemId).Create();
            }

            public void SetupItemNotAvailableAtStore()
            {
                _item = ItemEntityMother.InitialWithTypes().WithId(ItemId).Create();
            }

            public void SetupItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                var types = ExpectedResult.Prices
                    .Select(p => ItemTypeEntityMother.Initial()
                        .WithAvailableAt(new ItemTypeAvailableAtEntityBuilder().WithPrice(p.Price).WithItemTypeId(p.TypeId).WithStoreId(StoreId).Create())
                        .WithId(p.TypeId)
                        .WithName(p.Name)
                        .Create())
                    .ToList();

                _item = ItemEntityMother.InitialWithTypes()
                    .WithItemTypes(types)
                    .WithId(ItemId)
                    .Create();
            }

            public async Task SetupDatabaseWithoutItemAsync()
            {
                await ApplyMigrationsAsync(ArrangeScope);
            }

            public async Task SetupDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);

                await ApplyMigrationsAsync(ArrangeScope);

                await using var itemContext = GetContextInstance<ItemContext>(ArrangeScope);

                await itemContext.AddAsync(_item);

                await itemContext.SaveChangesAsync();
            }
        }
    }

    public sealed class GetTotalSearchResultCount(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly GetTotalSearchResultCountFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task GetTotalSearchResultCount_WithNoItems_ShouldReturnZero()
        {
            // Arrange
            _fixture.SetupSearchInput();
            _fixture.SetupNoSearchResults();
            _fixture.SetupExpectedResultZero();
            await _fixture.SetupDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<int>>();
            var okResult = (Ok<int>)result;
            okResult.Value.Should().Be(_fixture.ExpectedResult);
        }

        [Fact]
        public async Task GetTotalSearchResultCount_WithSearchResults_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupSearchInput();
            _fixture.SetupSearchResults();
            _fixture.SetupExpectedResult();
            await _fixture.SetupDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<int>>();
            var okResult = (Ok<int>)result;
            okResult.Value.Should().Be(_fixture.ExpectedResult);
        }

        private sealed class GetTotalSearchResultCountFixture(DockerFixture dockerFixture) : ItemControllerFixture(dockerFixture)
        {
            private Item[]? _items;
            public string? SearchInput { get; private set; }
            public int? ExpectedResult { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(SearchInput);

                var scope = CreateServiceScope();
                return await ItemEndpoints.GetTotalSearchResultCount(
                    SearchInput,
                    scope.ServiceProvider.GetRequiredService<IQueryDispatcher>(),
                    default);
            }

            public void SetupSearchInput()
            {
                SearchInput = new DomainTestBuilder<string>().Create();
            }

            public void SetupNoSearchResults()
            {
                _items = [];
            }

            public void SetupSearchResults()
            {
                TestPropertyNotSetException.ThrowIfNull(SearchInput);

                _items =
                [
                    ItemEntityMother.Initial().Create(),
                    ItemEntityMother.Initial().WithName(SearchInput).Create(),
                    ItemEntityMother.Initial().WithName(SearchInput).Create(),
                    ItemEntityMother.Initial().WithName(SearchInput).Create(),
                    ItemEntityMother.Initial().Create()
                ];
            }

            public void SetupExpectedResult()
            {
                ExpectedResult = 3;
            }

            public void SetupExpectedResultZero()
            {
                ExpectedResult = 0;
            }

            public async Task SetupDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_items);

                await ApplyMigrationsAsync(ArrangeScope);

                await using var itemContext = GetContextInstance<ItemContext>(ArrangeScope);
                itemContext.AddRange(_items);
                await itemContext.SaveChangesAsync();
            }
        }
    }

    public sealed class SearchItemsAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly SearchItemsAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task SearchItemsAsync_WithItemNameNotMatching_WithItemNameMatching_WithItemNameMatchingAndNoManufacturer_WithItemNameMatchingButDeleted_WithItemNameMatchingButTemporary_ShouldReturnOneItem()
        {
            // Arrange
            _fixture.SetupSearchInput();
            _fixture.SetupExpectedResult();
            _fixture.SetupItemWithNameNotMatching();
            _fixture.SetupItemWithNameMatching();
            _fixture.SetupItemWithNameMatchingAndNoManufacturer();
            _fixture.SetupItemWithNameMatchingButDeleted();
            _fixture.SetupItemWithNameMatchingButTemporary();
            _fixture.SetupManufacturerNameForItemWithNameMatching();
            await _fixture.SetupDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActWithoutPageSetAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<List<SearchItemResultContract>>>();
            var okResult = (Ok<List<SearchItemResultContract>>)result;
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public async Task SearchItemsAsync_WithPagination_ShouldReturnExpectedResults()
        {
            // Arrange
            _fixture.SetupSearchInput();
            _fixture.SetupPageAndPageSize();
            _fixture.SetupTenMatchingItems();
            _fixture.SetupExpectedResultForTenMatchingItems();
            await _fixture.SetupDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActWithPageSetAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<List<SearchItemResultContract>>>();
            var okResult = (Ok<List<SearchItemResultContract>>)result;
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class SearchItemsAsyncFixture(DockerFixture dockerFixture) : ItemControllerFixture(dockerFixture)
        {
            private readonly List<Item> _items = new();
            private Item? _itemMatching;
            private readonly List<Manufacturer> _manufacturers = new(1);

            public string? SearchInput { get; private set; }
            public int? Page { get; private set; }
            public int? PageSize { get; private set; }
            public List<SearchItemResultContract>? ExpectedResult { get; private set; }

            public async Task<IResult> ActWithPageSetAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(SearchInput);
                TestPropertyNotSetException.ThrowIfNull(Page);
                TestPropertyNotSetException.ThrowIfNull(PageSize);

                var scope = CreateServiceScope();

                return await ItemEndpoints.SearchItems(
                    SearchInput,
                    scope.ServiceProvider.GetRequiredService<IQueryDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<
                        IToContractConverter<SearchItemResultReadModel, SearchItemResultContract>>(),
                    default,
                    Page.Value,
                    PageSize.Value);
            }

            public async Task<IResult> ActWithoutPageSetAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(SearchInput);

                var scope = CreateServiceScope();

                return await ItemEndpoints.SearchItems(
                    SearchInput,
                    scope.ServiceProvider.GetRequiredService<IQueryDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<
                        IToContractConverter<SearchItemResultReadModel, SearchItemResultContract>>(),
                    default);
            }

            public async Task SetupDatabaseAsync()
            {
                await ApplyMigrationsAsync(ArrangeScope);

                await using var itemContext = GetContextInstance<ItemContext>(ArrangeScope);
                await using var manufacturerContext = GetContextInstance<ManufacturerContext>(ArrangeScope);

                await itemContext.AddRangeAsync(_items);
                await manufacturerContext.AddRangeAsync(_manufacturers);

                await itemContext.SaveChangesAsync();
                await manufacturerContext.SaveChangesAsync();
            }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(SearchInput);

                ExpectedResult =
                    new List<SearchItemResultContract>
                    {
                        new DomainTestBuilder<SearchItemResultContract>()
                            .FillConstructorWith(
                                nameof(SearchItemResultContract.ItemName).LowerFirstChar(),
                                "abc" + SearchInput + "def")
                            .Create(),
                        new DomainTestBuilder<SearchItemResultContract>()
                            .FillConstructorWith(
                                nameof(SearchItemResultContract.ItemName).LowerFirstChar(),
                                "abc" + SearchInput + "def")
                            .FillConstructorWith(nameof(SearchItemResultContract.ManufacturerName).LowerFirstChar(),
                                (string)null!)
                            .Create()
                    };
            }

            public void SetupSearchInput()
            {
                SearchInput = new DomainTestBuilder<string>().Create();
            }

            public void SetupPageAndPageSize()
            {
                Page = 2;
                PageSize = 3;
            }

            public void SetupTenMatchingItems()
            {
                TestPropertyNotSetException.ThrowIfNull(SearchInput);

                _items.Add(ItemEntityMother.Initial().WithName($"{SearchInput}A").WithId(Guid.Parse("4915e93b-3b0b-4c43-9299-4d64d8367122")).WithoutManufacturerId().Create());
                _items.Add(ItemEntityMother.Initial().WithName($"{SearchInput}B").WithId(Guid.Parse("4915e93b-3b0b-4c43-9299-4d64d8367123")).WithoutManufacturerId().Create());
                _items.Add(ItemEntityMother.Initial().WithName($"{SearchInput}C").WithId(Guid.Parse("4915e93b-3b0b-4c43-9299-4d64d8367124")).WithoutManufacturerId().Create());
                _items.Add(ItemEntityMother.Initial().WithName($"{SearchInput}D").WithId(Guid.Parse("4915e93b-3b0b-4c43-9299-4d64d8367125")).WithoutManufacturerId().Create());
                _items.Add(ItemEntityMother.Initial().WithName($"{SearchInput}E").WithId(Guid.Parse("4915e93b-3b0b-4c43-9299-4d64d8367126")).WithoutManufacturerId().Create());
                _items.Add(ItemEntityMother.Initial().WithName($"{SearchInput}F").WithId(Guid.Parse("4915e93b-3b0b-4c43-9299-4d64d8367127")).WithoutManufacturerId().Create());
                _items.Add(ItemEntityMother.Initial().WithName($"{SearchInput}F").WithId(Guid.Parse("4915e93b-3b0b-4c43-9299-4d64d8367128")).WithoutManufacturerId().Create());
                _items.Add(ItemEntityMother.Initial().WithName($"{SearchInput}G").WithId(Guid.Parse("4915e93b-3b0b-4c43-9299-4d64d8367129")).WithoutManufacturerId().Create());
                _items.Add(ItemEntityMother.Initial().WithName($"{SearchInput}H").WithId(Guid.Parse("4915e93b-3b0b-4c43-9299-4d64d8367130")).WithoutManufacturerId().Create());
                _items.Add(ItemEntityMother.Initial().WithName($"{SearchInput}I").WithId(Guid.Parse("4915e93b-3b0b-4c43-9299-4d64d8367131")).WithoutManufacturerId().Create());
            }

            public void SetupExpectedResultForTenMatchingItems()
            {
                ExpectedResult = new[] { _items[3], _items[4], _items[5] }
                    .Select(item => new SearchItemResultContract(item.Id, item.Name, null))
                    .ToList();
            }

            public void SetupItemWithNameNotMatching()
            {
                _items.Add(ItemEntityMother.Initial().Create());
            }

            public void SetupItemWithNameMatching()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                var expected = ExpectedResult.First();
                _itemMatching = ItemEntityMother
                    .Initial()
                    .WithName(expected.ItemName)
                    .WithId(expected.ItemId)
                    .Create();
                _items.Add(_itemMatching);
            }

            public void SetupItemWithNameMatchingAndNoManufacturer()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                var expected = ExpectedResult.Last();
                var item = ItemEntityMother
                    .Initial()
                    .WithName(expected.ItemName)
                    .WithId(expected.ItemId)
                    .WithoutManufacturerId()
                    .Create();
                _items.Add(item);
            }

            public void SetupItemWithNameMatchingButDeleted()
            {
                TestPropertyNotSetException.ThrowIfNull(SearchInput);

                var name = "abc" + SearchInput + "def";
                _items.Add(ItemEntityMother.Initial().WithName(name).WithDeleted(true).Create());
            }

            public void SetupItemWithNameMatchingButTemporary()
            {
                TestPropertyNotSetException.ThrowIfNull(SearchInput);

                var name = "abc" + SearchInput + "def";
                _items.Add(ItemEntityMother.Initial().WithName(name).WithIsTemporary(true).Create());
            }

            public void SetupManufacturerNameForItemWithNameMatching()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMatching);
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                var manufacturer = new ManufacturerEntityBuilder()
                    .WithId(_itemMatching.ManufacturerId!.Value)
                    .WithName(ExpectedResult.First().ManufacturerName)
                    .Create();
                _manufacturers.Add(manufacturer);
            }
        }
    }

    public sealed class SearchItemsForShoppingListAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly SearchItemsForShoppingListAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task SearchItemsForShoppingListAsync_WithDeletedItemMatchingCategory_ShouldReturnEmptyList()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupDeletedItemForCategory();
            _fixture.SetupEmptyShoppingList();
            await _fixture.SetupDatabaseAsync();

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContent>();
        }

        [Fact]
        public async Task SearchItemsForShoppingListAsync_WithItemMatchingCategory_ShouldReturnResults()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupItemForCategory();
            _fixture.SetupEmptyShoppingList();
            await _fixture.SetupDatabaseAsync();

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<List<SearchItemForShoppingListResultContract>>>();

            var okResult = (Ok<List<SearchItemForShoppingListResultContract>>)result;
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeAssignableTo<IEnumerable<SearchItemForShoppingListResultContract>>();

            var contract = okResult.Value!.ToList();
            contract.Should().HaveCount(1);
        }

        [Fact]
        public async Task SearchItemsForShoppingListAsync_WithItemWithTypesMatchingCategory_ShouldReturnResults()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupItemWithTypeForCategory();
            _fixture.SetupEmptyShoppingList();
            await _fixture.SetupDatabaseAsync();

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<List<SearchItemForShoppingListResultContract>>>();

            var okResult = (Ok<List<SearchItemForShoppingListResultContract>>)result;
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeAssignableTo<IEnumerable<SearchItemForShoppingListResultContract>>();

            var contract = okResult.Value!.ToList();
            contract.Should().HaveCount(3); // three item types
        }

        [Fact]
        public async Task SearchItemsForShoppingListAsync_WithItemWithTypesMatchingCategoryExceedingLimit_ShouldReturnLimitedResults()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupItemWithTypeForCategoryExceedingLimit();
            _fixture.SetupEmptyShoppingList();
            await _fixture.SetupDatabaseAsync();

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<List<SearchItemForShoppingListResultContract>>>();

            var okResult = (Ok<List<SearchItemForShoppingListResultContract>>)result;
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeAssignableTo<IEnumerable<SearchItemForShoppingListResultContract>>();

            var contract = okResult.Value!.ToList();
            contract.Should().HaveCount(20);
            contract.Should().OnlyContain(c => c.TypeId != null);
        }

        [Fact]
        public async Task SearchItemsForShoppingListAsync_WithItemAndTypeNameMatchingInput_ShouldReturnTypeOnlyOnce()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupEmptyShoppingList();
            _fixture.SetupItemWithTypesWhereItemAndTypeNameMatch();
            _fixture.SetupExpectedResultForItemAndTypeNameMatch();
            await _fixture.SetupDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<List<SearchItemForShoppingListResultContract>>>();

            var okResult = (Ok<List<SearchItemForShoppingListResultContract>>)result;
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeAssignableTo<IEnumerable<SearchItemForShoppingListResultContract>>();

            var contract = okResult.Value!.ToList();
            contract.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public async Task SearchItemsForShoppingListAsync_WithOnlyTypeNameMatchingInput_ShouldReturnType()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupEmptyShoppingList();
            _fixture.SetupItemWithTypesWhereOnlyTypeNameMatch();
            _fixture.SetupExpectedResultForOnlyTypeNameMatch();
            await _fixture.SetupDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<List<SearchItemForShoppingListResultContract>>>();

            var okResult = (Ok<List<SearchItemForShoppingListResultContract>>)result;
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeAssignableTo<IEnumerable<SearchItemForShoppingListResultContract>>();

            var contract = okResult.Value!.ToList();
            contract.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public async Task SearchItemsForShoppingListAsync_WithItemAlreadyOnShoppingList_ShouldReturnEmptyList()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupItemAlreadyOnShoppingList();
            _fixture.SetupShoppingListContainingItem();
            await _fixture.SetupDatabaseAsync();

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContent>();
        }

        [Fact]
        public async Task SearchItemsForShoppingListAsync_WithItemAlreadyOnShoppingList_WithFindingViaCategoryName_ShouldReturnEmptyList()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupItemAlreadyOnShoppingListWithFindingViaItemCategory();
            _fixture.SetupShoppingListContainingItem();
            await _fixture.SetupDatabaseAsync();

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContent>();
        }

        [Fact]
        public async Task SearchItemsForShoppingListAsync_WithItemAndCategoryExceedingLimit_ShouldReturnNoItemTypes()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupEmptyShoppingList();
            _fixture.SetupItemsAndItemCategoriesExceedingLimit();
            await _fixture.SetupDatabaseAsync();

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<List<SearchItemForShoppingListResultContract>>>();

            var okResult = (Ok<List<SearchItemForShoppingListResultContract>>)result;
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeAssignableTo<IEnumerable<SearchItemForShoppingListResultContract>>();

            var contract = okResult.Value!.ToList();
            contract.Should().HaveCount(20);
            contract.Should().OnlyContain(c => c.TypeId == null);
        }

        private sealed class SearchItemsForShoppingListAsyncFixture(DockerFixture dockerFixture)
            : ItemControllerFixture(dockerFixture)
        {
            private readonly List<Item> _items = new();
            private readonly List<ItemCategory> _itemCategories = new();
            private Store? _store;
            private Repositories.ShoppingLists.Entities.ShoppingList? _shoppingList;

            public string SearchInput { get; } = new DomainTestBuilder<string>().Create();
            public Guid StoreId { get; } = Guid.NewGuid();
            public IReadOnlyCollection<SearchItemForShoppingListResultContract>? ExpectedResult { get; private set; }

            public async Task<IResult> ActAsync()
            {
                var scope = CreateServiceScope();
                return await ItemEndpoints.SearchItemsForShoppingList(
                    StoreId,
                    SearchInput,
                    scope.ServiceProvider.GetRequiredService<IQueryDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<
                        IToContractConverter<SearchItemForShoppingResultReadModel,
                            SearchItemForShoppingListResultContract>>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    default);
            }

            public void SetupStore()
            {
                _store = StoreEntityMother.Initial()
                    .WithId(StoreId)
                    .Create();
            }

            public void SetupEmptyShoppingList()
            {
                _shoppingList = ShoppingListEntityMother.Empty()
                    .WithStoreId(StoreId)
                    .Create();
            }

            public void SetupShoppingListContainingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var item = _items.First();
                _shoppingList = ShoppingListEntityMother
                    .InitialWithOneItem(item.Id, null, _store.Sections.First().Id)
                    .WithStoreId(StoreId)
                    .Create();
            }

            public void SetupDeletedItemForCategory()
            {
                SetupItemForCategory(true);
            }

            public void SetupItemForCategory(bool isDeleted = false)
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var category = new ItemCategoryEntityBuilder()
                    .WithName(SearchInput)
                    .WithDeleted(false)
                    .Create();
                var item = ItemEntityMother.InitialForStore(StoreId, _store.Sections.First().Id)
                    .WithItemCategoryId(category.Id)
                    .WithoutManufacturerId()
                    .WithDeleted(isDeleted)
                    .Create();

                _items.Add(item);
                _itemCategories.Add(category);
            }

            public void SetupItemWithTypesWhereItemAndTypeNameMatch()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var availability = new ItemTypeAvailableAtEntityBuilder()
                    .WithStoreId(StoreId)
                    .WithDefaultSectionId(_store.Sections.First().Id)
                    .Create();
                var itemTypes = ItemTypeEntityMother
                    .Initial()
                    .WithName("ABC" + SearchInput)
                    .WithAvailableAt([availability])
                    .CreateMany(1)
                    .ToList();
                var item = ItemEntityMother
                    .InitialWithTypes()
                    .WithoutManufacturerId()
                    .WithName(SearchInput + "12355")
                    .WithItemTypes(itemTypes)
                    .Create();
                _items.Add(item);

                var itemCategory = new ItemCategoryEntityBuilder()
                    .WithDeleted(false)
                    .WithId(item.ItemCategoryId!.Value)
                    .Create();
                _itemCategories.Add(itemCategory);
            }

            public void SetupItemWithTypesWhereOnlyTypeNameMatch()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var availability = new ItemTypeAvailableAtEntityBuilder()
                    .WithStoreId(StoreId)
                    .WithDefaultSectionId(_store.Sections.First().Id)
                    .Create();
                var itemTypes = ItemTypeEntityMother
                    .Initial()
                    .WithName("ABC4732" + SearchInput)
                    .WithAvailableAt([availability])
                    .CreateMany(1)
                    .ToList();
                var item = ItemEntityMother
                    .InitialWithTypes()
                    .WithoutManufacturerId()
                    .WithItemTypes(itemTypes)
                    .Create();
                _items.Add(item);

                var itemCategory = new ItemCategoryEntityBuilder()
                    .WithDeleted(false)
                    .WithId(item.ItemCategoryId!.Value)
                    .Create();
                _itemCategories.Add(itemCategory);
            }

            public void SetupItemAlreadyOnShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var availability = new AvailableAtEntityBuilder()
                    .WithStoreId(StoreId)
                    .WithDefaultSectionId(_store.Sections.First().Id)
                    .Create();
                var item = ItemEntityMother.Initial()
                    .WithName("Item" + SearchInput)
                    .WithAvailableAt([availability])
                    .Create();
                _items.Add(item);
            }

            public void SetupItemAlreadyOnShoppingListWithFindingViaItemCategory()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var availability = new AvailableAtEntityBuilder()
                    .WithStoreId(StoreId)
                    .WithDefaultSectionId(_store.Sections.First().Id)
                    .Create();
                var item = ItemEntityMother.Initial()
                    .WithoutManufacturerId()
                    .WithAvailableAt([availability])
                    .Create();
                _items.Add(item);

                var category = new ItemCategoryEntityBuilder()
                    .WithDeleted(false)
                    .WithId(item.ItemCategoryId!.Value)
                    .WithName("Cat" + SearchInput)
                    .Create();

                _itemCategories.Add(category);
            }

            public void SetupItemsAndItemCategoriesExceedingLimit()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var itemCategoryWithName = new ItemCategoryEntityBuilder().WithName(SearchInput).WithDeleted(false).Create();
                var itemCategory = new ItemCategoryEntityBuilder().WithDeleted(false).Create();
                _itemCategories.Add(itemCategoryWithName);
                _itemCategories.Add(itemCategory);

                var itemsWithCategory = Enumerable.Range(0, 13)
                    .Select(_ => ItemEntityMother.Initial().WithAvailableAt(CreateAvailabilities())
                        .WithoutManufacturerId().WithItemCategoryId(itemCategoryWithName.Id).Create())
                    .ToList();
                _items.AddRange(itemsWithCategory);

                var items = Enumerable.Range(0, 8)
                    .Select(_ => ItemEntityMother.Initial().WithName(SearchInput + "X").WithAvailableAt(CreateAvailabilities())
                        .WithItemCategoryId(itemCategory.Id).WithoutManufacturerId().Create())
                    .ToList();
                var itemsWithTypes = Enumerable.Range(0, 2)
                    .Select(_ => ItemEntityMother.InitialWithTypes()
                        .WithItemTypes(CreateItemTypes()).WithItemCategoryId(itemCategory.Id).WithoutManufacturerId().Create())
                    .ToList();
                _items.AddRange(items);
                _items.AddRange(itemsWithTypes);

                IList<AvailableAt> CreateAvailabilities()
                {
                    return new AvailableAtEntityBuilder()
                        .WithStoreId(StoreId)
                        .WithDefaultSectionId(_store.Sections.First().Id)
                        .CreateMany(1)
                        .ToList();
                }

                IList<ItemType> CreateItemTypes()
                {
                    var typeAvailabilities = new ItemTypeAvailableAtEntityBuilder()
                        .WithStoreId(StoreId)
                        .WithDefaultSectionId(_store.Sections.First().Id)
                        .CreateMany(1)
                        .ToList();
                    return ItemTypeEntityMother
                        .Initial()
                        .WithName(SearchInput + "Y")
                        .WithAvailableAt(typeAvailabilities)
                        .CreateMany(1)
                        .ToList();
                }
            }

            public void SetupItemWithTypeForCategory()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var category = new ItemCategoryEntityBuilder()
                    .WithName(SearchInput)
                    .WithDeleted(false)
                    .Create();
                var item = ItemEntityMother.InitialWithTypesForStore(StoreId, _store.Sections.First().Id)
                    .WithItemCategoryId(category.Id)
                    .WithoutManufacturerId()
                    .Create();

                _items.Add(item);
                _itemCategories.Add(category);
            }

            public void SetupItemWithTypeForCategoryExceedingLimit()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var category = new ItemCategoryEntityBuilder()
                    .WithName(SearchInput)
                    .WithDeleted(false)
                    .Create();
                var items = Enumerable.Range(0, 7)
                    .Select(_ => ItemEntityMother.InitialWithTypesForStore(StoreId, _store.Sections.First().Id)
                        .WithItemCategoryId(category.Id)
                        .WithoutManufacturerId()
                        .Create())
                    .ToList();

                _items.AddRange(items);
                _itemCategories.Add(category);
            }

            public void SetupExpectedResultForItemAndTypeNameMatch()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var item = _items.First();
                var quantityType = item.QuantityType.ToEnum<QuantityType>();
                var availability = item.ItemTypes.First().AvailableAt.First(av => av.StoreId == StoreId);
                var section = _store.Sections.First(s => s.Id == availability.DefaultSectionId);

                ExpectedResult = [new SearchItemForShoppingListResultContract(
                        item.Id,
                        item.ItemTypes.First().Id,
                        $"{item.Name} {item.ItemTypes.First().Name}",
                        quantityType.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                        availability.Price,
                        quantityType.GetAttribute<PriceLabelAttribute>().PriceLabel,
                        _itemCategories.First().Name,
                        "",
                        new SectionContract(section.Id, section.Name, section.SortIndex, section.IsDefaultSection))
                    ];
            }

            public void SetupExpectedResultForOnlyTypeNameMatch()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var item = _items.First();
                var quantityType = item.QuantityType.ToEnum<QuantityType>();
                var availability = item.ItemTypes.First().AvailableAt.First(av => av.StoreId == StoreId);
                var section = _store.Sections.First(s => s.Id == availability.DefaultSectionId);

                ExpectedResult = [new SearchItemForShoppingListResultContract(
                        item.Id,
                        item.ItemTypes.First().Id,
                        $"{item.Name} {item.ItemTypes.First().Name}",
                        quantityType.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                        availability.Price,
                        quantityType.GetAttribute<PriceLabelAttribute>().PriceLabel,
                        _itemCategories.First().Name,
                        "",
                        new SectionContract(section.Id, section.Name, section.SortIndex, section.IsDefaultSection))
                    ];
            }

            public async Task SetupDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);

                await ApplyMigrationsAsync(ArrangeScope);

                await using var itemContext = GetContextInstance<ItemContext>(ArrangeScope);
                await using var itemCategoryContext = GetContextInstance<ItemCategoryContext>(ArrangeScope);
                await using var storeContext = GetContextInstance<StoreContext>(ArrangeScope);
                await using var shoppingListContext = GetContextInstance<ShoppingListContext>(ArrangeScope);

                await itemCategoryContext.AddRangeAsync(_itemCategories);
                await itemContext.AddRangeAsync(_items);
                await storeContext.AddAsync(_store);
                await shoppingListContext.AddAsync(_shoppingList);

                await itemContext.SaveChangesAsync();
                await itemCategoryContext.SaveChangesAsync();
                await storeContext.SaveChangesAsync();
                await shoppingListContext.SaveChangesAsync();
            }
        }
    }

    public sealed class SearchItemsByItemCategoryAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly SearchItemsByItemCategoryAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task SearchItemsByItemCategoryAsync_WithoutTypes_WithMatchingItemCategoryAndManufacturer_WithMatchingItemCategoryAndNoManufacturer_WithNotMatchingItemCategory_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemCategory();
            _fixture.SetupManufacturer();
            _fixture.SetupItemsWithAndWithoutItemCategory();
            await _fixture.PrepareDatabaseAsync();
            _fixture.SetupExpectedResult();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Ok<List<SearchItemByItemCategoryResultContract>>>();
            var resultValue = (result as Ok<List<SearchItemByItemCategoryResultContract>>)!.Value;
            resultValue.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class SearchItemsByItemCategoryAsyncFixture(DockerFixture dockerFixture)
            : ItemControllerFixture(dockerFixture)
        {
            private Item? _item;
            private Item? _itemWithoutManufacturer;
            private Item? _itemWithoutItemCategory;
            private ItemCategory? _itemCategory;
            private Store? _store;
            private Manufacturer? _manufacturer;

            public Guid? ItemCategoryId => _itemCategory?.Id;
            public IReadOnlyCollection<SearchItemByItemCategoryResultContract>? ExpectedResult { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemCategoryId);

                var scope = CreateServiceScope();
                return await ItemEndpoints.SearchItemsByItemCategory(
                    ItemCategoryId.Value,
                    scope.ServiceProvider.GetRequiredService<IQueryDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<
                        IToContractConverter<SearchItemByItemCategoryResult, SearchItemByItemCategoryResultContract>>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    default);
            }

            public void SetupItemCategory()
            {
                _itemCategory = new ItemCategoryEntityBuilder()
                    .WithDeleted(false)
                    .Create();
            }

            public void SetupManufacturer()
            {
                _manufacturer = new ManufacturerEntityBuilder()
                    .Create();
            }

            public void SetupItemsWithAndWithoutItemCategory()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                TestPropertyNotSetException.ThrowIfNull(_manufacturer);

                var sections = new List<Section>()
                {
                    new SectionEntityBuilder().WithIsDefaultSection(true).WithSortIndex(0).Create(),
                    new SectionEntityBuilder().WithIsDefaultSection(false).WithSortIndex(1).Create()
                };
                _store = StoreEntityMother.Initial().WithSections(sections).Create();

                var defaultSectionId = CommonFixture.ChooseRandom(_store.Sections).Id;

                _item =
                    ItemEntityMother.Initial().WithItemCategoryId(_itemCategory.Id)
                        .WithManufacturerId(_manufacturer.Id)
                        .WithAvailableAt(AvailableAtEntityMother
                            .InitialForStore(_store.Id)
                            .WithDefaultSectionId(defaultSectionId)
                            .CreateMany(1)
                            .ToList())
                        .Create();
                _itemWithoutManufacturer =
                    ItemEntityMother.Initial().WithItemCategoryId(_itemCategory.Id)
                        .WithoutManufacturerId()
                        .WithAvailableAt(AvailableAtEntityMother
                            .InitialForStore(_store.Id)
                            .WithDefaultSectionId(defaultSectionId)
                            .CreateMany(1)
                            .ToList())
                        .Create();
                _itemWithoutItemCategory = ItemEntityMother.Initial().Create();
            }

            public async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);
                TestPropertyNotSetException.ThrowIfNull(_itemWithoutItemCategory);
                TestPropertyNotSetException.ThrowIfNull(_itemWithoutManufacturer);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                TestPropertyNotSetException.ThrowIfNull(_store);
                TestPropertyNotSetException.ThrowIfNull(_manufacturer);

                await ApplyMigrationsAsync(ArrangeScope);

                var itemContext = ArrangeScope.ServiceProvider.GetRequiredService<ItemContext>();
                var itemCategoryContext = ArrangeScope.ServiceProvider.GetRequiredService<ItemCategoryContext>();
                var manufacturerContext = ArrangeScope.ServiceProvider.GetRequiredService<ManufacturerContext>();
                var storeContext = ArrangeScope.ServiceProvider.GetRequiredService<StoreContext>();

                await itemCategoryContext.AddAsync(_itemCategory);
                await storeContext.AddAsync(_store);
                await itemContext.AddAsync(_item);
                await itemContext.AddAsync(_itemWithoutManufacturer);
                await itemContext.AddAsync(_itemWithoutItemCategory);
                await manufacturerContext.AddAsync(_manufacturer);

                await itemCategoryContext.SaveChangesAsync();
                await itemContext.SaveChangesAsync();
                await storeContext.SaveChangesAsync();
                await manufacturerContext.SaveChangesAsync();
            }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);
                TestPropertyNotSetException.ThrowIfNull(_item);
                TestPropertyNotSetException.ThrowIfNull(_itemWithoutManufacturer);
                TestPropertyNotSetException.ThrowIfNull(_manufacturer);

                ExpectedResult = new List<SearchItemByItemCategoryResultContract>(2)
                {
                    new(
                        _item.Id,
                        null,
                        _item.Name,
                        _manufacturer.Name,
                        _item.AvailableAt.Select(av =>
                            new SearchItemByItemCategoryAvailabilityContract(
                                _store.Id,
                                _store.Name,
                                av.Price))),
                    new(
                        _itemWithoutManufacturer.Id,
                        null,
                        _itemWithoutManufacturer.Name,
                        null,
                        _itemWithoutManufacturer.AvailableAt.Select(av =>
                            new SearchItemByItemCategoryAvailabilityContract(
                                _store.Id,
                                _store.Name,
                                av.Price)))
                };
            }
        }
    }

    public sealed class CreateItemAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly CreateItemAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task CreateItemAsync_WithValidData_ShouldCreateItem()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupExpectedDbResult();
            _fixture.SetupContract();
            _fixture.SetupStores();
            _fixture.SetupItemCategory();
            _fixture.SetupManufacturer();
            await _fixture.PrepareDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedDbResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CreatedAtRoute<ItemContract>>();
            var createdResult = (CreatedAtRoute<ItemContract>)result;
            var itemContract = createdResult.Value!;

            itemContract.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt
                .Excluding(info => info.Path == "Id"));

            using var assertionScope = _fixture.CreateServiceScope();

            var items = (await _fixture.LoadAllItemsAsync(assertionScope)).ToList();
            items.Should().HaveCount(1);
            var item = items.Single();
            item.Should().BeEquivalentTo(_fixture.ExpectedDbResult, opt => opt
                .ExcludeItemCycleRef()
                .ExcludeRowVersion()
                .WithCreatedAtPrecision(5.Minutes())
                .Excluding(info => info.Path == "Id"));
            item.Id.Should().Be(itemContract.Id);
        }

        private sealed class CreateItemAsyncFixture(DockerFixture dockerFixture) : ItemControllerFixture(dockerFixture)
        {
            private IReadOnlyCollection<Store>? _stores;
            private ItemCategory? _itemCategory;
            private Manufacturer? _manufacturer;
            public CreateItemContract? Contract { get; private set; }
            public ItemContract? ExpectedResult { get; private set; }
            public Item? ExpectedDbResult { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);

                var scope = CreateServiceScope();
                return await ItemEndpoints.CreateItem(
                    Contract,
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    scope.ServiceProvider.GetRequiredService<IToDomainConverter<CreateItemContract, ItemCreation>>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<ItemReadModel, ItemContract>>(),
                    default);
            }

            public void SetupExpectedResult()
            {
                var manufacturer = new ManufacturerContractBuilder()
                    .WithIsDeleted(false)
                    .Create();

                var itemCategory = new ItemCategoryContractBuilder()
                    .WithIsDeleted(false)
                    .Create();

                Contracts.Items.Queries.Get.ItemAvailabilityContract[] availabilities =
                [
                    ItemAvailabilityContractMother.Valid().Create(),
                    ItemAvailabilityContractMother.Valid().Create()
                ];

                ExpectedResult = ItemContractMother.Valid()
                    .WithIsDeleted(false)
                    .WithIsTemporary(false)
                    .WithManufacturer(manufacturer)
                    .WithItemCategory(itemCategory)
                    .WithAvailabilities(availabilities)
                    .WithEmptyItemTypes()
                    .Create();
            }

            public void SetupExpectedDbResult()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                ExpectedDbResult = new Item()
                {
                    Id = Guid.NewGuid(),
                    Name = ExpectedResult.Name,
                    Deleted = false,
                    Comment = ExpectedResult.Comment,
                    IsTemporary = false,
                    QuantityType = ExpectedResult.QuantityType.Id,
                    QuantityInPacket = ExpectedResult.QuantityInPacket,
                    QuantityTypeInPacket = ExpectedResult.QuantityTypeInPacket?.Id,
                    ItemCategoryId = ExpectedResult.ItemCategory.Id,
                    ManufacturerId = ExpectedResult.Manufacturer.Id,
                    CreatedFrom = null,
                    UpdatedOn = null,
                    PredecessorId = null,
                    CreatedAt = DateTime.UtcNow,
                    Predecessor = null,
                    ItemTypes = [],
                    AvailableAt = ExpectedResult.Availabilities
                        .Select(av => new AvailableAt()
                        {
                            ItemId = Guid.NewGuid(),
                            StoreId = av.Store.Id,
                            DefaultSectionId = av.DefaultSection.Id,
                            Price = av.Price
                        })
                        .ToList()
                };
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                Contract = new CreateItemContract
                {
                    Name = ExpectedResult.Name,
                    Comment = ExpectedResult.Comment,
                    QuantityType = ExpectedResult.QuantityType.Id,
                    QuantityInPacket = ExpectedResult.QuantityInPacket,
                    QuantityTypeInPacket = ExpectedResult.QuantityTypeInPacket?.Id,
                    ItemCategoryId = ExpectedResult.ItemCategory.Id,
                    ManufacturerId = ExpectedResult.Manufacturer.Id,
                    Availabilities = ExpectedResult.Availabilities
                        .Select(av => new ItemAvailabilityContract
                        {
                            DefaultSectionId = av.DefaultSection.Id,
                            StoreId = av.Store.Id,
                            Price = av.Price
                        })
                        .ToList()
                };
            }

            public async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_stores);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                TestPropertyNotSetException.ThrowIfNull(_manufacturer);

                await ApplyMigrationsAsync(ArrangeScope);

                var storeContext = ArrangeScope.ServiceProvider.GetRequiredService<StoreContext>();
                var itemCategoryContext = ArrangeScope.ServiceProvider.GetRequiredService<ItemCategoryContext>();
                var manufacturerContext = ArrangeScope.ServiceProvider.GetRequiredService<ManufacturerContext>();

                await storeContext.AddRangeAsync(_stores);
                await itemCategoryContext.AddAsync(_itemCategory);
                await manufacturerContext.AddAsync(_manufacturer);

                await storeContext.SaveChangesAsync();
                await itemCategoryContext.SaveChangesAsync();
                await manufacturerContext.SaveChangesAsync();
            }

            public void SetupStores()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                _stores = ExpectedResult.Availabilities
                    .Select(av =>
                    {
                        var sections = new Section[av.Store.Sections.Count];
                        for (int i = 0; i < av.Store.Sections.Count; i++)
                        {
                            var section = av.Store.Sections.ElementAt(i);
                            var builder = i == 0 ? SectionEntityMother.Default() : SectionEntityMother.NotDefault();

                            sections[i] = builder.WithName(section.Name).WithSortIndex(section.SortingIndex)
                                .WithId(section.Id).Create();
                        }

                        return StoreEntityMother.Active().WithId(av.Store.Id).WithName(av.Store.Name).WithSections(sections)
                            .Create();
                    })
                    .ToList();
            }

            public void SetupItemCategory()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                _itemCategory = ItemCategoryEntityMother.Active()
                    .WithId(ExpectedResult.ItemCategory.Id)
                    .WithName(ExpectedResult.ItemCategory.Name)
                    .Create();
            }

            public void SetupManufacturer()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                _manufacturer = ManufacturerEntityMother.Active()
                    .WithId(ExpectedResult.Manufacturer.Id)
                    .WithName(ExpectedResult.Manufacturer.Name)
                    .Create();
            }
        }
    }

    public sealed class CreateItemWithTypesAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly CreateItemWithTypesAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task CreateItemWithTypesAsync_WithValidData_ShouldCreateItemWithTypes()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupExpectedDbResult();
            _fixture.SetupContract();
            _fixture.SetupStores();
            _fixture.SetupItemCategory();
            _fixture.SetupManufacturer();
            await _fixture.PrepareDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedDbResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CreatedAtRoute<ItemContract>>();
            var createdResult = (CreatedAtRoute<ItemContract>)result;
            var itemContract = createdResult.Value!;

            itemContract.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt
                .ExcludeItemTypeId()
                .Excluding(info => info.Path == "Id"));

            using var assertionScope = _fixture.CreateServiceScope();

            var items = (await _fixture.LoadAllItemsAsync(assertionScope)).ToList();
            items.Should().HaveCount(1);
            var item = items.Single();
            item.Should().BeEquivalentTo(_fixture.ExpectedDbResult, opt => opt
                .ExcludeItemCycleRef()
                .ExcludeRowVersion()
                .WithCreatedAtPrecision(5.Minutes())
                .ExcludeItemTypeId()
                .Excluding(info => info.Path == "Id"));
            item.Id.Should().Be(itemContract.Id);
        }

        private sealed class CreateItemWithTypesAsyncFixture(DockerFixture dockerFixture) : ItemControllerFixture(dockerFixture)
        {
            private IReadOnlyCollection<Store>? _stores;
            private ItemCategory? _itemCategory;
            private Manufacturer? _manufacturer;
            public CreateItemWithTypesContract? Contract { get; private set; }
            public ItemContract? ExpectedResult { get; private set; }
            public Item? ExpectedDbResult { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);

                var scope = CreateServiceScope();
                return await ItemEndpoints.CreateItemWithTypes(
                    Contract,
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    scope.ServiceProvider.GetRequiredService<IToDomainConverter<CreateItemWithTypesContract, IItem>>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<ItemReadModel, ItemContract>>(),
                    default);
            }

            public void SetupExpectedResult()
            {
                var manufacturer = new ManufacturerContractBuilder()
                    .WithIsDeleted(false)
                    .Create();

                var itemCategory = new ItemCategoryContractBuilder()
                    .WithIsDeleted(false)
                    .Create();

                ItemTypeContract[] itemTypes =
                [
                    ItemTypeContractMother.Valid().Create(),
                    ItemTypeContractMother.Valid().Create()
                ];

                ExpectedResult = ItemContractMother.Valid()
                    .WithIsDeleted(false)
                    .WithIsTemporary(false)
                    .WithManufacturer(manufacturer)
                    .WithItemCategory(itemCategory)
                    .WithEmptyAvailabilities()
                    .WithItemTypes(itemTypes)
                    .Create();
            }

            public void SetupExpectedDbResult()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                ExpectedDbResult = new Item
                {
                    Id = Guid.NewGuid(),
                    Name = ExpectedResult.Name,
                    Deleted = false,
                    Comment = ExpectedResult.Comment,
                    IsTemporary = false,
                    QuantityType = ExpectedResult.QuantityType.Id,
                    QuantityInPacket = ExpectedResult.QuantityInPacket,
                    QuantityTypeInPacket = ExpectedResult.QuantityTypeInPacket?.Id,
                    ItemCategoryId = ExpectedResult.ItemCategory.Id,
                    ManufacturerId = ExpectedResult.Manufacturer.Id,
                    CreatedFrom = null,
                    UpdatedOn = null,
                    PredecessorId = null,
                    CreatedAt = DateTime.UtcNow,
                    Predecessor = null,
                    ItemTypes = ExpectedResult.ItemTypes
                        .Select(t => new ItemType
                        {
                            Id = Guid.NewGuid(),
                            Name = t.Name,
                            IsDeleted = false,
                            CreatedAt = DateTime.UtcNow,
                            ItemId = ExpectedResult.Id,
                            PredecessorId = null,
                            Predecessor = null,
                            AvailableAt = t.Availabilities
                                .Select(av => new ItemTypeAvailableAt
                                {
                                    ItemTypeId = Guid.NewGuid(),
                                    StoreId = av.Store.Id,
                                    DefaultSectionId = av.DefaultSection.Id,
                                    Price = av.Price
                                })
                                .ToList()
                        })
                        .ToList(),
                    AvailableAt = []
                };
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                Contract = new CreateItemWithTypesContract
                {
                    Name = ExpectedResult.Name,
                    Comment = ExpectedResult.Comment,
                    QuantityType = ExpectedResult.QuantityType.Id,
                    QuantityInPacket = ExpectedResult.QuantityInPacket,
                    QuantityTypeInPacket = ExpectedResult.QuantityTypeInPacket?.Id,
                    ItemCategoryId = ExpectedResult.ItemCategory.Id,
                    ManufacturerId = ExpectedResult.Manufacturer.Id,
                    ItemTypes = ExpectedResult.ItemTypes
                        .Select(t => new CreateItemTypeContract
                        {
                            Name = t.Name,
                            Availabilities = t.Availabilities
                                .Select(av => new ItemAvailabilityContract
                                {
                                    DefaultSectionId = av.DefaultSection.Id,
                                    Price = av.Price,
                                    StoreId = av.Store.Id
                                })
                        })
                        .ToList(),
                    //Availabilities = ExpectedResult.Availabilities
                    //    .Select(av => new ItemWithTypesAvailabilityContract
                    //    {
                    //        DefaultSectionId = av.DefaultSection.Id,
                    //        StoreId = av.Store.Id,
                    //        Price = av.Price
                    //    })
                    //    .ToList()
                };
            }

            public async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_stores);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                TestPropertyNotSetException.ThrowIfNull(_manufacturer);

                await ApplyMigrationsAsync(ArrangeScope);

                var storeContext = ArrangeScope.ServiceProvider.GetRequiredService<StoreContext>();
                var itemCategoryContext = ArrangeScope.ServiceProvider.GetRequiredService<ItemCategoryContext>();
                var manufacturerContext = ArrangeScope.ServiceProvider.GetRequiredService<ManufacturerContext>();

                await storeContext.AddRangeAsync(_stores);
                await itemCategoryContext.AddAsync(_itemCategory);
                await manufacturerContext.AddAsync(_manufacturer);

                await storeContext.SaveChangesAsync();
                await itemCategoryContext.SaveChangesAsync();
                await manufacturerContext.SaveChangesAsync();
            }

            public void SetupStores()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                _stores = ExpectedResult.ItemTypes.SelectMany(t => t.Availabilities)
                    .Select(av =>
                    {
                        var sections = new Section[av.Store.Sections.Count];
                        for (int i = 0; i < av.Store.Sections.Count; i++)
                        {
                            var section = av.Store.Sections.ElementAt(i);
                            var builder = i == 0 ? SectionEntityMother.Default() : SectionEntityMother.NotDefault();

                            sections[i] = builder.WithName(section.Name).WithSortIndex(section.SortingIndex)
                                .WithId(section.Id).Create();
                        }

                        return StoreEntityMother.Active().WithId(av.Store.Id).WithName(av.Store.Name).WithSections(sections)
                            .Create();
                    })
                    .ToList();
            }

            public void SetupItemCategory()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                _itemCategory = ItemCategoryEntityMother.Active()
                    .WithId(ExpectedResult.ItemCategory.Id)
                    .WithName(ExpectedResult.ItemCategory.Name)
                    .Create();
            }

            public void SetupManufacturer()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                _manufacturer = ManufacturerEntityMother.Active()
                    .WithId(ExpectedResult.Manufacturer.Id)
                    .WithName(ExpectedResult.Manufacturer.Name)
                    .Create();
            }
        }
    }

    public sealed class ModifyItemAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly ModifyItemAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task ModifyItemAsync_WithValidData_ShouldModifyItemAndChangeRecipe()
        {
            // Arrange
            _fixture.SetupExistingItem();
            _fixture.SetupExpectedItem();
            _fixture.SetupExpectedRecipe();
            _fixture.SetupExistingRecipe();
            _fixture.SetupContract();
            await _fixture.PrepareDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExistingItem);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            using var assertionScope = _fixture.CreateServiceScope();

            result.Should().BeOfType<NoContent>();

            var items = (await _fixture.LoadAllItemsAsync(assertionScope)).ToList();
            items.Should().HaveCount(1);
            var item = items.SingleOrDefault(i => i.Id == _fixture.ExistingItem.Id);
            item.Should().NotBeNull();
            item.Should().BeEquivalentTo(_fixture.ExpectedItem, opt => opt
                .ExcludeItemCycleRef()
                .ExcludeRowVersion()
                .WithCreatedAtPrecision());

            var recipes = (await _fixture.LoadAllRecipesAsync(assertionScope)).ToList();
            recipes.Should().HaveCount(1);
            var recipe = recipes.First();
            recipe.Should().BeEquivalentTo(_fixture.ExpectedRecipe,
                opt => opt.ExcludeRecipeCycleRef().ExcludeRowVersion().WithCreatedAtPrecision());
        }

        private sealed class ModifyItemAsyncFixture(DockerFixture dockerFixture) : ItemControllerFixture(dockerFixture)
        {
            private AvailableAt? _existingAvailability;
            private AvailableAt? _expectedAvailability;
            private Recipe? _existingRecipe;

            public Item? ExistingItem { get; private set; }
            public Item? ExpectedItem { get; private set; }
            public Recipe? ExpectedRecipe { get; private set; }
            public ModifyItemContract? Contract { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);
                TestPropertyNotSetException.ThrowIfNull(ExistingItem);

                var scope = CreateServiceScope();
                return await ItemEndpoints.ModifyItem(
                    ExistingItem.Id,
                    Contract,
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    scope.ServiceProvider.GetRequiredService<IToDomainConverter<(Guid, ModifyItemContract), ModifyItemCommand>>(),
                    default);
            }

            public void SetupExistingItem()
            {
                _existingAvailability = new AvailableAtEntityBuilder().Create();
                ExistingItem = ItemEntityMother
                    .Initial()
                    .WithAvailableAt(_existingAvailability)
                    .Create();
            }

            public void SetupExpectedItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingItem);

                _expectedAvailability = new AvailableAtEntityBuilder().Create();

                ExpectedItem = ItemEntityMother
                    .Initial()
                    .WithId(ExistingItem.Id)
                    .WithAvailableAt(_expectedAvailability)
                    .WithCreatedAt(ExistingItem.CreatedAt)
                    .Create();
            }

            public void SetupExpectedRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedAvailability);
                TestPropertyNotSetException.ThrowIfNull(ExpectedItem);

                var ingredient = new IngredientEntityBuilder()
                    .WithDefaultItemId(ExpectedItem.Id)
                    .WithoutDefaultItemTypeId()
                    .WithDefaultStoreId(_expectedAvailability.StoreId)
                    .Create();
                ExpectedRecipe = new RecipeEntityBuilder()
                    .WithoutSideDishId()
                    .WithIngredient(ingredient)
                    .Create();
            }

            public void SetupExistingRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedRecipe);
                TestPropertyNotSetException.ThrowIfNull(ExistingItem);
                TestPropertyNotSetException.ThrowIfNull(_existingAvailability);

                _existingRecipe = ExpectedRecipe.DeepClone();
                _existingRecipe.Ingredients.First().DefaultStoreId = _existingAvailability.StoreId;
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingItem);
                TestPropertyNotSetException.ThrowIfNull(ExpectedItem);
                TestPropertyNotSetException.ThrowIfNull(_expectedAvailability);

                Contract = new ModifyItemContract(
                    ExpectedItem.Name,
                    ExpectedItem.Comment,
                    ExpectedItem.QuantityType,
                    ExpectedItem.QuantityInPacket,
                    ExpectedItem.QuantityTypeInPacket,
                    ExpectedItem.ItemCategoryId!.Value,
                    ExpectedItem.ManufacturerId,
                    [new ItemAvailabilityContract
                    {
                        StoreId = _expectedAvailability.StoreId,
                        DefaultSectionId = _expectedAvailability.DefaultSectionId,
                        Price = _expectedAvailability.Price
                    }]);
            }

            public async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingItem);
                TestPropertyNotSetException.ThrowIfNull(ExpectedItem);
                TestPropertyNotSetException.ThrowIfNull(_existingRecipe);

                await ApplyMigrationsAsync(ArrangeScope);

                var itemDatabaseService = new ItemEntityDatabaseService(this);
                await itemDatabaseService.CreateReferencesAsync(ExistingItem, ExpectedItem);
                await itemDatabaseService.SaveAsync(ExistingItem);

                await using var recipeDbContext = GetContextInstance<RecipeContext>(ArrangeScope);
                await recipeDbContext.AddAsync(_existingRecipe);
                await recipeDbContext.SaveChangesAsync();
            }
        }
    }

    public sealed class ModifyItemWithTypesAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly ModifyItemWithTypesAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task ModifyItemWithTypesAsync_WithRemovingType_ShouldMarkTypeAsDeleted()
        {
            // Arrange
            _fixture.SetupExistingItem();
            _fixture.SetupExpectedItem();
            _fixture.SetupExpectedRecipe();
            _fixture.SetupExistingRecipe();
            _fixture.SetupContractWithLessItemTypes();
            await _fixture.PrepareDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExistingItem);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            using var assertionScope = _fixture.CreateServiceScope();

            result.Should().BeOfType<NoContent>();

            var items = (await _fixture.LoadAllItemsAsync(assertionScope)).ToList();
            items.Should().HaveCount(1);
            var item = items.SingleOrDefault(i => i.Id == _fixture.ExistingItem.Id);
            item.Should().NotBeNull();
            item.Should().BeEquivalentTo(_fixture.ExpectedItem, opt => opt
                .Excluding(info => info.Path == "UpdatedOn")
                .ExcludeItemCycleRef()
                .ExcludeRowVersion()
                .WithCreatedAtPrecision());

            var recipes = (await _fixture.LoadAllRecipesAsync(assertionScope)).ToList();
            recipes.Should().HaveCount(1);
            var recipe = recipes.First();
            recipe.Should().BeEquivalentTo(_fixture.ExpectedRecipe,
                opt => opt.ExcludeRecipeCycleRef().ExcludeRowVersion().WithCreatedAtPrecision());
        }

        private sealed class ModifyItemWithTypesAsyncFixture(DockerFixture dockerFixture)
            : ItemControllerFixture(dockerFixture)
        {
            private ItemTypeAvailableAt? _existingAvailability;
            private ItemTypeAvailableAt? _expectedAvailability;
            private Recipe? _existingRecipe;

            public Item? ExistingItem { get; private set; }
            public Item? ExpectedItem { get; private set; }
            public Recipe? ExpectedRecipe { get; private set; }
            public ModifyItemWithTypesContract? Contract { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);
                TestPropertyNotSetException.ThrowIfNull(ExistingItem);

                var scope = CreateServiceScope();
                return await ItemEndpoints.ModifyItemWithTypes(
                    ExistingItem.Id,
                    Contract,
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    scope.ServiceProvider.GetRequiredService<
                        IToDomainConverter<(Guid, ModifyItemWithTypesContract), ModifyItemWithTypesCommand>>(),
                    default);
            }
            public void SetupExistingItem()
            {
                _existingAvailability = new ItemTypeAvailableAtEntityBuilder().Create();
                ExistingItem = ItemEntityMother
                    .InitialWithTypes()
                    .WithItemTypes(new List<ItemType>
                    {
                        ItemTypeEntityMother.Initial().WithAvailableAt(_existingAvailability).Create(),
                        ItemTypeEntityMother.Initial().Create()
                    })
                    .Create();

                foreach (var type in ExistingItem.ItemTypes)
                {
                    type.ItemId = ExistingItem.Id;
                }
            }

            public void SetupExpectedItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingItem);

                _expectedAvailability = new ItemTypeAvailableAtEntityBuilder().Create();

                ExpectedItem = ItemEntityMother
                    .InitialWithTypes()
                    .WithId(ExistingItem.Id)
                    .WithCreatedAt(ExistingItem.CreatedAt)
                    .WithItemTypes(new List<ItemType>
                    {
                        ItemTypeEntityMother.Initial()
                            .WithId(ExistingItem.ItemTypes.First().Id)
                            .WithItemId(ExistingItem.Id)
                            .WithAvailableAt(_expectedAvailability)
                            .WithCreatedAt(ExistingItem.ItemTypes.First().CreatedAt)
                            .Create(),
                        ExistingItem.ItemTypes.Last().DeepClone(),
                    })
                    .Create();

                foreach (var type in ExpectedItem.ItemTypes.Skip(1))
                {
                    type.IsDeleted = true;
                }
            }

            public void SetupExpectedRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedAvailability);
                TestPropertyNotSetException.ThrowIfNull(ExpectedItem);

                var ingredient = new IngredientEntityBuilder()
                    .WithDefaultItemId(ExpectedItem.Id)
                    .WithDefaultItemTypeId(ExpectedItem.ItemTypes.First().Id)
                    .WithDefaultStoreId(_expectedAvailability.StoreId)
                    .Create();
                ExpectedRecipe = new RecipeEntityBuilder()
                    .WithoutSideDishId()
                    .WithIngredient(ingredient)
                    .Create();
            }

            public void SetupExistingRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedRecipe);
                TestPropertyNotSetException.ThrowIfNull(ExistingItem);
                TestPropertyNotSetException.ThrowIfNull(_existingAvailability);

                _existingRecipe = ExpectedRecipe.DeepClone();
                _existingRecipe.Ingredients.First().DefaultStoreId = _existingAvailability.StoreId;
            }

            public void SetupContractWithLessItemTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingItem);
                TestPropertyNotSetException.ThrowIfNull(ExpectedItem);
                TestPropertyNotSetException.ThrowIfNull(_expectedAvailability);

                Contract = new ModifyItemWithTypesContract(
                    ExpectedItem.Name,
                    ExpectedItem.Comment,
                    ExpectedItem.QuantityType,
                    ExpectedItem.QuantityInPacket,
                    ExpectedItem.QuantityTypeInPacket,
                    ExpectedItem.ItemCategoryId!.Value,
                    ExpectedItem.ManufacturerId,
                    new List<ModifyItemTypeContract>
                    {
                        new()
                        {
                            Id = ExistingItem.ItemTypes.First().Id,
                            Name = ExpectedItem.ItemTypes.First().Name,
                            Availabilities = [new ItemAvailabilityContract
                            {
                                StoreId = _expectedAvailability.StoreId,
                                DefaultSectionId = _expectedAvailability.DefaultSectionId,
                                Price = _expectedAvailability.Price
                            }]
                        }
        });
            }

            public async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(ExistingItem);
                TestPropertyNotSetException.ThrowIfNull(ExpectedItem);
                TestPropertyNotSetException.ThrowIfNull(_existingRecipe);

                await ApplyMigrationsAsync(ArrangeScope);

                var itemDatabaseService = new ItemEntityDatabaseService(this);
                await itemDatabaseService.CreateReferencesAsync(ExistingItem, ExpectedItem);
                await itemDatabaseService.SaveAsync(ExistingItem);

                await using var recipeDbContext = GetContextInstance<RecipeContext>(ArrangeScope);
                await recipeDbContext.AddAsync(_existingRecipe);
                await recipeDbContext.SaveChangesAsync();
            }
        }
    }

    public sealed class UpdateItemWithTypesAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly UpdateItemWithTypesAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task UpdateItemWithTypesAsync_WithReferencedOnRecipe_ShouldUpdateRecipe()
        {
            // Arrange
            _fixture.SetupCurrentItemWithoutPredecessor();
            _fixture.SetupExpectedNewItemForItemWithoutPredecessor();
            _fixture.SetupExpectedRecipe();
            _fixture.SetupExistingRecipe();
            _fixture.SetupContract();
            await _fixture.ApplyMigrationsAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.CurrentItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedNewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedRecipe);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().BeOfType<NoContent>();

            using var assertionScope = _fixture.CreateServiceScope();
            var items = (await _fixture.LoadAllItemsAsync(assertionScope)).ToList();
            items.Should().HaveCount(2);

            var oldItem = items.FirstOrDefault(i => i.Id == _fixture.CurrentItem.Id);
            var newItem = items.FirstOrDefault(i => i.Id != _fixture.CurrentItem.Id);
            oldItem.Should().NotBeNull();
            newItem.Should().NotBeNull();

            oldItem.Should().BeEquivalentTo(_fixture.CurrentItem,
                opt => opt.ExcludeItemCycleRef()
                    .Excluding(info => info.Path == "UpdatedOn" || info.Path == "Deleted")
                    .ExcludeRowVersion()
                    .WithCreatedAtPrecision());
            oldItem!.Deleted.Should().BeTrue();
            oldItem.UpdatedOn.Should().NotBeNull();
            oldItem.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(30));

            newItem.Should().BeEquivalentTo(_fixture.ExpectedNewItem,
                opt => opt.ExcludeItemCycleRef()
                    .Excluding(info => info.Path == "Id")
                    .ExcludeItemTypeId()
                    .ExcludeRowVersion()
                    .WithCreatedAtPrecision());

            var recipes = (await _fixture.LoadAllRecipesAsync(assertionScope)).ToList();
            recipes.Should().HaveCount(1);
            recipes[0].Should().BeEquivalentTo(_fixture.ExpectedRecipe,
                opt => opt.ExcludeRowVersion()
                    .ExcludeRecipeCycleRef()
                    .WithCreatedAtPrecision()
                    .Excluding(info => info.Path == "Ingredients[0].DefaultItemId"
                                       || info.Path == "Ingredients[0].DefaultItemTypeId"));
            recipes[0].Ingredients.First().DefaultItemId.Should().Be(newItem!.Id);
            recipes[0].Ingredients.First().DefaultItemTypeId.Should().Be(newItem.ItemTypes.First().Id);
        }

        [Fact]
        public async Task UpdateItemWithTypesAsync_WithItemUpdatedTwiceAlready_ShouldReturnOk()
        {
            // Arrange
            _fixture.SetupSecondLevelPredecessor();
            _fixture.SetupFirstLevelPredecessor();
            _fixture.SetupCurrentItem();
            _fixture.SetupExpectedNewItem();
            _fixture.SetupContract();
            await _fixture.ApplyMigrationsAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.FirstLevelPredecessor);
            TestPropertyNotSetException.ThrowIfNull(_fixture.SecondLevelPredecessor);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedNewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.CurrentItem);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().BeOfType<NoContent>();

            using var assertScope = _fixture.CreateServiceScope();
            var allEntities = (await _fixture.LoadAllItemsAsync(assertScope)).ToList();

            allEntities.Should().Contain(e => e.Id == _fixture.SecondLevelPredecessor.Id);
            allEntities.Should().Contain(e => e.Id == _fixture.FirstLevelPredecessor.Id);
            allEntities.Should().Contain(e => e.Id == _fixture.CurrentItem.Id);
            allEntities.Should().Contain(e =>
                e.Id != _fixture.SecondLevelPredecessor.Id
                && e.Id != _fixture.FirstLevelPredecessor.Id
                && e.Id != _fixture.CurrentItem.Id);

            var secondLevelEntity = allEntities.Single(e => e.Id == _fixture.SecondLevelPredecessor.Id);
            var firstLevelEntity = allEntities.Single(e => e.Id == _fixture.FirstLevelPredecessor.Id);
            var currentEntity = allEntities.Single(e => e.Id == _fixture.CurrentItem.Id);
            var newEntity = allEntities.Single(e =>
                e.Id != _fixture.SecondLevelPredecessor.Id
                && e.Id != _fixture.FirstLevelPredecessor.Id
                && e.Id != _fixture.CurrentItem.Id);

            // second level item should not be altered
            secondLevelEntity.Should().BeEquivalentTo(_fixture.SecondLevelPredecessor,
                opt => opt.UsingDateTimeOffsetWithPrecision().ExcludeItemCycleRef());

            // first level item should not be altered
            firstLevelEntity.Should().BeEquivalentTo(_fixture.FirstLevelPredecessor,
                opt => opt.UsingDateTimeOffsetWithPrecision().ExcludeItemCycleRef());

            // current item should be deleted but not altered otherwise
            currentEntity.Should().BeEquivalentTo(_fixture.CurrentItem,
                opt => opt
                    .ExcludeItemCycleRef()
                    .ExcludeRowVersion()
                    .Excluding(info => info.Path == "Deleted" || info.Path == "UpdatedOn")
                    .WithCreatedAtPrecision());
            currentEntity.Deleted.Should().BeTrue();
            currentEntity.UpdatedOn.Should().NotBeNull();
            currentEntity.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(30));

            // there should be a new entity with the CurrentItem as predecessor
            newEntity.Should().BeEquivalentTo(_fixture.ExpectedNewItem,
                opt => opt.ExcludeItemCycleRef()
                    .ExcludeRowVersion()
                    .ExcludeItemTypeId()
                    .Excluding(info => info.Path == "Id")
                    .WithCreatedAtPrecision());
        }

        private sealed class UpdateItemWithTypesAsyncFixture : ItemControllerFixture
        {
            private readonly ItemEntityDatabaseService _itemDatabaseService;
            private ItemTypeAvailableAt? _existingAvailability;
            private ItemTypeAvailableAt? _expectedAvailability;
            private Recipe? _existingRecipe;
            private readonly List<Item> _itemsToSave = new();

            public UpdateItemWithTypesAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
                _itemDatabaseService = new ItemEntityDatabaseService(this);
            }

            public Item? CurrentItem { get; private set; }
            public Item? SecondLevelPredecessor { get; private set; }
            public Item? FirstLevelPredecessor { get; private set; }
            public UpdateItemWithTypesContract? Contract { get; private set; }
            public Item? ExpectedNewItem { get; private set; }
            public Recipe? ExpectedRecipe { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(CurrentItem);
                TestPropertyNotSetException.ThrowIfNull(Contract);

                var scope = CreateServiceScope();

                return await ItemEndpoints.UpdateItemWithTypes(
                    CurrentItem.Id,
                    Contract,
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    scope.ServiceProvider.GetRequiredService<
                        IToDomainConverter<(Guid, UpdateItemWithTypesContract), UpdateItemWithTypesCommand>>(),
                    default);
            }

            public void SetupCurrentItem()
            {
                TestPropertyNotSetException.ThrowIfNull(FirstLevelPredecessor);

                var builder = ItemEntityMother.InitialWithTypes();

                var types = FirstLevelPredecessor.ItemTypes
                    .Select(t =>
                        ItemTypeEntityMother.Initial().WithPredecessorId(t.Id).WithCreatedAt(t.CreatedAt).Create())
                    .ToList();
                builder.WithItemTypes(types);

                CurrentItem = builder
                    .WithPredecessorId(FirstLevelPredecessor.Id)
                    .WithCreatedAt(FirstLevelPredecessor.CreatedAt)
                    .Create();

                _itemsToSave.Add(CurrentItem);
            }

            public void SetupFirstLevelPredecessor()
            {
                TestPropertyNotSetException.ThrowIfNull(SecondLevelPredecessor);

                var types = SecondLevelPredecessor.ItemTypes
                    .Select(t => ItemTypeEntityMother.Initial().WithPredecessorId(t.Id).Create())
                    .ToList();

                FirstLevelPredecessor = ItemEntityMother.InitialWithTypes()
                    .WithDeleted(true)
                    .WithItemTypes(types)
                    .WithPredecessorId(SecondLevelPredecessor.Id)
                    .WithCreatedAt(SecondLevelPredecessor.CreatedAt)
                    .Create();

                _itemsToSave.Add(FirstLevelPredecessor);
            }

            public void SetupSecondLevelPredecessor()
            {
                SecondLevelPredecessor = ItemEntityMother.InitialWithTypes().WithDeleted(true).Create();

                _itemsToSave.Add(SecondLevelPredecessor);
            }

            public void SetupExpectedNewItem()
            {
                TestPropertyNotSetException.ThrowIfNull(CurrentItem);

                var itemTypes = CurrentItem.ItemTypes
                    .Select(t => ItemTypeEntityMother.Initial().WithPredecessorId(t.Id).WithCreatedAt(t.CreatedAt).Create())
                    .ToList();

                ExpectedNewItem = ItemEntityMother.InitialWithTypes()
                    .WithPredecessorId(CurrentItem.Id)
                    .WithItemTypes(itemTypes)
                    .WithCreatedAt(CurrentItem.CreatedAt)
                    .Create();
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedNewItem);

                Contract = new UpdateItemWithTypesContract(
                    ExpectedNewItem.Name,
                    ExpectedNewItem.Comment,
                    ExpectedNewItem.QuantityType,
                    ExpectedNewItem.QuantityInPacket,
                    ExpectedNewItem.QuantityTypeInPacket,
                    ExpectedNewItem.ItemCategoryId!.Value,
                    ExpectedNewItem.ManufacturerId,
                    ExpectedNewItem.ItemTypes.Select(t => new UpdateItemTypeContract(
                        t.PredecessorId!.Value,
                        t.Name,
                        t.AvailableAt.Select(av => new ItemAvailabilityContract()
                        {
                            DefaultSectionId = av.DefaultSectionId,
                            Price = av.Price,
                            StoreId = av.StoreId
                        }))));
            }

            public void SetupExpectedNewItemForItemWithoutPredecessor()
            {
                TestPropertyNotSetException.ThrowIfNull(CurrentItem);

                var itemTypes = CurrentItem.ItemTypes
                    .Select(t => ItemTypeEntityMother.Initial().WithPredecessorId(t.Id).WithCreatedAt(t.CreatedAt).Create())
                    .ToList();

                _expectedAvailability = itemTypes.First().AvailableAt.First();

                ExpectedNewItem = ItemEntityMother.InitialWithTypes()
                    .WithPredecessorId(CurrentItem.Id)
                    .WithItemTypes(itemTypes)
                    .WithCreatedAt(CurrentItem.CreatedAt)
                    .Create();
            }

            public void SetupCurrentItemWithoutPredecessor()
            {
                var itemType = ItemTypeEntityMother.Initial().Create();
                _existingAvailability = itemType.AvailableAt.First();
                CurrentItem = ItemEntityMother.InitialWithTypes().WithItemType(itemType).Create();

                _itemsToSave.Add(CurrentItem);
            }

            public void SetupExpectedRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedAvailability);
                TestPropertyNotSetException.ThrowIfNull(ExpectedNewItem);

                var ingredient = new IngredientEntityBuilder()
                    .WithDefaultStoreId(_expectedAvailability.StoreId)
                    .WithDefaultItemId(ExpectedNewItem.Id)
                    .WithDefaultItemTypeId(ExpectedNewItem.ItemTypes.First().Id)
                    .Create();
                ExpectedRecipe = new RecipeEntityBuilder()
                    .WithoutSideDishId()
                    .WithIngredient(ingredient)
                    .Create();
            }

            public void SetupExistingRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedRecipe);
                TestPropertyNotSetException.ThrowIfNull(CurrentItem);
                TestPropertyNotSetException.ThrowIfNull(_existingAvailability);

                _existingRecipe = ExpectedRecipe.DeepClone();
                _existingRecipe.Ingredients.First().DefaultItemId = CurrentItem.Id;
                _existingRecipe.Ingredients.First().DefaultItemTypeId = CurrentItem.ItemTypes.First().Id;
                _existingRecipe.Ingredients.First().DefaultStoreId = _existingAvailability.StoreId;
            }

            public async Task ApplyMigrationsAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedNewItem);

                await ApplyMigrationsAsync(ArrangeScope);

                await _itemDatabaseService.CreateReferencesAsync(_itemsToSave.Append(ExpectedNewItem).ToArray());
                await _itemDatabaseService.SaveAsync(_itemsToSave.ToArray());

                if (_existingRecipe is not null)
                {
                    await using var recipeContext = GetContextInstance<RecipeContext>(ArrangeScope);
                    await recipeContext.AddAsync(_existingRecipe);
                    await recipeContext.SaveChangesAsync();
                }
            }
        }
    }

    public sealed class UpdateItemAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly UpdateItemAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task UpdateItemAsync_WithExistingItem_WithAvailableStoreChanged_ShouldUpdateItemAndRecipe()
        {
            // Arrange
            _fixture.SetupExpectedItems();
            _fixture.SetupExistingItem();
            _fixture.SetupContract();
            _fixture.SetupExpectedRecipe();
            _fixture.SetupExistingRecipe();
            await _fixture.SetupDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedOldItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedNewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedRecipe);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().BeOfType<NoContent>();

            using var assertionScope = _fixture.CreateServiceScope();

            var items = (await _fixture.LoadAllItemsAsync(assertionScope)).ToList();
            items.Should().HaveCount(2);

            var oldItem = items.FirstOrDefault(i => i.Id == _fixture.ItemId);
            var newItem = items.FirstOrDefault(i => i.Id != _fixture.ItemId);
            oldItem.Should().NotBeNull();
            newItem.Should().NotBeNull();
            oldItem.Should().BeEquivalentTo(_fixture.ExpectedOldItem,
                opt => opt.ExcludeItemCycleRef().Excluding(info => info.Path == "UpdatedOn").ExcludeRowVersion().WithCreatedAtPrecision());
            oldItem!.UpdatedOn.Should().NotBeNull();
            oldItem.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(30));
            newItem.Should().BeEquivalentTo(_fixture.ExpectedNewItem,
                opt => opt.ExcludeItemCycleRef().Excluding(info => info.Path == "Id").ExcludeRowVersion().WithCreatedAtPrecision());

            var recipes = (await _fixture.LoadAllRecipesAsync(assertionScope)).ToList();
            recipes.Should().HaveCount(1);
            recipes[0].Should().BeEquivalentTo(_fixture.ExpectedRecipe,
                opt => opt.ExcludeRowVersion().ExcludeRecipeCycleRef()
                    .Excluding(info => info.Path == "Ingredients[0].DefaultItemId")
                    .WithCreatedAtPrecision());
            recipes[0].Ingredients.First().DefaultItemId.Should().NotBe(_fixture.ExpectedOldItem.Id);
        }

        private sealed class UpdateItemAsyncFixture(DockerFixture dockerFixture) : ItemControllerFixture(dockerFixture)
        {
            private AvailableAt? _expectedAvailability;
            private AvailableAt? _existingAvailability;
            private Item? _existingItem;
            private Recipe? _existingRecipe;

            public Guid ItemId { get; } = Guid.NewGuid();
            public Item? ExpectedOldItem { get; private set; }
            public Item? ExpectedNewItem { get; private set; }
            public Recipe? ExpectedRecipe { get; private set; }
            public UpdateItemContract? Contract { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);

                var scope = CreateServiceScope();
                return await ItemEndpoints.UpdateItem(
                    ItemId,
                    Contract,
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    scope.ServiceProvider
                        .GetRequiredService<IToDomainConverter<(Guid, UpdateItemContract), UpdateItemCommand>>(),
                    default);
            }

            public void SetupExpectedItems()
            {
                _existingAvailability = new AvailableAtEntityBuilder().Create();
                ExpectedOldItem = ItemEntityMother.Initial()
                    .WithId(ItemId)
                    .WithAvailableAt(_existingAvailability)
                    .WithUpdatedOn(DateTimeOffset.UtcNow)
                    .WithDeleted(true)
                    .Create();

                _expectedAvailability = new AvailableAtEntityBuilder().Create();
                ExpectedNewItem = ItemEntityMother.Initial()
                    .WithAvailableAt(_expectedAvailability)
                    .WithPredecessorId(ExpectedOldItem.Id)
                    .WithCreatedAt(ExpectedOldItem.CreatedAt)
                    .Create();
            }

            public void SetupExistingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedNewItem);

                _existingItem = ExpectedOldItem.DeepClone()!;
                _existingItem.Deleted = false;
                _existingItem!.UpdatedOn = null;
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedNewItem);

                Contract = new UpdateItemContract(
                    ExpectedNewItem.Name,
                    ExpectedNewItem.Comment,
                    ExpectedNewItem.QuantityType,
                    ExpectedNewItem.QuantityInPacket,
                    ExpectedNewItem.QuantityTypeInPacket,
                    ExpectedNewItem.ItemCategoryId!.Value,
                    ExpectedNewItem.ManufacturerId,
                    ExpectedNewItem.AvailableAt.Select(av => new ItemAvailabilityContract
                    {
                        StoreId = av.StoreId,
                        DefaultSectionId = av.DefaultSectionId,
                        Price = av.Price
                    }));
            }

            public void SetupExpectedRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedAvailability);
                TestPropertyNotSetException.ThrowIfNull(ExpectedNewItem);

                var ingredient = new IngredientEntityBuilder()
                    .WithoutDefaultItemTypeId()
                    .WithDefaultStoreId(_expectedAvailability.StoreId)
                    .Create();
                ExpectedRecipe = new RecipeEntityBuilder()
                    .WithoutSideDishId()
                    .WithIngredient(ingredient)
                    .Create();
            }

            public void SetupExistingRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedRecipe);
                TestPropertyNotSetException.ThrowIfNull(_existingItem);
                TestPropertyNotSetException.ThrowIfNull(_existingAvailability);

                _existingRecipe = ExpectedRecipe.DeepClone();
                _existingRecipe.Ingredients.First().DefaultItemId = _existingItem.Id;
                _existingRecipe.Ingredients.First().DefaultStoreId = _existingAvailability.StoreId;
            }

            public async Task SetupDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingItem);
                TestPropertyNotSetException.ThrowIfNull(ExpectedNewItem);
                TestPropertyNotSetException.ThrowIfNull(ExpectedOldItem);
                TestPropertyNotSetException.ThrowIfNull(_existingRecipe);

                await ApplyMigrationsAsync(ArrangeScope);

                var itemDatabaseService = new ItemEntityDatabaseService(this);
                await itemDatabaseService.CreateReferencesAsync(ExpectedNewItem, ExpectedOldItem, _existingItem);
                await itemDatabaseService.SaveAsync(_existingItem);

                await using var recipeContext = GetContextInstance<RecipeContext>(ArrangeScope);

                await recipeContext.AddAsync(_existingRecipe);
                await recipeContext.SaveChangesAsync();
            }
        }
    }

    public sealed class UpdateItemPriceAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly UpdateItemPriceAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task UpdateItemPriceAsync_WithoutTypes_ShouldUpdateItem()
        {
            // Arrange
            _fixture.SetupItemId();
            _fixture.SetupContractWithoutItemType();
            _fixture.SetupExistingItemWithoutTypes();
            _fixture.SetupExpectedResultWithoutTypes();
            await _fixture.PrepareDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedOldItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedNewItem);

            // Act
            await _fixture.ActAsync();

            // Assert
            using var assertionServiceScope = _fixture.CreateServiceScope();

            var allStoredItems = (await _fixture.LoadAllItemsAsync(assertionServiceScope)).ToList();
            allStoredItems.Should().HaveCount(2);
            allStoredItems.Should().Contain(i => i.Id == _fixture.ExpectedOldItem.Id);
            allStoredItems.Should().Contain(i => i.Id != _fixture.ExpectedOldItem.Id);

            var oldItem = allStoredItems.First(i => i.Id == _fixture.ExpectedOldItem.Id);
            oldItem.Should().BeEquivalentTo(_fixture.ExpectedOldItem,
                opt => opt
                    .ExcludeItemCycleRef()
                    .ExcludeRowVersion()
                    .Excluding(info => info.Path == "UpdatedOn")
                    .WithCreatedAtPrecision());
            oldItem.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(30));

            var newItem = allStoredItems.First(i => i.Id != _fixture.ExpectedOldItem.Id);
            newItem.Should().BeEquivalentTo(_fixture.ExpectedNewItem,
                opt => opt
                    .ExcludeItemCycleRef()
                    .ExcludeRowVersion()
                    .Excluding(info => info.Path == "Id")
                    .WithCreatedAtPrecision());
        }

        [Fact]
        public async Task UpdateItemPriceAsync_WithItemTypes_ShouldUpdateItem()
        {
            // Arrange
            _fixture.SetupItemId();
            _fixture.SetupContractWithoutItemType();
            _fixture.SetupExistingItemWithTypes();
            _fixture.SetupExpectedResultWithTypes();
            await _fixture.PrepareDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedOldItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedNewItem);

            // Act
            await _fixture.ActAsync();

            // Assert
            using var assertionServiceScope = _fixture.CreateServiceScope();

            var allStoredItems = (await _fixture.LoadAllItemsAsync(assertionServiceScope)).ToList();
            allStoredItems.Should().HaveCount(2);
            allStoredItems.Should().Contain(i => i.Id == _fixture.ExpectedOldItem.Id);
            allStoredItems.Should().Contain(i => i.Id != _fixture.ExpectedOldItem.Id);

            var oldItem = allStoredItems.First(i => i.Id == _fixture.ExpectedOldItem.Id);
            oldItem.Should().BeEquivalentTo(_fixture.ExpectedOldItem,
                opt => opt
                    .ExcludeItemCycleRef()
                    .ExcludeRowVersion()
                    .Excluding(info => info.Path == "UpdatedOn")
                    .WithCreatedAtPrecision());
            oldItem.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(30));

            var newItem = allStoredItems.First(i => i.Id != _fixture.ExpectedOldItem.Id);
            newItem.Should().BeEquivalentTo(_fixture.ExpectedNewItem,
                opt => opt
                    .ExcludeItemCycleRef()
                    .ExcludeRowVersion()
                    .WithCreatedAtPrecision()
                    .Excluding(info => info.Path == "Id"
                                       || Regex.IsMatch(info.Path, @"ItemTypes\[\d+\].Id")));
        }

        private sealed class UpdateItemPriceAsyncFixture(DockerFixture dockerFixture)
            : ItemControllerFixture(dockerFixture)
        {
            private Item? _existingItem;

            public ItemId? ItemId { get; private set; }
            public UpdateItemPriceContract? Contract { get; private set; }
            public Item? ExpectedNewItem { get; private set; }
            public Item? ExpectedOldItem { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemId);
                TestPropertyNotSetException.ThrowIfNull(Contract);

                var scope = CreateServiceScope();

                return await ItemEndpoints.UpdateItemPrice(
                    ItemId.Value,
                    Contract,
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    scope.ServiceProvider
                        .GetRequiredService<IToDomainConverter<(Guid, UpdateItemPriceContract), UpdateItemPriceCommand>>(),
                    default);
            }

            public void SetupItemId()
            {
                ItemId = Domain.Items.Models.ItemId.New;
            }

            public void SetupContractWithoutItemType()
            {
                var price = new DomainTestBuilder<Price>().Create();
                Contract = new TestBuilder<UpdateItemPriceContract>()
                    .FillPropertyWith(c => c.ItemTypeId, null)
                    .FillPropertyWith(c => c.Price, price)
                    .Create();
            }

            public void SetupExistingItemWithoutTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                _existingItem = ItemEntityMother.InitialForStore(Contract.StoreId)
                    .WithId(ItemId.Value)
                    .Create();
            }

            public void SetupExistingItemWithTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                var itemTypes = ItemTypeEntityMother.InitialForStore(Contract.StoreId)
                    .CreateMany(1)
                    .ToList();
                _existingItem = ItemEntityMother.InitialWithTypes()
                    .WithId(ItemId.Value)
                    .WithItemTypes(itemTypes)
                    .Create();
            }

            public async Task PrepareDatabaseAsync()
            {
                await ApplyMigrationsAsync(ArrangeScope);

                TestPropertyNotSetException.ThrowIfNull(_existingItem);

                using var scope = CreateServiceScope();
                await using var context = GetContextInstance<ItemContext>(ArrangeScope);

                context.Add(_existingItem);

                await context.SaveChangesAsync();
            }

            public void SetupExpectedResultWithoutTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingItem);
                TestPropertyNotSetException.ThrowIfNull(Contract);

                ExpectedOldItem = new Item
                {
                    Id = _existingItem.Id,
                    AvailableAt = _existingItem.AvailableAt,
                    Name = _existingItem.Name,
                    Comment = _existingItem.Comment,
                    CreatedFrom = _existingItem.CreatedFrom,
                    Deleted = true,
                    IsTemporary = _existingItem.IsTemporary,
                    ItemCategoryId = _existingItem.ItemCategoryId,
                    ManufacturerId = _existingItem.ManufacturerId,
                    QuantityInPacket = _existingItem.QuantityInPacket,
                    QuantityType = _existingItem.QuantityType,
                    QuantityTypeInPacket = _existingItem.QuantityTypeInPacket,
                    PredecessorId = null,
                    ItemTypes = new List<ItemType>(),
                    UpdatedOn = DateTimeOffset.Now,
                    CreatedAt = _existingItem.CreatedAt
                };

                ExpectedNewItem = new Item
                {
                    AvailableAt = _existingItem.AvailableAt.Select(av => new AvailableAt
                    {
                        StoreId = av.StoreId,
                        Price = Contract.Price,
                        DefaultSectionId = av.DefaultSectionId
                    }).ToList(),
                    Name = _existingItem.Name,
                    Comment = _existingItem.Comment,
                    CreatedFrom = _existingItem.CreatedFrom,
                    Deleted = false,
                    IsTemporary = false,
                    ItemCategoryId = _existingItem.ItemCategoryId,
                    ManufacturerId = _existingItem.ManufacturerId,
                    QuantityInPacket = _existingItem.QuantityInPacket,
                    QuantityType = _existingItem.QuantityType,
                    QuantityTypeInPacket = _existingItem.QuantityTypeInPacket,
                    PredecessorId = _existingItem.Id,
                    ItemTypes = new List<ItemType>(),
                    CreatedAt = _existingItem.CreatedAt
                };
            }

            public void SetupExpectedResultWithTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingItem);
                TestPropertyNotSetException.ThrowIfNull(Contract);

                ExpectedOldItem = new Item
                {
                    Id = _existingItem.Id,
                    AvailableAt = new List<AvailableAt>(),
                    Name = _existingItem.Name,
                    Comment = _existingItem.Comment,
                    CreatedFrom = null,
                    Deleted = true,
                    IsTemporary = _existingItem.IsTemporary,
                    ItemCategoryId = _existingItem.ItemCategoryId,
                    ManufacturerId = _existingItem.ManufacturerId,
                    QuantityInPacket = _existingItem.QuantityInPacket,
                    QuantityType = _existingItem.QuantityType,
                    QuantityTypeInPacket = _existingItem.QuantityTypeInPacket,
                    PredecessorId = null,
                    ItemTypes = _existingItem.ItemTypes,
                    UpdatedOn = DateTimeOffset.Now,
                    CreatedAt = _existingItem.CreatedAt
                };

                ExpectedNewItem = new Item
                {
                    AvailableAt = new List<AvailableAt>(),
                    Name = _existingItem.Name,
                    Comment = _existingItem.Comment,
                    CreatedFrom = null,
                    Deleted = false,
                    IsTemporary = false,
                    ItemCategoryId = _existingItem.ItemCategoryId,
                    ManufacturerId = _existingItem.ManufacturerId,
                    QuantityInPacket = _existingItem.QuantityInPacket,
                    QuantityType = _existingItem.QuantityType,
                    QuantityTypeInPacket = _existingItem.QuantityTypeInPacket,
                    PredecessorId = _existingItem.Id,
                    CreatedAt = _existingItem.CreatedAt,
                    ItemTypes = _existingItem.ItemTypes.Select(t => new ItemType
                    {
                        Name = t.Name,
                        PredecessorId = t.Id,
                        CreatedAt = t.CreatedAt,
                        AvailableAt = t.AvailableAt.Select(av =>
                            new ItemTypeAvailableAtEntityBuilder(av)
                                .WithPrice(Contract.Price)
                                .Create())
                            .ToList()
                    }).ToList()
                };
            }
        }
    }

    public sealed class DeleteItemAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly DeleteItemAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task DeleteItemAsync_WithValidData_ShouldDeleteItemAndRemoveItFromListAndRecipe()
        {
            // Arrange
            _fixture.SetupItem();
            _fixture.SetupShoppingListWithItem();
            _fixture.SetupStoreForShoppingList();
            _fixture.SetupRecipeWithItem();
            await _fixture.PrepareDatabaseAsync();
            _fixture.SetupExpectedItem();
            _fixture.SetupExpectedRecipe();
            _fixture.SetupExpectedShoppingList();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedRecipe);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedShoppingList);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContent>();

            using var assertionServiceScope = _fixture.CreateServiceScope();
            using var assertionScope = new AssertionScope();

            var items = (await _fixture.LoadAllItemsAsync(assertionServiceScope)).ToArray();
            items.Should().HaveCount(1);
            items.First().Should().BeEquivalentTo(_fixture.ExpectedItem,
                opt => opt
                    .ExcludeItemCycleRef()
                    .ExcludeRowVersion()
                    .UsingDateTimeOffsetWithPrecision());

            var recipes = (await _fixture.LoadAllRecipesAsync(assertionServiceScope)).ToArray();
            recipes.Should().HaveCount(1);
            recipes.First().Should().BeEquivalentTo(_fixture.ExpectedRecipe,
                opt => opt
                .ExcludeRecipeCycleRef()
                .Excluding(info => Regex.IsMatch(info.Path, @"Ingredients\[\d+\].Id"))
                .WithCreatedAtPrecision());

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertionServiceScope)).ToArray();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.First().Should().BeEquivalentTo(_fixture.ExpectedShoppingList,
                opt => opt.ExcludeShoppingListCycleRef().WithCreatedAtPrecision());
        }

        private sealed class DeleteItemAsyncFixture(DockerFixture dockerFixture) : ItemControllerFixture(dockerFixture)
        {
            private Repositories.ShoppingLists.Entities.ShoppingList? _shoppingList;
            private Store? _store;
            private Recipe? _recipe;

            public Item? Item { get; private set; }
            public Item? ExpectedItem { get; private set; }
            public Repositories.ShoppingLists.Entities.ShoppingList? ExpectedShoppingList { get; private set; }
            public Recipe? ExpectedRecipe { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);

                var scope = CreateServiceScope();
                return await ItemEndpoints.DeleteItem(Item.Id,
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    default);
            }

            public void SetupItem()
            {
                Item = ItemEntityMother.Initial().Create();
            }

            public void SetupExpectedItem()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);

                ExpectedItem = Item.DeepClone();
                ExpectedItem.Deleted = true;
            }

            public void SetupShoppingListWithItem()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);

                _shoppingList = ShoppingListEntityMother.InitialWithTwoItems(Item.Id, null, Guid.NewGuid()).Create();
            }

            public void SetupExpectedShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                TestPropertyNotSetException.ThrowIfNull(Item);

                ExpectedShoppingList = _shoppingList.DeepClone();
                ExpectedShoppingList.ItemsOnList =
                    ExpectedShoppingList.ItemsOnList.Where(map => map.ItemId != Item.Id).ToArray();
            }

            public void SetupStoreForShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);

                var sectionIds = _shoppingList.ItemsOnList.Select(x => x.SectionId).ToArray();

                _store = StoreEntityMother
                    .ValidSections(sectionIds)
                    .WithId(_shoppingList.StoreId)
                    .Create();
            }

            public void SetupRecipeWithItem()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);

                var ingredients = new IngredientEntityBuilder()
                    .WithDefaultItemId(Item.Id)
                    .WithoutDefaultItemTypeId()
                    .CreateMany(1)
                    .ToArray();

                _recipe = new RecipeEntityBuilder()
                    .WithoutSideDishId()
                    .WithIngredients(ingredients)
                    .Create();
            }

            public void SetupExpectedRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);

                ExpectedRecipe = _recipe.DeepClone();
                ExpectedRecipe.Ingredients.First().DefaultItemId = null;
                ExpectedRecipe.Ingredients.First().DefaultStoreId = null;
                ExpectedRecipe.Ingredients.First().AddToShoppingListByDefault = null;
            }

            public async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);
                TestPropertyNotSetException.ThrowIfNull(_store);
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                TestPropertyNotSetException.ThrowIfNull(_recipe);

                await ApplyMigrationsAsync(ArrangeScope);

                await using var itemContext = GetContextInstance<ItemContext>(ArrangeScope);
                await using var storeContext = GetContextInstance<StoreContext>(ArrangeScope);
                await using var shoppingListContext = GetContextInstance<ShoppingListContext>(ArrangeScope);
                await using var recipeContext = GetContextInstance<RecipeContext>(ArrangeScope);

                itemContext.Add(Item);
                storeContext.Add(_store);
                shoppingListContext.Add(_shoppingList);
                recipeContext.Add(_recipe);

                await itemContext.SaveChangesAsync();
                await storeContext.SaveChangesAsync();
                await shoppingListContext.SaveChangesAsync();
                await recipeContext.SaveChangesAsync();
            }
        }
    }

    private class ItemControllerFixture : DatabaseFixture
    {
        protected ItemControllerFixture(DockerFixture dockerFixture) : base(dockerFixture)
        {
            ArrangeScope = CreateServiceScope();
        }

        protected readonly IServiceScope ArrangeScope;

        public override IEnumerable<DbContext> GetDbContexts(IServiceScope scope)
        {
            yield return scope.ServiceProvider.GetRequiredService<ItemContext>();
            yield return scope.ServiceProvider.GetRequiredService<ShoppingListContext>();
            yield return scope.ServiceProvider.GetRequiredService<ItemCategoryContext>();
            yield return scope.ServiceProvider.GetRequiredService<ManufacturerContext>();
            yield return scope.ServiceProvider.GetRequiredService<StoreContext>();
            yield return scope.ServiceProvider.GetRequiredService<RecipeContext>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ArrangeScope.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}