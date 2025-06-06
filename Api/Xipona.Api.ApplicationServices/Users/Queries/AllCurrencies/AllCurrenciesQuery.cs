using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Users.Services.Queries;

namespace Xipona.Api.ApplicationServices.Users.Queries.AllCurrencies;

public record AllCurrenciesQuery : IQuery<List<CurrencyReadModel>>;
