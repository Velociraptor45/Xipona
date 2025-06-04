using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Core.Constants;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.Users.Models;
using ProjectHermes.Xipona.Api.Domain.Users.Ports;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.Xipona.Api.WebApp.BackgroundServices;

public class DatabaseMigrationBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Func<CancellationToken, IGeneralSettingRepository> _generalSettingRepositoryDelegate;
    private readonly IMemoryCache _cache;
    private readonly ILogger<DatabaseMigrationBackgroundService> _logger;

    public DatabaseMigrationBackgroundService(IServiceProvider serviceProvider,
        Func<CancellationToken, IGeneralSettingRepository> generalSettingRepositoryDelegate,
        IMemoryCache cache,
        ILogger<DatabaseMigrationBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _generalSettingRepositoryDelegate = generalSettingRepositoryDelegate;
        _cache = cache;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContexts = scope.ServiceProvider.GetRequiredService<IList<DbContext>>();

        _logger.LogInformation("Starting database migration");
        var sw = Stopwatch.StartNew();

        foreach (var dbContext in dbContexts)
        {
            await dbContext.Database.MigrateAsync(stoppingToken);
        }

        sw.Stop();
        _logger.LogInformation("Finished database migration in {Elapsed}", sw.Elapsed);

        var generalSettingsRepo = _generalSettingRepositoryDelegate(stoppingToken);

        IGeneralSetting generalSettings;
        if (await generalSettingsRepo.Exists())
        {
            generalSettings = await generalSettingsRepo.GetAsync();
        }
        else
        {
            generalSettings = new GeneralSetting(new GeneralSettingId(0), Currency.Euro);
            await generalSettingsRepo.StoreAsync(generalSettings);
        }

        _cache.Set(CacheKeys.GeneralSettings, generalSettings);
    }
}