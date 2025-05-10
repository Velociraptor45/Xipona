using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.Xipona.Api.Repositories.Common.Contexts;

namespace ProjectHermes.Xipona.Api.Repositories.Users.Contexts;

public class GeneralSettingContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<GeneralSettingContext>
{
    public GeneralSettingContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GeneralSettingContext>();
        optionsBuilder.UseMySql(GetDbConnectionString(), GetVersion());

        return new GeneralSettingContext(optionsBuilder.Options);
    }
}
