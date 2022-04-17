using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models
{
    public class Manufacturer
    {
        public Manufacturer(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}