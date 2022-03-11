using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared
{
    public class ItemId
    {
        public Guid? ActualId { get; }
        public Guid? OfflineId { get; }

        private ItemId(Guid? offlineId, Guid? actualId)
        {
            OfflineId = offlineId;
            ActualId = actualId;
        }

        public static ItemId FromOfflineId(Guid offlineId)
        {
            return new(offlineId, null);
        }

        public static ItemId FromActualId(Guid actualId)
        {
            return new(null, actualId);
        }

        public static bool operator ==(ItemId left, ItemId right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        public static bool operator !=(ItemId left, ItemId right)
        {
            if (left is null)
            {
                return !(right is null);
            }

            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return obj is ItemId primitive
                   && EqualityComparer<Guid?>.Default.Equals(ActualId, primitive.ActualId)
                   && EqualityComparer<Guid?>.Default.Equals(OfflineId, primitive.OfflineId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ActualId, OfflineId);
        }
    }
}