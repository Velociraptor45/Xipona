using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Deletions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories;

public static class ServiceCollectionExtensions
{
    internal static void AddItemCategories(this IServiceCollection services)
    {
        services.AddTransient<IItemCategoryFactory, ItemCategoryFactory>();
        services.AddTransient<Func<CancellationToken, IItemCategoryValidationService>>(provider =>
        {
            return ct => new ItemCategoryValidationService(
                provider.GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>(),
                ct);
        });

        services.AddTransient<Func<CancellationToken, IItemCategoryQueryService>>(provider =>
        {
            var itemCategoryRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>();
            return cancellationToken => new ItemCategoryQueryService(itemCategoryRepositoryDelegate, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemCategoryCreationService>>(provider =>
        {
            var itemCategoryRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>();
            var itemCategoryFactory = provider.GetRequiredService<IItemCategoryFactory>();
            return cancellationToken => new ItemCategoryCreationService(itemCategoryRepositoryDelegate, itemCategoryFactory,
                cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemCategoryDeletionService>>(provider =>
        {
            var itemCategoryRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>();
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var shoppingListRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IShoppingListRepository>>();
            return cancellationToken => new ItemCategoryDeletionService(itemCategoryRepositoryDelegate, itemRepository,
                shoppingListRepositoryDelegate, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemCategoryModificationService>>(provider =>
        {
            var itemCategoryRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>();
            return cancellationToken => new ItemCategoryModificationService(itemCategoryRepositoryDelegate, cancellationToken);
        });
    }
}