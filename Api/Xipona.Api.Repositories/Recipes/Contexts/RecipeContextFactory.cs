using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.Xipona.Api.Repositories.Common.Contexts;

namespace ProjectHermes.Xipona.Api.Repositories.Recipes.Contexts;

public class RecipeContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<RecipeContext>
{
    public RecipeContext CreateDbContext(string[] args)
    {
        var optionsBuilder = GetOptionBuilder<RecipeContext>();

        return new RecipeContext(optionsBuilder.Options);
    }
}