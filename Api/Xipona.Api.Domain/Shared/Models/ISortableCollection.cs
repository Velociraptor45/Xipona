using Xipona.Api.Domain.Common.Exceptions;
using Xipona.Api.Domain.Recipes.Reasons;

namespace Xipona.Api.Domain.Shared.Models;

public interface ISortableCollection<in T> where T : ISortable
{
    public void ValidateSortingIndexes(IEnumerable<T> models)
    {
        var sortingIndexes = models.Select(s => s.SortingIndex).ToLookup(si => si);
        foreach (var sortingIndex in sortingIndexes)
        {
            if (sortingIndex.Count() > 1)
                throw new DomainException(new DuplicatedSortingIndexReason(sortingIndex.Key));
        }
    }
}