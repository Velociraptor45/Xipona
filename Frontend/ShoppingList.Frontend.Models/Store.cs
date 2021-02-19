using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class Store
    {
        public Store(int id, string name, IEnumerable<StoreSection> sections)
        {
            Id = id;
            Name = name;
            Sections = sections.ToList();
        }

        public int Id { get; }
        public string Name { get; set; }
        public List<StoreSection> Sections { get; }
    }
}