using System;

namespace ShoppingList.Api.Contracts.Commands.Shared
{
    public class ItemIdContract
    {
        public int? Actual { get; set; }
        public Guid? Offline { get; set; }
    }
}