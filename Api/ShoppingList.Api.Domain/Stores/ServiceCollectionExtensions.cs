using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores;

public static class ServiceCollectionExtensions
{
    internal static void AddStores(this IServiceCollection services)
    {
        services.AddTransient<IStoreFactory, StoreFactory>();

        services.AddTransient<ISectionFactory, SectionFactory>();

        // services
        services.AddTransient<Func<CancellationToken, IStoreCreationService>>(provider =>
        {
            var storeRepository = provider.GetRequiredService<IStoreRepository>();
            var storeFactory = provider.GetRequiredService<IStoreFactory>();
            var shoppingListFactory = provider.GetRequiredService<IShoppingListFactory>();
            var shoppingListRepository = provider.GetRequiredService<IShoppingListRepository>();
            return cancellationToken =>
                new StoreCreationService(storeRepository, storeFactory, shoppingListFactory, shoppingListRepository,
                    cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IStoreUpdateService>>(provider =>
        {
            var storeRepository = provider.GetRequiredService<IStoreRepository>();
            return cancellationToken => new StoreUpdateService(storeRepository, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IStoreQueryService>>(provider =>
        {
            var storeRepository = provider.GetRequiredService<IStoreRepository>();
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            return cancellationToken => new StoreQueryService(storeRepository, itemRepository, cancellationToken);
        });
    }
}