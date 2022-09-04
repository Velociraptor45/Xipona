using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models
{
    public class ManufacturerSearchResult
    {
        public ManufacturerSearchResult(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public void ChangeName(string name)
        {
            Name = name;
        }
    }
}