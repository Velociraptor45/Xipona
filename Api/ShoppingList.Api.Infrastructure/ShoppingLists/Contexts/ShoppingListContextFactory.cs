using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Contexts
{
    public class ShoppingListContextFactory : IDesignTimeDbContextFactory<ShoppingListContext>
    {
        public ShoppingListContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShoppingListContext>();
            optionsBuilder.UseMySql(
                @"server=192.168.178.92;port=15909;user id=root;pwd=;database=prd-shoppinglist",
                new MySqlServerVersion(new Version(0, 4, 0)));

            return new ShoppingListContext(optionsBuilder.Options);
        }
    }
}