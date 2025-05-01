namespace ProjectHermes.Xipona.Api.Domain.Users.Services.Queries;

public interface IGeneralSettingsQueryService
{
    IEnumerable<CurrencyReadModel> GetAllCurrencies();
}