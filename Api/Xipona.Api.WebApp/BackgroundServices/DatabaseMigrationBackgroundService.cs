using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Core.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.Xipona.Api.WebApp.BackgroundServices;

public class DatabaseMigrationBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseMigrationBackgroundService> _logger;

    public DatabaseMigrationBackgroundService(IServiceProvider serviceProvider,
        ILogger<DatabaseMigrationBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
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
    }
}