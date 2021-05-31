using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Contexts;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Contexts
{
    public class StoreContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<StoreContext>
    {
        public StoreContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StoreContext>();
            optionsBuilder.UseMySql(
                @"server=192.168.178.92;port=15906;user id=root;pwd=;database=dev-shoppinglist",
                GetVersion());

            return new StoreContext(optionsBuilder.Options);
        }
    }
}