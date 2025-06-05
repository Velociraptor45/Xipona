using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.Common.Attributes;
using Xipona.Api.Domain.Common.Models;
using Xipona.Api.Domain.Users.Models;
using Xipona.Api.Domain.Users.Ports;

namespace Xipona.Api.Domain.Users.Services.Queries;

public class GeneralSettingsQueryService : IGeneralSettingsQueryService
{
    private readonly IGeneralSettingRepository _repository;

    public GeneralSettingsQueryService(IGeneralSettingRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<CurrencyReadModel> GetAllCurrencies()
    {
        var values = Enum.GetValues<Currency>().ToList();

        return values.Select(v => new CurrencyReadModel(v.ToInt(), v.GetAttribute<CurrencySymbolAttribute>().Symbol));
    }

    public async Task<IGeneralSetting> GetGeneralSettingsAsync()
    {
        var settings = await _repository.GetAsync();
        return settings;
    }
}
