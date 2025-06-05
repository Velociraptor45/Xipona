using Microsoft.EntityFrameworkCore.Design;
using Xipona.Api.Repositories.Common.Contexts;

namespace Xipona.Api.Repositories.RecipeTags.Contexts;

public class RecipeTagContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<RecipeTagContext>
{
    public RecipeTagContext CreateDbContext(string[] args)
    {
        var optionsBuilder = GetOptionBuilder<RecipeTagContext>();

        return new RecipeTagContext(optionsBuilder.Options);
    }
}