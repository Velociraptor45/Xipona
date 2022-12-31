using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items.Models
{
    public class ItemType
    {
        public ItemType(Guid id, string name, IEnumerable<ItemAvailability> availabilities)
        {
            Id = id;
            Name = name;
            Availabilities = availabilities.ToList();
        }

        public Guid Id { get; }
        public string Name { get; set; }
        public IReadOnlyCollection<ItemAvailability> Availabilities { get; }
    }
}