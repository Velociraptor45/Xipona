using ProjectHermes.ShoppingList.Frontend.Models.Stores.Comparer;
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
            Sections = new SortedSet<StoreSection>(sections, new SortingIndexComparer());
        }

        public int Id { get; }
        public string Name { get; set; }
        public SortedSet<StoreSection> Sections { get; private set; }
        public int MaxSortingIndex => Sections.Max(s => s.SortingIndex);
        public int MinSortingIndex => Sections.Min(s => s.SortingIndex);

        public void ChangeDefaultSection(int sectionId)
        {
            var section = Sections.FirstOrDefault(s => s.Id == sectionId);
            if (section == null)
                return;

            Sections.ToList().ForEach(s => s.SetAsDefaultSection(false));
            section.SetAsDefaultSection(true);
        }

        public void AddSection()
        {
            var section = new StoreSection(0, "", Sections.Max(s => s.SortingIndex) + 1, false);
            Sections.Add(section);
        }

        public void IncrementSection(StoreSection section)
        {
            var sections = Sections.ToList();

            int sectionIndex = sections.IndexOf(section);
            if (sectionIndex == -1 || sectionIndex >= sections.Count - 1)
                return;

            var tmp = sections[sectionIndex + 1];
            sections[sectionIndex + 1] = section;
            sections[sectionIndex] = tmp;

            UpdateSortingIndexes(sections);

            Sections = new SortedSet<StoreSection>(sections, new SortingIndexComparer());
        }

        public void DecrementSection(StoreSection section)
        {
            var sections = Sections.ToList();

            int sectionIndex = sections.IndexOf(section);
            if (sectionIndex == -1 || sectionIndex <= 0)
                return;

            var tmp = sections[sectionIndex - 1];
            sections[sectionIndex - 1] = section;
            sections[sectionIndex] = tmp;

            UpdateSortingIndexes(sections);

            Sections = new SortedSet<StoreSection>(sections, new SortingIndexComparer());
        }

        private void UpdateSortingIndexes(List<StoreSection> sections)
        {
            for (int i = 0; i < sections.Count; i++)
            {
                sections[i].SetSortingIndex(i);
            }
        }
    }
}