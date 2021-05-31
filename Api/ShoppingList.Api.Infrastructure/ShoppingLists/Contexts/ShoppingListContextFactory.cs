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
            optionsBuilder.UseMySql(
                @"server=192.168.178.92;port=15906;user id=root;pwd=;database=dev-shoppinglist",
                GetVersion());

            return new ShoppingListContext(optionsBuilder.Options);
        }
    }
}