using Xipona.Api.Core.Converter;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.Common.Models;
using Xipona.Api.Domain.Users.Models;
using GeneralSetting = Xipona.Api.Repositories.Users.Entities.GeneralSetting;

namespace Xipona.Api.Repositories.Users.Converters.ToDomain;

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
