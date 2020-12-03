using System;

namespace ShoppingList.Frontend.Models.Shared
{
    public class ItemId
    {
        public ItemId(int actualId)
        {
            ActualId = actualId;
            OfflineId = null;
        }

        public ItemId(Guid offlineId)
        {
            OfflineId = offlineId;
            ActualId = null;
        }

        public int? ActualId { get; }
        public Guid? OfflineId { get; }
    }
}