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
                @"server=192.168.178.92;port=15909;user id=root;pwd=;database=prd-shoppinglist",
                new MySqlServerVersion(new Version(0, 4, 0)));

            return new ShoppingContext(optionsBuilder.Options);
        }
    }
}