using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.Xipona.Api.Repositories.Common.Contexts;

namespace ProjectHermes.Xipona.Api.Repositories.RecipeTags.Contexts;

public class RecipeTagContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<RecipeTagContext>
{
    public RecipeTagContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RecipeTagContext>();
        optionsBuilder.UseMySql(GetDbConnectionString(), GetVersion());

        return new RecipeTagContext(optionsBuilder.Options);
    }
}