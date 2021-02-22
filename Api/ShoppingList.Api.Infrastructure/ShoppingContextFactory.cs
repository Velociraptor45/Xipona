using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;

namespace ProjectHermes.ShoppingLists.Api.Infrastructure
{
    public class ShoppingContextFactory : IDesignTimeDbContextFactory<ShoppingContext>
    {
        public ShoppingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShoppingContext>();
            optionsBuilder.UseMySql(@"server=192.168.178.92;port=15909;user id=root;pwd=5NiSgRpY2R6KLlA5uJIOJ4IZZ;database=shoppinglist-development");

            return new ShoppingContext(optionsBuilder.Options);
        }
    }
}