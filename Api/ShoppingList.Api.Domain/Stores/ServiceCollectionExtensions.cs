using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Deletions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores;

public static class ServiceCollectionExtensions
{
    internal static void AddStores(this IServiceCollection services)
    {
        services.AddTransient<IStoreFactory, StoreFactory>();

        services.AddTransient<ISectionFactory, SectionFactory>();

        services.AddTransient<Func<CancellationToken, IStoreCreationService>>(provider =>
        {
            var storeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>();
            var storeFactory = provider.GetRequiredService<IStoreFactory>();
            var shoppingListFactory = provider.GetRequiredService<IShoppingListFactory>();
            var shoppingListRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IShoppingListRepository>>();
            return ct => new StoreCreationService(storeRepositoryDelegate(ct), storeFactory, shoppingListFactory,
                    shoppingListRepositoryDelegate(ct));
        });

        services.AddTransient<Func<CancellationToken, IStoreModificationService>>(provider =>
        {
            var storeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>();
            var itemModificationServiceDelegate =
                provider.GetRequiredService<Func<CancellationToken, IItemModificationService>>();
            var shoppingListModificationServiceDelegate =
                provider.GetRequiredService<Func<CancellationToken, IShoppingListModificationService>>();
            return ct => new StoreModificationService(storeRepositoryDelegate(ct), itemModificationServiceDelegate(ct),
                    shoppingListModificationServiceDelegate(ct));
        });

        services.AddTransient<Func<CancellationToken, IStoreQueryService>>(provider =>
        {
            var storeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>();
            return ct => new StoreQueryService(storeRepositoryDelegate(ct));
        });

        services.AddTransient<Func<CancellationToken, IStoreDeletionService>>(provider =>
        {
            var storeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>();
            return ct => new StoreDeletionService(storeRepositoryDelegate(ct));
        });
    }
}