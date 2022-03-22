using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Contexts;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Contexts;

public class ItemCategoryContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<ItemCategoryContext>
{
    public ItemCategoryContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ItemCategoryContext>();
        optionsBuilder.UseMySql(GetDbConnectionString(), GetVersion());

        return new ItemCategoryContext(optionsBuilder.Options);
    }
}