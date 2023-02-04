using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

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

        services.AddTransient<Func<CancellationToken, IStoreModificationService>>(provider =>
        {
            var storeRepository = provider.GetRequiredService<IStoreRepository>();
            var itemModificationServiceDelegate =
                provider.GetRequiredService<Func<CancellationToken, IItemModificationService>>();
            var shoppingListModificationServiceDelegate =
                provider.GetRequiredService<Func<CancellationToken, IShoppingListModificationService>>();
            return cancellationToken =>
                new StoreModificationService(storeRepository, itemModificationServiceDelegate,
                    shoppingListModificationServiceDelegate, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IStoreQueryService>>(provider =>
        {
            var storeRepository = provider.GetRequiredService<IStoreRepository>();
            return cancellationToken => new StoreQueryService(storeRepository, cancellationToken);
        });
    }
}