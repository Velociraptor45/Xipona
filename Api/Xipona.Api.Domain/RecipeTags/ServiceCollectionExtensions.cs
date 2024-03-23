using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Ports;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Services.Creation;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Services.Query;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Services.Validations;

namespace ProjectHermes.Xipona.Api.Domain.RecipeTags;

internal static class ServiceCollectionExtensions
{
    public static void AddRecipeTags(this IServiceCollection services)
    {
        services.AddTransient<IRecipeTagFactory, RecipeTagFactory>();

        services.AddTransient<Func<CancellationToken, IRecipeTagQueryService>>(s =>
        {
            var repo = s.GetRequiredService<Func<CancellationToken, IRecipeTagRepository>>();
            return ct => new RecipeTagQueryService(repo(ct));
        });

        services.AddTransient<Func<CancellationToken, IRecipeTagCreationService>>(s =>
        {
            var repo = s.GetRequiredService<Func<CancellationToken, IRecipeTagRepository>>();
            var factory = s.GetRequiredService<IRecipeTagFactory>();
            return ct => new RecipeTagCreationService(factory, repo(ct));
        });

        services.AddTransient<Func<CancellationToken, IRecipeTagValidationService>>(s =>
        {
            var repo = s.GetRequiredService<Func<CancellationToken, IRecipeTagRepository>>();
            return ct => new RecipeTagValidationService(repo(ct));
        });
    }
}