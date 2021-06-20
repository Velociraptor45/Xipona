using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Contexts;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Contexts
{
    public class ShoppingListContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<ShoppingListContext>
    {
        public ShoppingListContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShoppingListContext>();
            optionsBuilder.UseMySql(GetDbConnectionString(), GetVersion());

            return new ShoppingListContext(optionsBuilder.Options);
        }
    }
}