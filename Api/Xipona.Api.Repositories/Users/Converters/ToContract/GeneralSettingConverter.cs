using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.Users.Models;
using GeneralSetting = ProjectHermes.Xipona.Api.Repositories.Users.Entities.GeneralSetting;

namespace ProjectHermes.Xipona.Api.Repositories.Users.Converters.ToContract;

public class GeneralSettingConverter : IToContractConverter<IGeneralSetting, GeneralSetting>
{
    public GeneralSetting ToContract(IGeneralSetting source)
    {
        return new GeneralSetting
        {
            Id = source.Id,
            Currency = source.Currency.ToInt(),
            RowVersion = ((AggregateRoot)source).RowVersion
        };
    }
}
