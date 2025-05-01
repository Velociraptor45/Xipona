using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Users.Models;
using ProjectHermes.Xipona.Api.Repositories.Users.Contexts;
using GeneralSetting = ProjectHermes.Xipona.Api.Repositories.Users.Entities.GeneralSetting;

namespace ProjectHermes.Xipona.Api.Repositories.Users.Adapters;

public class GeneralSettingRepository
{
    private readonly GeneralSettingContext _dbContext;
    private readonly IToDomainConverter<GeneralSetting, IGeneralSetting> _toDomainConverter;
    private readonly IToContractConverter<IGeneralSetting, GeneralSetting> _toContractConverter;
    private readonly ILogger<GeneralSettingRepository> _logger;
    private readonly CancellationToken _cancellationToken;

    public GeneralSettingRepository(GeneralSettingContext dbContext,
        IToDomainConverter<GeneralSetting, IGeneralSetting> toDomainConverter,
        IToContractConverter<IGeneralSetting, GeneralSetting> toContractConverter,
        ILogger<GeneralSettingRepository> logger,
        CancellationToken cancellationToken)
    {
        _dbContext = dbContext;
        _toDomainConverter = toDomainConverter;
        _toContractConverter = toContractConverter;
        _logger = logger;
        _cancellationToken = cancellationToken;
    }

    public async Task<IGeneralSetting> StoreAsync(IGeneralSetting model)
    {
        var convertedEntity = _toContractConverter.ToContract(model);
        var existingEntity = await GetTrackedEntity(_cancellationToken);

        if (existingEntity is null)
        {
            _dbContext.Entry(convertedEntity).State = EntityState.Added;
        }
        else
        {
            var existingRowVersion = existingEntity.RowVersion;
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(convertedEntity);
            _dbContext.Entry(existingEntity).State = EntityState.Modified;
            _dbContext.Entry(existingEntity).Property(r => r.RowVersion).OriginalValue = existingRowVersion;
        }

        try
        {
            await _dbContext.SaveChangesAsync(_cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogInformation(ex,
                "Saving general setting '{GeneralSettingId}' failed due to concurrency violation", model.Id.Value);
            throw new DomainException(new ModelOutOfDateReason());
        }

        return _toDomainConverter.ToDomain(convertedEntity);
    }

    private async Task<GeneralSetting?> GetTrackedEntity(CancellationToken cancellationToken)
    {
        return await _dbContext.GeneralSettings.SingleAsync(cancellationToken);
    }
}
