using Microsoft.Extensions.DependencyInjection;
using Xipona.Api.Domain.ItemCategories.Models.Factories;
using Xipona.Api.Domain.ItemCategories.Ports;
using Xipona.Api.Domain.ItemCategories.Services.Creations;
using Xipona.Api.Domain.ItemCategories.Services.Deletions;
using Xipona.Api.Domain.ItemCategories.Services.Modifications;
using Xipona.Api.Domain.ItemCategories.Services.Queries;
using Xipona.Api.Domain.ItemCategories.Services.Validations;
using Xipona.Api.Domain.Items.Ports;
using Xipona.Api.Domain.ShoppingLists.Ports;

namespace Xipona.Api.Domain.ItemCategories;

public static class ServiceCollectionExtensions
{
    internal static void AddItemCategories(this IServiceCollection services)
    {
        services.AddTransient<IItemCategoryFactory, ItemCategoryFactory>();
        services.AddTransient<Func<CancellationToken, IItemCategoryValidationService>>(provider =>
        {
            return ct => new ItemCategoryValidationService(
                provider.GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>()(ct));
        });

        services.AddTransient<Func<CancellationToken, IItemCategoryQueryService>>(provider =>
        {
            var itemCategoryRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>();
            return ct => new ItemCategoryQueryService(itemCategoryRepositoryDelegate(ct));
        });

        services.AddTransient<Func<CancellationToken, IItemCategoryCreationService>>(provider =>
        {
            var itemCategoryRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>();
            var itemCategoryFactory = provider.GetRequiredService<IItemCategoryFactory>();
            return ct => new ItemCategoryCreationService(itemCategoryRepositoryDelegate(ct), itemCategoryFactory);
        });

        services.AddTransient<Func<CancellationToken, IItemCategoryDeletionService>>(provider =>
        {
            var itemCategoryRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>();
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            var shoppingListRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IShoppingListRepository>>();
            return ct => new ItemCategoryDeletionService(itemCategoryRepositoryDelegate(ct),
                itemRepositoryDelegate(ct), shoppingListRepositoryDelegate(ct));
        });

        services.AddTransient<Func<CancellationToken, IItemCategoryModificationService>>(provider =>
        {
            var itemCategoryRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>();
            return ct => new ItemCategoryModificationService(itemCategoryRepositoryDelegate(ct));
        });
    }
}