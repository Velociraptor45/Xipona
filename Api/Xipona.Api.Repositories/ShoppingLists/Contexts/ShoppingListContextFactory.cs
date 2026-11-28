using Microsoft.EntityFrameworkCore.Design;
using Xipona.Api.Repositories.Common.Contexts;

namespace Xipona.Api.Repositories.ShoppingLists.Contexts;

public class ShoppingListContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<ShoppingListContext>
{
    public ShoppingListContext CreateDbContext(string[] args)
    {
        var optionsBuilder = GetOptionBuilder<ShoppingListContext>();

        return new ShoppingListContext(optionsBuilder.Options);
    }
}