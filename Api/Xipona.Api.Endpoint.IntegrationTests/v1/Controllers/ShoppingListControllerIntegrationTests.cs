using FluentAssertions;
using Force.DeepCloner;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Shared;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using ProjectHermes.Xipona.Api.Repositories.Items.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Contexts;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Contexts;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;
using ProjectHermes.Xipona.Api.Repositories.Stores.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Stores.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Items.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Stores.Entities;
using ProjectHermes.Xipona.Api.TestTools.AutoFixture;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System;
using Xunit;
using Item = ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item;

namespace ProjectHermes.Xipona.Api.Endpoint.IntegrationTests.v1.Controllers;

public class ShoppingListControllerIntegrationTests
{
    public sealed class RemoveItemFromShoppingListAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly RemoveItemFromShoppingListAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task RemoveItemFromShoppingListAsync_WithItemWithoutTypes_ShouldRemoveItemFromShoppingList()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupItemWithoutTypes();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.RemoveItemFromShoppingListAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion());
        }

        [Fact]
        public async Task RemoveItemFromShoppingListAsync_WithItemWithTypes_ShouldRemoveItemFromShoppingList()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupItemWithTypes();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.RemoveItemFromShoppingListAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion());
        }

        [Fact]
        public async Task RemoveItemFromShoppingListAsync_WithItemOfflineId_ShouldRemoveItemFromShoppingList()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupItemWithoutTypesForOfflineUse();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.RemoveItemFromShoppingListAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion());
        }

        [Fact]
        public async Task RemoveItemFromShoppingListAsync_WithTemporaryItem_ShouldRemoveItemFromShoppingListAndDeleteItem()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupTemporaryItem();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItem);

            // Act
            var result = await sut.RemoveItemFromShoppingListAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            shoppingLists.Should().HaveCount(1);
            shoppingLists.Single().Should().BeEquivalentTo(_fixture.ExpectedResult,
                opt => opt.ExcludeShoppingListCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion());

            var items = (await _fixture.LoadAllItemsAsync(assertScope)).ToList();
            items.Should().HaveCount(1);
            items.Single().Should().BeEquivalentTo(_fixture.ExpectedItem,
                opt => opt.ExcludeItemCycleRef().UsingDateTimeOffsetWithPrecision().ExcludeRowVersion());
        }

        private sealed class RemoveItemFromShoppingListAsyncFixture(DockerFixture dockerFixture)
            : ShoppingListControllerFixture(dockerFixture)
        {
            private readonly CommonFixture _commonFixture = new();
            private ShoppingList? _shoppingList;
            private Item? _item;
            private ItemIdContract? _itemId;
            private Guid? _itemTypeId;

            public Guid? ShoppingListId { get; private set; }
            public RemoveItemFromShoppingListContract? Contract { get; private set; }
            public ShoppingList? ExpectedResult { get; private set; }
            public Item? ExpectedItem { get; private set; }

            public void SetupExpectedResult()
            {
                _shoppingList = ShoppingListEntityMother.Active().Create();
                ExpectedResult = _shoppingList.DeepClone();
                ShoppingListId = _shoppingList.Id;
            }

            public void SetupItemWithoutTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                var itemOnList = new ItemsOnListEntityBuilder().WithoutItemTypeId().Create();
                _shoppingList.ItemsOnList.Add(itemOnList);
                _itemId = new(itemOnList.ItemId, null);
                _itemTypeId = null;

                _item = ItemEntityMother.InitialForStore(_shoppingList.StoreId).WithId(itemOnList.ItemId).Create();
            }

            public void SetupItemWithoutTypesForOfflineUse()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);

                var itemOnList = new ItemsOnListEntityBuilder().WithoutItemTypeId().Create();
                _item = ItemEntityMother.InitialForStore(_shoppingList.StoreId)
                    .WithId(itemOnList.ItemId)
                    .WithCreatedFrom(Guid.NewGuid())
                    .Create();

                _shoppingList.ItemsOnList.Add(itemOnList);
                _itemId = new(null, _item.CreatedFrom);
                _itemTypeId = null;
            }

            public void SetupItemWithTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);

                _item = ItemEntityMother.InitialWithTypesForStore(_shoppingList.StoreId).Create();

                var itemOnList = new ItemsOnListEntityBuilder()
                    .WithItemId(_item.Id)
                    .WithItemTypeId(_commonFixture.ChooseRandom(_item.ItemTypes).Id)
                    .Create();
                _itemId = new(itemOnList.ItemId, null);
                _itemTypeId = itemOnList.ItemTypeId;
                _shoppingList.ItemsOnList.Add(itemOnList);
            }

            public void SetupTemporaryItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                var itemOnList = new ItemsOnListEntityBuilder().WithoutItemTypeId().Create();
                _shoppingList.ItemsOnList.Add(itemOnList);
                _itemId = new(itemOnList.ItemId, null);
                _itemTypeId = null;

                _item = ItemEntityMother.Temporary(_shoppingList.StoreId).WithId(itemOnList.ItemId).Create();
                ExpectedItem = _item.DeepClone();
                ExpectedItem.Deleted = true;
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);
                Contract = new RemoveItemFromShoppingListContract(_itemId, _itemTypeId);
            }

            public async Task SetupDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);
                TestPropertyNotSetException.ThrowIfNull(_item);

                await ApplyMigrationsAsync(ArrangeScope);

                var shoppingListContext = ArrangeScope.ServiceProvider.GetRequiredService<ShoppingListContext>();
                var itemContext = ArrangeScope.ServiceProvider.GetRequiredService<ItemContext>();

                await shoppingListContext.AddAsync(_shoppingList);
                await itemContext.AddAsync(_item);

                await shoppingListContext.SaveChangesAsync();
                await itemContext.SaveChangesAsync();
            }
        }
    }

    public sealed class FinishListAsync(DockerFixture dockerFixture) : IAssemblyFixture<DockerFixture>
    {
        private readonly FinishListAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task FinishListAsync_WithUndefinedFinishedAt_ShouldSetCorrectFinishedAtDate()
        {
            // Arrange
            _fixture.SetupShoppingListId();
            _fixture.SetupExistingShoppingList();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);

            // Act
            var result = await sut.FinishListAsync(_fixture.ShoppingListId.Value, null);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            var allShoppingLists = (await _fixture.LoadAllShoppingLists()).ToList();
            allShoppingLists.Should().HaveCount(2);
            allShoppingLists.Should().ContainSingle(sl => sl.Id == _fixture.ShoppingListId.Value);

            var finishedShoppingList = allShoppingLists.Single(sl => sl.Id == _fixture.ShoppingListId.Value);
            finishedShoppingList.CompletionDate.Should().NotBeNull();
            finishedShoppingList.CompletionDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(10));
        }

        [Fact]
        public async Task FinishListAsync_WithDefinedFinishedAt_ShouldSetCorrectFinishedAtDate()
        {
            // Arrange
            _fixture.SetupShoppingListId();
            _fixture.SetupFinishedAt();
            _fixture.SetupExistingShoppingList();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.FinishedAt);

            // Act
            var result = await sut.FinishListAsync(_fixture.ShoppingListId.Value, _fixture.FinishedAt);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            var allShoppingLists = (await _fixture.LoadAllShoppingLists()).ToList();
            allShoppingLists.Should().HaveCount(2);
            allShoppingLists.Should().ContainSingle(sl => sl.Id == _fixture.ShoppingListId.Value);

            var finishedShoppingList = allShoppingLists.Single(sl => sl.Id == _fixture.ShoppingListId.Value);
            finishedShoppingList.CompletionDate.Should().NotBeNull();
            finishedShoppingList.CompletionDate.Should().BeCloseTo(_fixture.FinishedAt.Value, TimeSpan.FromMilliseconds(10));
        }

        private sealed class FinishListAsyncFixture(DockerFixture dockerFixture)
            : ShoppingListControllerFixture(dockerFixture)
        {
            private ShoppingList? _existingShoppingList;

            public Guid? ShoppingListId { get; private set; }
            public DateTimeOffset? FinishedAt { get; private set; }

            public void SetupShoppingListId()
            {
                ShoppingListId = Guid.NewGuid();
            }

            public void SetupFinishedAt()
            {
                FinishedAt = new TestBuilder<DateTimeOffset>().Create();
            }

            public void SetupExistingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ShoppingListId);

                var items = new ItemsOnListEntityBuilder()
                    .WithShoppingList(null!)
                    .WithShoppingListId(ShoppingListId.Value)
                    .CreateMany(3)
                    .ToList();

                _existingShoppingList = new ShoppingListEntityBuilder()
                    .WithoutCompletionDate()
                    .WithId(ShoppingListId.Value)
                    .WithItemsOnList(items)
                    .Create();
            }

            public async Task SetupDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingShoppingList);

                await ApplyMigrationsAsync(ArrangeScope);

                var itemContext = ArrangeScope.ServiceProvider.GetRequiredService<ShoppingListContext>();

                await itemContext.AddAsync(_existingShoppingList);

                await itemContext.SaveChangesAsync();
            }
        }
    }

    public sealed class AddTemporaryItemToShoppingListAsync(DockerFixture dockerFixture)
        : IAssemblyFixture<DockerFixture>
    {
        private readonly AddTemporaryItemToShoppingListAsyncFixture _fixture = new(dockerFixture);

        [Fact]
        public async Task AddTemporaryItemToShoppingListAsync_WithValidData_ShouldCreateAndAddTemporaryItemToShoppingList()
        {
            // Arrange
            _fixture.SetupExpectedItem();
            _fixture.SetupExpectedShoppingList();
            _fixture.SetupStore();
            _fixture.SetupExistingShoppingList();
            _fixture.SetupContract();
            await _fixture.SetupDatabaseAsync();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedShoppingList);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);

            // Act
            var result = await sut.AddTemporaryItemToShoppingListAsync(_fixture.ShoppingListId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            var items = (await _fixture.LoadAllItemsAsync(assertScope)).ToList();

            items.Should().HaveCount(1);
            var item = items.Single();
            item.Should().BeEquivalentTo(_fixture.ExpectedItem, opt =>
                opt.ExcludeItemCycleRef().ExcludeRowVersion()
                    .Excluding(info => info.Path == "Id" || info.Path == "CreatedAt"));
            item.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(20));

            var itemId = item.Id;

            shoppingLists.Should().HaveCount(1);
            var shoppingList = shoppingLists.Single();
            shoppingList.Should().BeEquivalentTo(_fixture.ExpectedShoppingList, opt =>
                opt.ExcludeShoppingListCycleRef().ExcludeRowVersion()
                    .Excluding(info => info.Path == "ItemsOnList[0].ItemId" || info.Path == "ItemsOnList[0].Id")
                    .WithCreatedAtPrecision());
            shoppingList.ItemsOnList.First().ItemId.Should().Be(itemId);
        }

        private sealed class AddTemporaryItemToShoppingListAsyncFixture(DockerFixture dockerFixture)
            : ShoppingListControllerFixture(dockerFixture)
        {
            private Store? _store;
            private ShoppingList? _existingShoppingList;

            public Guid? ShoppingListId => ExpectedShoppingList?.Id;
            public Item? ExpectedItem { get; private set; }
            public ShoppingList? ExpectedShoppingList { get; private set; }
            public AddTemporaryItemToShoppingListContract? Contract { get; private set; }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedItem);
                TestPropertyNotSetException.ThrowIfNull(ExpectedShoppingList);

                Contract = new AddTemporaryItemToShoppingListContract(
                    ExpectedItem.Name,
                    ExpectedItem.QuantityType,
                    ExpectedShoppingList.ItemsOnList.First().Quantity,
                    ExpectedItem.AvailableAt.First().Price,
                    ExpectedItem.AvailableAt.First().DefaultSectionId,
                    ExpectedItem.CreatedFrom!.Value);
            }

            public void SetupExpectedItem()
            {
                ExpectedItem = ItemEntityMother
                    .Initial()
                    .WithAvailableAt(new AvailableAtEntityBuilder().CreateMany(1).ToList())
                    .WithQuantityTypeInPacket(QuantityTypeInPacket.Unit.ToInt())
                    .WithQuantityType(QuantityType.Unit.ToInt())
                    .WithQuantityInPacket(1)
                    .WithCreatedFrom(Guid.NewGuid())
                    .WithComment(string.Empty)
                    .WithoutManufacturerId()
                    .WithoutItemCategoryId()
                    .WithIsTemporary(true)
                    .WithoutUpdatedOn()
                    .WithCreatedAt(DateTimeOffset.UtcNow)
                    .Create();
            }

            public void SetupExpectedShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedItem);

                var shoppingListId = Guid.NewGuid();
                var items = new ItemsOnListEntityBuilder()
                    .WithShoppingListId(shoppingListId)
                    .WithoutItemTypeId()
                    .WithInBasket(false)
                    .WithSectionId(ExpectedItem.AvailableAt.First().DefaultSectionId)
                    .CreateMany(1)
                    .ToList();

                ExpectedShoppingList = ShoppingListEntityMother
                    .Empty()
                    .WithStoreId(ExpectedItem.AvailableAt.First().StoreId)
                    .WithItemsOnList(items)
                    .Create();
            }

            public void SetupExistingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedShoppingList);

                _existingShoppingList = ShoppingListEntityMother
                    .Empty()
                    .WithId(ExpectedShoppingList.Id)
                    .WithStoreId(ExpectedShoppingList.StoreId)
                    .WithCreatedAt(ExpectedShoppingList.CreatedAt)
                    .Create();
            }

            public void SetupStore()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedShoppingList);

                _store = StoreEntityMother
                    .ValidSections(ExpectedShoppingList.ItemsOnList.First().SectionId.ToMonoList())
                    .WithId(ExpectedShoppingList.StoreId)
                    .Create();
            }

            public async Task SetupDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingShoppingList);
                TestPropertyNotSetException.ThrowIfNull(_store);

                await ApplyMigrationsAsync(ArrangeScope);

                await using var storeContext = GetContextInstance<StoreContext>(ArrangeScope);
                await using var shoppingListContext = GetContextInstance<ShoppingListContext>(ArrangeScope);

                await storeContext.AddAsync(_store);
                await shoppingListContext.AddAsync(_existingShoppingList);

                await storeContext.SaveChangesAsync();
                await shoppingListContext.SaveChangesAsync();
            }
        }
    }

    private abstract class ShoppingListControllerFixture : DatabaseFixture
    {
        protected ShoppingListControllerFixture(DockerFixture dockerFixture) : base(dockerFixture)
        {
            ArrangeScope = CreateServiceScope();
        }

        protected readonly IServiceScope ArrangeScope;

        public ShoppingListController CreateSut()
        {
            var scope = CreateServiceScope();
            return scope.ServiceProvider.GetRequiredService<ShoppingListController>();
        }

        public override IEnumerable<DbContext> GetDbContexts(IServiceScope scope)
        {
            yield return scope.ServiceProvider.GetRequiredService<ShoppingListContext>();
            yield return scope.ServiceProvider.GetRequiredService<StoreContext>();
            yield return scope.ServiceProvider.GetRequiredService<ItemContext>();
            yield return scope.ServiceProvider.GetRequiredService<RecipeContext>();
        }

        public async Task<IEnumerable<ShoppingList>> LoadAllShoppingLists()
        {
            using var assertScope = CreateServiceScope();
            var dbContext = assertScope.ServiceProvider.GetRequiredService<ShoppingListContext>();

            return await dbContext.ShoppingLists.AsNoTracking()
                .Include(sl => sl.ItemsOnList)
                .ToListAsync();
        }
    }
}