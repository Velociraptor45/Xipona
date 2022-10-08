using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Comparer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Stores.Models
{
    public class Store : ISortable<Section>
    {
        public Store(Guid id, string name, IEnumerable<Section> sections)
        {
            Id = id;
            Name = name;
            Sections = new SortedSet<Section>(sections, new SortingIndexComparer());
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public SortedSet<Section> Sections { get; private set; }
        public int MaxSortingIndex => Sections.Max(s => s.SortingIndex);
        public int MinSortingIndex => Sections.Min(s => s.SortingIndex);
        public Section DefaultSection => Sections.Single(s => s.IsDefaultSection);

        public void ChangeDefaultSection(Guid sectionId)
        {
            var section = Sections.FirstOrDefault(s => s.Id.FrontendId == sectionId);
            if (section == null)
                return;

            Sections.ToList().ForEach(s => s.SetAsDefaultSection(false));
            section.SetAsDefaultSection(true);
        }

        public void AddSection()
        {
            var section = new Section(
                new SectionId(Guid.NewGuid()),
                name: "New Section",
                Sections.Max(s => s.SortingIndex) + 1,
                false);
            Sections.Add(section);
        }

        public void Increment(Section section)
        {
            var sections = Sections.ToList();

            int sectionIndex = sections.IndexOf(section);
            if (sectionIndex == -1 || sectionIndex >= sections.Count - 1)
                return;

            var tmp = sections[sectionIndex + 1];
            sections[sectionIndex + 1] = section;
            sections[sectionIndex] = tmp;

            UpdateSortingIndexes(sections);

            Sections = new SortedSet<Section>(sections, new SortingIndexComparer());
        }

        public void Decrement(Section section)
        {
            var sections = Sections.ToList();

            int sectionIndex = sections.IndexOf(section);
            if (sectionIndex is -1 or <= 0)
                return;

            var tmp = sections[sectionIndex - 1];
            sections[sectionIndex - 1] = section;
            sections[sectionIndex] = tmp;

            UpdateSortingIndexes(sections);

            Sections = new SortedSet<Section>(sections, new SortingIndexComparer());
        }

        private static void UpdateSortingIndexes(List<Section> sections)
        {
            for (int i = 0; i < sections.Count; i++)
            {
                sections[i].SetSortingIndex(i);
            }
        }

        public void Remove(Section model)
        {
            Sections.Remove(model);
        }

        public ItemStore AsItemStore()
        {
            return new ItemStore(Id, Name, Sections.Select(s => s.AsItemSection()));
        }
    }
}