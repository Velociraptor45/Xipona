namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;

public class OfflineTolerantItemId
{
    private OfflineTolerantItemId(Guid? offlineId, Guid? actualId)
    {
        OfflineId = offlineId;
        ActualId = actualId;
    }

    public static OfflineTolerantItemId FromOfflineId(Guid offlineId)
    {
        return new(offlineId, null);
    }

    public static OfflineTolerantItemId FromActualId(Guid actualId)
    {
        return new(null, actualId);
    }

    public Guid? ActualId { get; }
    public Guid? OfflineId { get; }

    public bool IsActualId => ActualId != null;
}