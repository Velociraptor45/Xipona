using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Contexts;
using ShoppingList.Api.Domain.TestKit.Stores.Models;
using Xunit;

namespace ShoppingList.Api.Endpoint.IntegrationsTests.v1.Controllers;

public class StoreControllerIntegrationTests : IClassFixture<DockerFixture>
{
    private readonly LocalFixture _fixture;

    public StoreControllerIntegrationTests(DockerFixture dockerFixture)
    {
        _fixture = new LocalFixture(dockerFixture);
    }

    [Fact]
    public async Task CreateStoreAsync()
    {
        // Arrange
        using (var arrangementScope = _fixture.CreateNewServiceScope())
        {
            await _fixture.SetupDatabaseAsync(arrangementScope);
        }

        var sut = _fixture.CreateSut();

        var contract = new CreateStoreContract("MyCoolStore",
            new List<CreateSectionContract> { new("MyCoolSection", 0, true) });

        // Act
        await sut.CreateStoreAsync(contract);

        // Assert
        using (var assertionScope = _fixture.CreateNewServiceScope())
        {
            var repo = _fixture.CreateStoreRepository(assertionScope);

            using (await _fixture.CreateTransactionAsync(assertionScope))
            {
                var stores = await repo.GetAsync(default);

                stores.Should().HaveCount(1);
                stores.First().Name.Should().Be(new StoreName("MyCoolStore"));
            }
        }
    }

    [Fact]
    public async Task UpdateStoreAsync()
    {
        // Arrange

        Store store;
        using (var arrangementScope = _fixture.CreateNewServiceScope())
        {
            await _fixture.SetupDatabaseAsync(arrangementScope);

            using (var t = await _fixture.CreateTransactionAsync(arrangementScope))
            {
                var repo = _fixture.CreateStoreRepository(arrangementScope);
                store = StoreMother.Initial().Create();
                await repo.StoreAsync(store, default);
                await t.CommitAsync(default);
            }
        }

        var sut = _fixture.CreateSut();

        var contract = new UpdateStoreContract(store.Id.Value, "MyStore", new List<UpdateSectionContract>()
        {
            new(store.Sections.First().Id.Value, "mySection", 0, true)
        });

        // Act
        var response = await sut.UpdateStoreAsync(contract);

        // Assert
        using (var assertionScope = _fixture.CreateNewServiceScope())
        {
            response.Should().BeOfType<OkResult>();

            using (await _fixture.CreateTransactionAsync(assertionScope))
            {
                var repo = _fixture.CreateStoreRepository(assertionScope);
                var stores = await repo.GetAsync(default);

                stores.Should().HaveCount(1);
                stores.First().Name.Should().Be(new StoreName("MyStore"));
            }
        }
    }

    private class LocalFixture : DatabaseFixture
    {
        public LocalFixture(DockerFixture dockerFixture) : base(dockerFixture)
        {
        }

        public StoreController CreateSut()
        {
            var scope = CreateNewServiceScope();
            return scope.ServiceProvider.GetRequiredService<StoreController>();
        }

        public override IEnumerable<DbContext> GetDbContexts(IServiceScope scope)
        {
            yield return scope.ServiceProvider.GetRequiredService<ShoppingListContext>();
            yield return scope.ServiceProvider.GetRequiredService<StoreContext>();
        }

        public IStoreRepository CreateStoreRepository(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<IStoreRepository>();
        }
    }
}