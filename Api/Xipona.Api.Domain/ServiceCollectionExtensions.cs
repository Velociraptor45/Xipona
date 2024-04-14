using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.ItemCategories;
using ProjectHermes.Xipona.Api.Domain.Items;
using ProjectHermes.Xipona.Api.Domain.Manufacturers;
using ProjectHermes.Xipona.Api.Domain.Recipes;
using ProjectHermes.Xipona.Api.Domain.RecipeTags;
using ProjectHermes.Xipona.Api.Domain.Shared;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists;
using ProjectHermes.Xipona.Api.Domain.Stores;
using System.Reflection;

namespace ProjectHermes.Xipona.Api.Domain;

public static class ServiceCollectionExtensions
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddItemCategories();
        services.AddManufacturers();
        services.AddShared();
        services.AddShoppingLists();
        services.AddItems();
        services.AddRecipes();
        services.AddRecipeTags();
        services.AddStores();

        var assembly = Assembly.GetExecutingAssembly();
        services.AddImplementationOfGenericType(assembly, typeof(IDomainEventHandler<>));
    }
}