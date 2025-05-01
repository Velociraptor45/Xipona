using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.Users.Ports;

namespace ProjectHermes.Xipona.Api.Domain.Users.Services.Update;

public class GeneralSettingsUpdateService : IGeneralSettingsUpdateService
{
    private readonly IGeneralSettingRepository _generalSettingRepository;

    public GeneralSettingsUpdateService(IGeneralSettingRepository generalSettingRepository)
    {
        _generalSettingRepository = generalSettingRepository;
    }

    public async Task UpdateAsync(Currency currency)
    {
        var settings = await _generalSettingRepository.GetAsync();
        settings.Update(currency);
        await _generalSettingRepository.StoreAsync(settings);
    }
}
