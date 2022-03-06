using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.Shared
{
    public class StoreSectionContract
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int SortingIndex { get; set; }
        public bool IsDefaultSection { get; set; }
    }
}