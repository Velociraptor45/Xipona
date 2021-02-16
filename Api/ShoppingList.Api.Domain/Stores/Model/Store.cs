using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Model
{
    public class Store : IStore
    {
        private readonly Dictionary<StoreSectionId, IStoreSection> sections;

        public Store(StoreId id, string name, bool isDeleted, IEnumerable<IStoreSection> sections)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
            this.sections = sections.ToDictionary(s => s.Id);
        }

        public StoreId Id { get; }
        public string Name { get; private set; }
        public bool IsDeleted { get; }
        public IReadOnlyCollection<IStoreSection> Sections => sections.Values;

        public void ChangeName(string name)
        {
            Name = name;
        }

        public void UpdateStores(IEnumerable<IStoreSection> storeSections)
        {
            sections.Clear();
            foreach (var section in storeSections.ToList())
            {
                sections.Add(section.Id, section);
            }
        }
    }
}