using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.Xipona.Api.Repositories.Common.Contexts;

namespace ProjectHermes.Xipona.Api.Repositories.Recipes.Contexts;

public class RecipeContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<RecipeContext>
{
    public RecipeContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RecipeContext>();
        optionsBuilder.UseMySql(GetDbConnectionString(), GetVersion());

        return new RecipeContext(optionsBuilder.Options);
    }
}