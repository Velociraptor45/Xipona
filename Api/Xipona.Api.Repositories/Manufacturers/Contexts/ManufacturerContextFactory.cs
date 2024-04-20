using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.Xipona.Api.Repositories.Common.Contexts;

namespace ProjectHermes.Xipona.Api.Repositories.Manufacturers.Contexts;

public class ManufacturerContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<ManufacturerContext>
{
    public ManufacturerContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ManufacturerContext>();
        optionsBuilder.UseMySql(GetDbConnectionString(), GetVersion());

        return new ManufacturerContext(optionsBuilder.Options);
    }
}