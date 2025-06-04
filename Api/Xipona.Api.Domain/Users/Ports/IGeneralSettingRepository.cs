using ProjectHermes.Xipona.Api.Domain.Users.Models;

namespace ProjectHermes.Xipona.Api.Domain.Users.Ports;

public interface IGeneralSettingRepository
{
    Task<IGeneralSetting> StoreAsync(IGeneralSetting model);
    Task<IGeneralSetting> GetAsync();
    Task<bool> Exists();
}
