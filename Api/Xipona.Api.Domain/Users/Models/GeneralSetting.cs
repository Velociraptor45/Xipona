using ProjectHermes.Xipona.Api.Domain.Common.Models;

namespace ProjectHermes.Xipona.Api.Domain.Users.Models;

public class GeneralSetting : AggregateRoot, IGeneralSetting
{

    public GeneralSetting(GeneralSettingId id, Currency currency)
    {
        Id = id;
        Currency = currency;
    }

    public GeneralSettingId Id { get; }
    public Currency Currency { get; }
}
