using Xipona.Api.ApplicationServices.Common.Queries;

namespace Xipona.Api.ApplicationServices.Items.Queries.TotalSearchResultCounts;

public record TotalSearchResultCountQuery(string SearchInput) : IQuery<int>;