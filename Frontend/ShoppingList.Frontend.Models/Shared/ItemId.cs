using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared
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
                   && EqualityComparer<int?>.Default.Equals(ActualId, primitive.ActualId)
                   && EqualityComparer<Guid?>.Default.Equals(OfflineId, primitive.OfflineId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ActualId, OfflineId);
        }
    }
}