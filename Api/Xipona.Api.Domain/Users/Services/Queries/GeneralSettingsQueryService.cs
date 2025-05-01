using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Common.Attributes;
using ProjectHermes.Xipona.Api.Domain.Common.Models;

namespace ProjectHermes.Xipona.Api.Domain.Users.Services.Queries;

public class GeneralSettingsQueryService : IGeneralSettingsQueryService
{
    public IEnumerable<CurrencyReadModel> GetAllCurrencies()
    {
        var values = Enum.GetValues<Currency>().ToList();

        return values.Select(v => new CurrencyReadModel(v.ToInt(), v.GetAttribute<CurrencySymbolAttribute>().Symbol));
    }
}
