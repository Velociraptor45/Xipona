using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using System;

namespace ProjectHermes.ShoppingLists.Api.Infrastructure
{
    public class ShoppingContextFactory : IDesignTimeDbContextFactory<ShoppingContext>
    {
        public ShoppingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShoppingContext>();
            optionsBuilder.UseMySql(
                @"server=;port=15909;user id=root;pwd=;database=shoppinglist-development",
                new MySqlServerVersion(new Version(0, 3, 1)));

            return new ShoppingContext(optionsBuilder.Options);
        }
    }
}