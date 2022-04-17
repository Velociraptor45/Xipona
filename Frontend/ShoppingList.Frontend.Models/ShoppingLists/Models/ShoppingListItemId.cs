using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models
{
    public class ShoppingListItemId
    {
        public Guid? ActualId { get; }
        public Guid? OfflineId { get; }

        private ShoppingListItemId(Guid? offlineId, Guid? actualId)
        {
            OfflineId = offlineId;
            ActualId = actualId;
        }

        public static ShoppingListItemId FromOfflineId(Guid offlineId)
        {
            return new(offlineId, null);
        }

        public static ShoppingListItemId FromActualId(Guid actualId)
        {
            return new(null, actualId);
        }

        public static bool operator ==(ShoppingListItemId left, ShoppingListItemId right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        public static bool operator !=(ShoppingListItemId left, ShoppingListItemId right)
        {
            if (left is null)
            {
                return right is not null;
            }

            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return obj is ShoppingListItemId primitive
                   && EqualityComparer<Guid?>.Default.Equals(ActualId, primitive.ActualId)
                   && EqualityComparer<Guid?>.Default.Equals(OfflineId, primitive.OfflineId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ActualId, OfflineId);
        }
    }
}