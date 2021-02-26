using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items
{
    public class StoreItemStore
    {
        public StoreItemStore(int id, string name, IEnumerable<StoreItemSection> sections)
        {
            Id = id;
            Name = name;
            Sections = sections.ToList().AsReadOnly();
        }

        public int Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<StoreItemSection> Sections { get; }
    }
}