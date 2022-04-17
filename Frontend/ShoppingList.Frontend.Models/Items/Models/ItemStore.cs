using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items.Models
{
    public class ItemStore
    {
        public ItemStore(Guid id, string name, IEnumerable<ItemSection> sections)
        {
            Id = id;
            Name = name;
            Sections = sections.ToList().AsReadOnly();
        }

        public Guid Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<ItemSection> Sections { get; }
    }
}