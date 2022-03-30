using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared
{
    public class ItemIdContract
    {
        private ItemIdContract(Guid? actual, Guid? offline)
        {
            Actual = actual;
            Offline = offline;
        }

        public Guid? Actual { get; set; }
        public Guid? Offline { get; set; }

        public ItemIdContract FromActualId(Guid? actualId)
        {
            return new ItemIdContract(actualId, null);
        }

        public ItemIdContract FromOfflineId(Guid? offlineId)
        {
            return new ItemIdContract(null, offlineId);
        }
    }
}