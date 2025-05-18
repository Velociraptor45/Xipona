using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Domain.ItemCategories;
using ProjectHermes.Xipona.Api.Domain.Items;
using ProjectHermes.Xipona.Api.Domain.Manufacturers;
using ProjectHermes.Xipona.Api.Domain.Recipes;
using ProjectHermes.Xipona.Api.Domain.RecipeTags;
using ProjectHermes.Xipona.Api.Domain.Shared;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists;
using ProjectHermes.Xipona.Api.Domain.Stores;
using ProjectHermes.Xipona.Api.Domain.Users;

namespace ProjectHermes.Xipona.Api.Domain;

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