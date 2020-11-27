using System;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.Models
{
    public class ShoppingListItemId
    {
        public ShoppingListItemId(int id)
        {
            Actual = new ShoppingListItemActualId(id);
            Offline = null;
        }

        public ShoppingListItemId(Guid id)
        {
            Offline = new ShoppingListItemOfflineId(id);
            Actual = null;
        }

        public ShoppingListItemId(ShoppingListItemActualId actual)
        {
            Actual = actual ?? throw new ArgumentNullException(nameof(actual));
            Offline = null;
        }

        public ShoppingListItemId(ShoppingListItemOfflineId offline)
        {
            Offline = offline ?? throw new ArgumentNullException(nameof(offline));
            Actual = null;
        }

        public ShoppingListItemActualId Actual { get; }
        public ShoppingListItemOfflineId Offline { get; }
        public bool IsActualId => Actual != null;

        public static bool operator ==(ShoppingListItemId left, ShoppingListItemId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ShoppingListItemId left, ShoppingListItemId right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return obj is ShoppingListItemId id &&
                   EqualityComparer<ShoppingListItemActualId>.Default.Equals(Actual, id.Actual) &&
                   EqualityComparer<ShoppingListItemOfflineId>.Default.Equals(Offline, id.Offline);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Actual, Offline);
        }

        public override string ToString()
        {
            return IsActualId ?
                Actual.Value.ToString() :
                Offline.Value.ToString();
        }
    }
}