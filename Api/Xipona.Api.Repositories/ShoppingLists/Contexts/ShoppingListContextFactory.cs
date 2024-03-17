using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.Xipona.Api.Repositories.Common.Contexts;

namespace ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Contexts;

public class ShoppingListContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<ShoppingListContext>
{
    public ShoppingListContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ShoppingListContext>();
        optionsBuilder.UseMySql(GetDbConnectionString(), GetVersion());

        return new ShoppingListContext(optionsBuilder.Options);
    }
}