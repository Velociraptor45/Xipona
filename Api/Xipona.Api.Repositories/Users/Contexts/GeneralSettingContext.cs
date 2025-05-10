using Microsoft.EntityFrameworkCore;
using ProjectHermes.Xipona.Api.Repositories.Users.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.Users.Contexts;

public class GeneralSettingContext : DbContext
{
    public DbSet<GeneralSetting> GeneralSettings { get; set; }

    public GeneralSettingContext(DbContextOptions<GeneralSettingContext> options) : base(options)
    {

    }
}
