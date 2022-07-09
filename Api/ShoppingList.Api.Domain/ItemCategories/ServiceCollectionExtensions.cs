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
        services.AddTransient<IItemCategoryValidationService, ItemCategoryValidationService>();

        services.AddTransient<Func<CancellationToken, IItemCategoryQueryService>>(provider =>
        {
            var itemCategoryRepository = provider.GetRequiredService<IItemCategoryRepository>();
            return cancellationToken => new ItemCategoryQueryService(itemCategoryRepository, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemCategoryCreationService>>(provider =>
        {
            var itemCategoryRepository = provider.GetRequiredService<IItemCategoryRepository>();
            var itemCategoryFactory = provider.GetRequiredService<IItemCategoryFactory>();
            return cancellationToken => new ItemCategoryCreationService(itemCategoryRepository, itemCategoryFactory,
                cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemCategoryDeletionService>>(provider =>
        {
            var itemCategoryRepository = provider.GetRequiredService<IItemCategoryRepository>();
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            var shoppingListRepository = provider.GetRequiredService<IShoppingListRepository>();
            return cancellationToken => new ItemCategoryDeletionService(itemCategoryRepository, itemRepository,
                shoppingListRepository, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IItemCategoryModificationService>>(provider =>
        {
            var itemCategoryRepository = provider.GetRequiredService<IItemCategoryRepository>();
            return cancellationToken => new ItemCategoryModificationService(itemCategoryRepository, cancellationToken);
        });
    }
}