namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;

public record SearchItemResultsReadModel(IEnumerable<SearchItemResultReadModel> SearchResults, int TotalResultCount);