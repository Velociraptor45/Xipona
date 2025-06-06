using Xipona.Api.Domain.Users.Models;

namespace Xipona.Api.Domain.Users.Ports;

public interface IGeneralSettingRepository
{
    Task<IGeneralSetting> StoreAsync(IGeneralSetting model);
    Task<IGeneralSetting> GetAsync();
}
