using Microsoft.Extensions.Caching.Memory;
using Xipona.Api.Core.Constants;
using Xipona.Api.Domain.Common.Models;
using Xipona.Api.Domain.Users.Ports;

namespace Xipona.Api.Domain.Users.Services.Update;

public class GeneralSettingsUpdateService : IGeneralSettingsUpdateService
{
    private readonly IGeneralSettingRepository _generalSettingRepository;
    private readonly IMemoryCache _cache;

    public GeneralSettingsUpdateService(IGeneralSettingRepository generalSettingRepository, IMemoryCache cache)
    {
        _generalSettingRepository = generalSettingRepository;
        _cache = cache;
    }

    public async Task UpdateAsync(Currency currency)
    {
        var settings = await _generalSettingRepository.GetAsync();
        settings.Update(currency);

        await _generalSettingRepository.StoreAsync(settings);

        _cache.Set(CacheKeys.GeneralSettings, settings);
    }
}
