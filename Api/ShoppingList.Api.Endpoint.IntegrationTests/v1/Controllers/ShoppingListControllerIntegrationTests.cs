using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.ShoppingLists.Entities;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Endpoint.IntegrationTests.v1.Controllers;

public class ShoppingListControllerIntegrationTests
{
    [Collection(DockerCollection.Name)]
    public sealed class FinishListAsync
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
            private Infrastructure.ShoppingLists.Entities.ShoppingList? _existingShoppingList;

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
            //yield return scope.ServiceProvider.GetRequiredService<ItemContext>();
            //yield return scope.ServiceProvider.GetRequiredService<ItemCategoryContext>();
            //yield return scope.ServiceProvider.GetRequiredService<ManufacturerContext>();
            //yield return scope.ServiceProvider.GetRequiredService<StoreContext>();
        }

        public async Task<IEnumerable<Infrastructure.ShoppingLists.Entities.ShoppingList>> LoadAllShoppingLists()
        {
            using var assertScope = CreateServiceScope();
            var dbContext = assertScope.ServiceProvider.GetRequiredService<ShoppingListContext>();

            return await dbContext.ShoppingLists.AsNoTracking()
                .Include(sl => sl.ItemsOnList)
                .ToListAsync();
        }
    }
}