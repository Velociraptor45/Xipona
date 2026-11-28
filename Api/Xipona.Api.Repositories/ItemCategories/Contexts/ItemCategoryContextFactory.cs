using Microsoft.EntityFrameworkCore.Design;
using Xipona.Api.Repositories.Common.Contexts;

namespace Xipona.Api.Repositories.ItemCategories.Contexts;

public class ItemCategoryContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<ItemCategoryContext>
{
    public ItemCategoryContext CreateDbContext(string[] args)
    {
        var optionsBuilder = GetOptionBuilder<ItemCategoryContext>();

        return new ItemCategoryContext(optionsBuilder.Options);
    }
}