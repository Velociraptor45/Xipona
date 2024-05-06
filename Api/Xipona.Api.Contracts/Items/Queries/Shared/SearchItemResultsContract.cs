using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.Shared
{
    public class SearchItemResultsContract
    {
        public SearchItemResultsContract(IEnumerable<SearchItemResultContract> searchResults, int totalResulCount)
        {
            SearchResults = searchResults;
            TotalResulCount = totalResulCount;
        }

        public IEnumerable<SearchItemResultContract> SearchResults { get; set; }
        public int TotalResulCount { get; set; }
    }
}