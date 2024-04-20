using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.Xipona.Api.Repositories.Common.Contexts;

namespace ProjectHermes.Xipona.Api.Repositories.Stores.Contexts;

public class StoreContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<StoreContext>
{
    public StoreContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StoreContext>();
        optionsBuilder.UseMySql(GetDbConnectionString(), GetVersion());

        return new StoreContext(optionsBuilder.Options);
    }
}