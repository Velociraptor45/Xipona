using Xipona.Api.Core.Converter;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.Common.Models;
using Xipona.Api.Domain.Users.Models;
using GeneralSetting = Xipona.Api.Repositories.Users.Entities.GeneralSetting;

namespace Xipona.Api.Repositories.Users.Converters.ToContract;

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
