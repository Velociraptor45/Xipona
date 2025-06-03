using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.Xipona.Api.Repositories.Common.Contexts;

namespace ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Contexts;

public class ShoppingListContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<ShoppingListContext>
{
    public ShoppingListContext CreateDbContext(string[] args)
    {
        var optionsBuilder = GetOptionBuilder<ShoppingListContext>();

        return new ShoppingListContext(optionsBuilder.Options);
    }
}