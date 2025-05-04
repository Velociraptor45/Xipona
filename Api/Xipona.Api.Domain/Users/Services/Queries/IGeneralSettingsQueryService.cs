using ProjectHermes.Xipona.Api.Domain.Users.Models;

namespace ProjectHermes.Xipona.Api.Domain.Users.Services.Queries;

public interface IGeneralSettingsQueryService
{
    IEnumerable<CurrencyReadModel> GetAllCurrencies();
    Task<IGeneralSetting> GetGeneralSettingsAsync();
}