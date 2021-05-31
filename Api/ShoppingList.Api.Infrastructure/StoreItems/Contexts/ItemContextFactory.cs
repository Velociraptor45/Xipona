using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Contexts;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Contexts
{
    public class ItemContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<ItemContext>
    {
        public ItemContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ItemContext>();
            optionsBuilder.UseMySql(
                @"server=192.168.178.92;port=15906;user id=root;pwd=;database=dev-shoppinglist",
                GetVersion());

            return new ItemContext(optionsBuilder.Options);
        }
    }
}