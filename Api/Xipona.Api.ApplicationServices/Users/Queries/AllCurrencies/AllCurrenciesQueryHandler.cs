using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Users.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Users.Queries.AllCurrencies;

public class AllCurrenciesQueryHandler : IQueryHandler<AllCurrenciesQuery, List<CurrencyReadModel>>
{
    private readonly Func<CancellationToken, IGeneralSettingsQueryService> _queryServiceDelegate;

    public AllCurrenciesQueryHandler(Func<CancellationToken, IGeneralSettingsQueryService> queryServiceDelegate)
    {
        _queryServiceDelegate = queryServiceDelegate;
    }

    public Task<List<CurrencyReadModel>> HandleAsync(AllCurrenciesQuery query, CancellationToken cancellationToken)
    {
        var queryService = _queryServiceDelegate(cancellationToken);
        var currencies = queryService.GetAllCurrencies().ToList();

        return Task.FromResult(currencies);
    }
}
