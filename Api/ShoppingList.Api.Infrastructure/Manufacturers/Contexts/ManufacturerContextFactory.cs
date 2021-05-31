using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Contexts;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Contexts
{
    public class ManufacturerContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<ManufacturerContext>
    {
        public ManufacturerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ManufacturerContext>();
            optionsBuilder.UseMySql(
                @"server=192.168.178.92;port=15906;user id=root;pwd=;database=dev-shoppinglist",
                GetVersion());

            return new ManufacturerContext(optionsBuilder.Options);
        }
    }
}