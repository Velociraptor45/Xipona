using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.WebApp.BackgroundServices;

public class DatabaseMigrationBackgroundService : BackgroundService
{
    private readonly IList<DbContext> _dbContexts;
    private readonly ILogger<DatabaseMigrationBackgroundService> _logger;

    public DatabaseMigrationBackgroundService(IList<DbContext> dbContexts,
        ILogger<DatabaseMigrationBackgroundService> logger)
    {
        _dbContexts = dbContexts;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting database migration");
        var sw = Stopwatch.StartNew();

        foreach (var dbContext in _dbContexts)
        {
            await dbContext.Database.MigrateAsync(stoppingToken);
        }

        sw.Stop();
        _logger.LogInformation($"Finished database migration in {sw.Elapsed}");
    }
}