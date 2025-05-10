using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Common.Attributes;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.Users.Models;
using ProjectHermes.Xipona.Api.Domain.Users.Ports;

namespace ProjectHermes.Xipona.Api.Domain.Users.Services.Queries;

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
