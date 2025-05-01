using ProjectHermes.Xipona.Api.Domain.Common.Models;

namespace ProjectHermes.Xipona.Api.Domain.Users.Models;

public interface IGeneralSetting
{
    GeneralSettingId Id { get; }
    Currency Currency { get; }
    void Update(Currency currency);
}
