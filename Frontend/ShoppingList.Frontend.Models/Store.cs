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

        public void ChangeDefaultSection(int sectionId)
        {
            var section = Sections.FirstOrDefault(s => s.Id == sectionId);
            if (section == null)
                return;

            Sections.ForEach(s => s.SetAsDefaultSection(false));
            section.SetAsDefaultSection(true);
        }
    }
}