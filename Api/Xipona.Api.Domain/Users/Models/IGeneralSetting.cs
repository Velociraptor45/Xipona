using Xipona.Api.Domain.Common.Models;

namespace Xipona.Api.Domain.Users.Models;

public interface IGeneralSetting
{
    GeneralSettingId Id { get; }
    Currency Currency { get; }
    string CurrencySymbol { get; }
    void Update(Currency currency);
}
