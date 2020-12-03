using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItemId
    {
        public StoreItemId(int id)
        {
            Actual = new StoreItemActualId(id);
            Offline = null;
        }

        public StoreItemId(Guid id)
        {
            Offline = new StoreItemOfflineId(id);
            Actual = null;
        }

        public StoreItemId(StoreItemActualId actual)
        {
            Actual = actual ?? throw new ArgumentNullException(nameof(actual));
            Offline = null;
        }

        public StoreItemId(StoreItemOfflineId offline)
        {
            Offline = offline ?? throw new ArgumentNullException(nameof(offline));
            Actual = null;
        }

        public StoreItemId(int actualId, Guid offlineId)
        {
            Actual = new StoreItemActualId(actualId);
            Offline = new StoreItemOfflineId(offlineId);
        }

        public StoreItemId(StoreItemActualId actualId, StoreItemOfflineId offline)
        {
            Actual = actualId ?? throw new ArgumentNullException(nameof(actualId));
            Offline = offline ?? throw new ArgumentNullException(nameof(offline));
        }

        public StoreItemActualId Actual { get; }
        public StoreItemOfflineId Offline { get; }
        public bool IsActualId => Actual != null;

        public static bool operator ==(StoreItemId left, StoreItemId right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        public static bool operator !=(StoreItemId left, StoreItemId right)
        {
            if (left is null)
            {
                return !(right is null);
            }

            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return obj is StoreItemId storeId &&
                    EqualityComparer<StoreItemActualId>.Default.Equals(Actual, storeId.Actual) &&
                    EqualityComparer<StoreItemOfflineId>.Default.Equals(Offline, storeId.Offline);
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