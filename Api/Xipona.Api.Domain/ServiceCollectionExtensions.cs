using Microsoft.Extensions.DependencyInjection;
using Xipona.Api.Domain.ItemCategories;
using Xipona.Api.Domain.Items;
using Xipona.Api.Domain.Manufacturers;
using Xipona.Api.Domain.Recipes;
using Xipona.Api.Domain.RecipeTags;
using Xipona.Api.Domain.Shared;
using Xipona.Api.Domain.ShoppingLists;
using Xipona.Api.Domain.Stores;
using Xipona.Api.Domain.Users;

namespace Xipona.Api.Domain;

public static class ServiceCollectionExtensions
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddUsers();
        services.AddItemCategories();
        services.AddManufacturers();
        services.AddShared();
        services.AddShoppingLists();
        services.AddItems();
        services.AddRecipes();
        services.AddRecipeTags();
        services.AddStores();

        services.AddDomainEventHandlers();

        services.AddSingleton(TimeProvider.System);
    }
}