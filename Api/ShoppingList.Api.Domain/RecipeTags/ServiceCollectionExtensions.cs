using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Ports;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Services.Creation;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Services.Query;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Services.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags;

internal static class ServiceCollectionExtensions
{
    public static void AddRecipeTags(this IServiceCollection services)
    {
        services.AddTransient<IRecipeTagFactory, RecipeTagFactory>();

        services.AddTransient<Func<CancellationToken, IRecipeTagQueryService>>(s =>
        {
            var repo = s.GetRequiredService<Func<CancellationToken, IRecipeTagRepository>>();
            return token => new RecipeTagQueryService(repo, token);
        });

        services.AddTransient<Func<CancellationToken, IRecipeTagCreationService>>(s =>
        {
            var repo = s.GetRequiredService<Func<CancellationToken, IRecipeTagRepository>>();
            var factory = s.GetRequiredService<IRecipeTagFactory>();
            return token => new RecipeTagCreationService(factory, repo, token);
        });

        services.AddTransient<Func<CancellationToken, IRecipeTagValidationService>>(s =>
        {
            var repo = s.GetRequiredService<Func<CancellationToken, IRecipeTagRepository>>();
            return token => new RecipeTagValidationService(repo, token);
        });
    }
}