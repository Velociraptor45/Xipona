using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;

public class OfflineTolerantItemId
{
    public OfflineTolerantItemId(Guid offlineId)
    {
        OfflineId = offlineId;
        ActualId = null;
    }

    public OfflineTolerantItemId(int actualId)
    {
        ActualId = actualId;
        OfflineId = null;
    }

    public int? ActualId { get; }
    public Guid? OfflineId { get; }

    public bool IsActualId => ActualId != null;
}