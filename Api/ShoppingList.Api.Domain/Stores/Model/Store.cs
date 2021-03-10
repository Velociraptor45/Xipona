using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Model
{
    public class Store : IStore
    {
        private readonly List<IStoreSection> sections;

        public Store(StoreId id, string name, bool isDeleted, IEnumerable<IStoreSection> sections)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
            this.sections = sections.ToList();
        }

        public StoreId Id { get; }
        public string Name { get; private set; }
        public bool IsDeleted { get; }
        public IReadOnlyCollection<IStoreSection> Sections => sections.AsReadOnly();

        public void ChangeName(string name)
        {
            Name = name;
        }

        public void UpdateStores(IEnumerable<IStoreSection> storeSections)
        {
            sections.Clear();
            sections.AddRange(storeSections);
        }
    }
}