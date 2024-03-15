using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.ShoppingList.Api.Repositories.Common.Contexts;

namespace ProjectHermes.ShoppingList.Api.Repositories.Stores.Contexts;

public class StoreContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<StoreContext>
{
    public StoreContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StoreContext>();
        optionsBuilder.UseMySql(GetDbConnectionString(), GetVersion());

        return new StoreContext(optionsBuilder.Options);
    }
}