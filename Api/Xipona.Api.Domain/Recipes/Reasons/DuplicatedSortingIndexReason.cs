using ProjectHermes.Xipona.Api.Domain.Common.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Reasons;

public class DuplicatedSortingIndexReason : IReason
{
    public DuplicatedSortingIndexReason(int sortingIndex)
    {
        Message = $"Sorting index {sortingIndex} is duplicated";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.DuplicatedSortingIndex;
}