using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.Users.Models;
using GeneralSetting = ProjectHermes.Xipona.Api.Repositories.Users.Entities.GeneralSetting;

namespace ProjectHermes.Xipona.Api.Repositories.Users.Converters.ToDomain;

public class GeneralSettingConverter : IToDomainConverter<GeneralSetting, IGeneralSetting>
{
    public IGeneralSetting ToDomain(GeneralSetting source)
    {
        AggregateRoot setting = new Domain.Users.Models.GeneralSetting(
            new GeneralSettingId(source.Id),
            source.Currency.ToEnum<Currency>());

        setting.EnrichWithRowVersion(source.RowVersion);

        return (setting as IGeneralSetting)!;
    }
}
