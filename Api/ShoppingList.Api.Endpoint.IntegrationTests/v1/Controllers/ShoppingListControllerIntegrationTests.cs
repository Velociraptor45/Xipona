using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.Items.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.ShoppingLists.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.Stores.Entities;
using ProjectHermes.ShoppingList.Api.TestTools.AutoFixture;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System;
using Xunit;
using Item = ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.Item;

namespace ProjectHermes.ShoppingList.Api.Endpoint.IntegrationTests.v1.Controllers;

public class ShoppingListControllerIntegrationTests
{
    public sealed class FinishListAsync : IAssemblyFixture<DockerFixture>
    {
        private readonly FinishListAsyncFixture _fixture;

        public FinishListAsync(DockerFixture dockerFixture)
        {
            _fixture = new FinishListAsyncFixture(dockerFixture);
        }

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
            result.Should().BeOfType<OkResult>();
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
            result.Should().BeOfType<OkResult>();
            var allShoppingLists = (await _fixture.LoadAllShoppingLists()).ToList();
            allShoppingLists.Should().HaveCount(2);
            allShoppingLists.Should().ContainSingle(sl => sl.Id == _fixture.ShoppingListId.Value);

            var finishedShoppingList = allShoppingLists.Single(sl => sl.Id == _fixture.ShoppingListId.Value);
            finishedShoppingList.CompletionDate.Should().NotBeNull();
            finishedShoppingList.CompletionDate.Should().BeCloseTo(_fixture.FinishedAt.Value, TimeSpan.FromMilliseconds(10));
        }

        private sealed class FinishListAsyncFixture : ShoppingListControllerFixture
        {
            private Repositories.ShoppingLists.Entities.ShoppingList? _existingShoppingList;

            public FinishListAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

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

                itemContext.Add(_existingShoppingList);

                await itemContext.SaveChangesAsync();
            }
        }
    }

    public sealed class AddTemporaryItemToShoppingListAsync : IAssemblyFixture<DockerFixture>
    {
        private readonly AddTemporaryItemToShoppingListAsyncFixture _fixture;

        public AddTemporaryItemToShoppingListAsync(DockerFixture dockerFixture)
        {
            _fixture = new AddTemporaryItemToShoppingListAsyncFixture(dockerFixture);
        }

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
            result.Should().BeOfType<OkResult>();

            using var assertScope = _fixture.CreateServiceScope();

            var shoppingLists = (await _fixture.LoadAllShoppingListsAsync(assertScope)).ToList();
            var items = (await _fixture.LoadAllItemsAsync(assertScope)).ToList();

            items.Should().HaveCount(1);
            var item = items.Single();
            item.Should().BeEquivalentTo(_fixture.ExpectedItem, opt =>
                opt.ExcludeItemCycleRef().ExcludeRowVersion().Excluding(info => info.Path == "Id"));

            var itemId = item.Id;

            shoppingLists.Should().HaveCount(1);
            var shoppingList = shoppingLists.Single();
            shoppingList.Should().BeEquivalentTo(_fixture.ExpectedShoppingList, opt =>
                opt.ExcludeShoppingListCycleRef().ExcludeRowVersion()
                    .Excluding(info => info.Path == "ItemsOnList[0].ItemId" || info.Path == "ItemsOnList[0].Id"));
            shoppingList.ItemsOnList.First().ItemId.Should().Be(itemId);
        }

        private sealed class AddTemporaryItemToShoppingListAsyncFixture : ShoppingListControllerFixture
        {
            private Store? _store;
            private Repositories.ShoppingLists.Entities.ShoppingList? _existingShoppingList;

            public AddTemporaryItemToShoppingListAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public Guid? ShoppingListId => ExpectedShoppingList?.Id;
            public Item? ExpectedItem { get; private set; }
            public Repositories.ShoppingLists.Entities.ShoppingList? ExpectedShoppingList { get; private set; }
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

                storeContext.Add(_store);
                shoppingListContext.Add(_existingShoppingList);

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
        }

        public async Task<IEnumerable<Repositories.ShoppingLists.Entities.ShoppingList>> LoadAllShoppingLists()
        {
            using var assertScope = CreateServiceScope();
            var dbContext = assertScope.ServiceProvider.GetRequiredService<ShoppingListContext>();

            return await dbContext.ShoppingLists.AsNoTracking()
                .Include(sl => sl.ItemsOnList)
                .ToListAsync();
        }
    }
}