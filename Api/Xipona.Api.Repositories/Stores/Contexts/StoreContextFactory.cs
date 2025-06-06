using Microsoft.EntityFrameworkCore.Design;
using Xipona.Api.Repositories.Common.Contexts;

namespace Xipona.Api.Repositories.Stores.Contexts;

public class StoreContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<StoreContext>
{
    public StoreContext CreateDbContext(string[] args)
    {
        var optionsBuilder = GetOptionBuilder<StoreContext>();

        return new StoreContext(optionsBuilder.Options);
    }
}