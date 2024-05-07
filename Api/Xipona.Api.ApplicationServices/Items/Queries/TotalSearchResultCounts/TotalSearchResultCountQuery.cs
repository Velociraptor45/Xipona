using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.TotalSearchResultCounts;

public record TotalSearchResultCountQuery(string SearchInput) : IQuery<int>;