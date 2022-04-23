using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore
{
    public class UpdateSectionContract
    {
        public UpdateSectionContract(Guid? id, string name, int sortingIndex, bool isDefaultSection)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
        }

        public Guid? Id { get; set; }
        public string Name { get; set; }
        public int SortingIndex { get; set; }
        public bool IsDefaultSection { get; set; }
    }
}