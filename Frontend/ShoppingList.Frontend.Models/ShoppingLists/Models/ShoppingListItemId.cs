using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models
{
    public sealed class ShoppingListItemId : IEquatable<ShoppingListItemId>
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
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((ShoppingListItemId)obj);
        }

        public bool Equals(ShoppingListItemId other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Nullable.Equals(ActualId, other.ActualId) && Nullable.Equals(OfflineId, other.OfflineId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ActualId, OfflineId);
        }
    }
}