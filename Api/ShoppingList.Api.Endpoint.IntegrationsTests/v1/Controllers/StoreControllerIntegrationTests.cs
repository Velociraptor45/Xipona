using Ductus.FluentDocker.Builders;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.ApplicationServices;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Domain;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Endpoint;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Infrastructure;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Contexts;
using System;
using System.IO;
using Xunit;

namespace ShoppingList.Api.Endpoint.IntegrationsTests.v1.Controllers;

public class StoreControllerIntegrationTests
{
    [Fact]
    public async Task Test()
    {
        var fileDir = Path.Combine(Directory.GetCurrentDirectory(), "docker-compose.yml");

        using (var c = new Builder()
                  .UseContainer()
                  .UseCompose()
                  .FromFile(fileDir)
                  .RemoveOrphans()
                  //.WaitForProcess("Database", "mariadb", millisTimeout: 30000)
                  //.WaitForPort("Database", "15906/tcp", 30000)
                  .Build()
                  .Start())
        {
            await Task.Delay(10000);

            var services = new ServiceCollection();
            services.AddDomain();
            services.AddEndpointControllers();
            services.AddInfrastructure("server=127.0.0.1;port=15906;database=dev-shoppinglist;user id=root;pwd=123root;AllowUserVariables=true;UseAffectedRows=false");
            services.AddApplicationServices();

            var provider = services.BuildServiceProvider();

            var contexts = GetDbContexts(provider);

            foreach (var context in contexts)
            {
                await context.Database.MigrateAsync();
                Console.WriteLine($"Context {context.GetType().Name} migrated");
            }

            var controller = provider.GetRequiredService<StoreController>();

            var contract = new CreateStoreContract("MyCoolStore",
                new List<CreateSectionContract> { new("MyCoolSection", 0, true) });

            await controller.CreateStoreAsync(contract);

            var repo = provider.GetRequiredService<IStoreRepository>();

            var generator = provider.GetRequiredService<ITransactionGenerator>();

            using (await generator.GenerateAsync(default))
            {
                var stores = await repo.GetAsync(default);

                stores.Should().HaveCount(1);
                stores.First().Name.Should().Be(new StoreName("MyCoolStore"));
            }
        }
    }

    private IEnumerable<DbContext> GetDbContexts(IServiceProvider provider)
    {
        yield return provider.GetRequiredService<ShoppingListContext>();
        //yield return provider.GetRequiredService<ItemCategoryContext>();
        //yield return provider.GetRequiredService<ManufacturerContext>();
        //yield return provider.GetRequiredService<ItemContext>();
        yield return provider.GetRequiredService<StoreContext>();
    }
}