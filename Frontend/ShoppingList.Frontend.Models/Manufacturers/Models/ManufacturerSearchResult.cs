using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models
{
    public class ManufacturerSearchResult : ISearchResult
    {
        public ManufacturerSearchResult(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}