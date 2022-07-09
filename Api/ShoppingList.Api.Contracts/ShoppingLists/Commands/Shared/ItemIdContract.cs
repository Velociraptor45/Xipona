using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.Shared
{
    public class ItemIdContract
    {
        public ItemIdContract(Guid? actual, Guid? offline)
        {
            Actual = actual;
            Offline = offline;
        }

        public Guid? Actual { get; set; }
        public Guid? Offline { get; set; }

        public static ItemIdContract FromActualId(Guid? actualId)
        {
            return new ItemIdContract(actualId, null);
        }

        public static ItemIdContract FromOfflineId(Guid? offlineId)
        {
            return new ItemIdContract(null, offlineId);
        }
    }
}