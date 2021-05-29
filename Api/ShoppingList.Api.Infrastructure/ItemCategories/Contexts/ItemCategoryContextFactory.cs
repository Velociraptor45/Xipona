using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Contexts
{
    public class ItemCategoryContextFactory : IDesignTimeDbContextFactory<ItemCategoryContext>
    {
        public ItemCategoryContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ItemCategoryContext>();
            optionsBuilder.UseMySql(
                @"server=192.168.178.92;port=15909;user id=root;pwd=;database=prd-shoppinglist",
                new MySqlServerVersion(new Version(0, 4, 0)));

            return new ItemCategoryContext(optionsBuilder.Options);
        }
    }
}