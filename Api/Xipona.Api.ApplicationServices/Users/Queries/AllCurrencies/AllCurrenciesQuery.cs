using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Users.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Users.Queries.AllCurrencies;

public record AllCurrenciesQuery : IQuery<List<CurrencyReadModel>>;
