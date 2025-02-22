using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.Shared
{
    /// <summary>
    /// Represents an item ID.
    /// </summary>
    public class ItemIdContract
    {
        /// <summary>
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="offline"></param>
        public ItemIdContract(Guid? actual, Guid? offline)
        {
            Actual = actual;
            Offline = offline;
        }

        /// <summary>
        /// The actual, normal ID of the item.
        /// </summary>
        public Guid? Actual { get; set; }

        /// <summary>
        /// The ID with which a temporary item can be identified by a client without having to
        /// wait for the api request to be successful (e.g. when offline).
        /// </summary>
        public Guid? Offline { get; set; }

        /// <summary>
        /// Creates an <see cref="ItemIdContract"/> only with an <see cref="Actual"/> and no <see cref="Offline"/>.
        /// </summary>
        /// <param name="actualId"></param>
        /// <returns></returns>
        public static ItemIdContract FromActualId(Guid? actualId)
        {
            return new ItemIdContract(actualId, null);
        }

        /// <summary>
        /// Creates an <see cref="ItemIdContract"/> only with an <see cref="Offline"/> and no <see cref="Actual"/>.
        /// </summary>
        /// <param name="offlineId"></param>
        /// <returns></returns>
        public static ItemIdContract FromOfflineId(Guid? offlineId)
        {
            return new ItemIdContract(null, offlineId);
        }
    }
}