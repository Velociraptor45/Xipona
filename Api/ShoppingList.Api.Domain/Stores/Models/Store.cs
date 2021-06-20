using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models
{
    public class Store : IStore
    {
        private StoreSections sections;

        public Store(StoreId id, string name, bool isDeleted, IEnumerable<IStoreSection> sections)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
            this.sections = new StoreSections(sections);
        }

        public StoreId Id { get; }
        public string Name { get; private set; }
        public bool IsDeleted { get; }
        public IReadOnlyCollection<IStoreSection> Sections => sections.AsReadOnly();

        public IStoreSection GetDefaultSection()
        {
            return sections.GetDefaultSection();
        }

        public bool ContainsSection(SectionId sectionId)
        {
            return sections.Contains(sectionId);
        }

        public void ChangeName(string name)
        {
            Name = name;
        }

        public void UpdateStores(IEnumerable<IStoreSection> storeSections)
        {
            sections = new StoreSections(storeSections);
        }
    }
}