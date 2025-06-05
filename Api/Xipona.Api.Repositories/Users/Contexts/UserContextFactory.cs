using Microsoft.EntityFrameworkCore.Design;
using ProjectHermes.Xipona.Api.Repositories.Common.Contexts;

namespace ProjectHermes.Xipona.Api.Repositories.Users.Contexts;

public class UserContextFactory : ContextFactoryBase, IDesignTimeDbContextFactory<UserContext>
{
    public UserContext CreateDbContext(string[] args)
    {
        var optionsBuilder = GetOptionBuilder<UserContext>();

        return new UserContext(optionsBuilder.Options);
    }
}
