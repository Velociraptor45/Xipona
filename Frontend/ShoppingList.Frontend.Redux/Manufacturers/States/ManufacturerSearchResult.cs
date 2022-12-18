using ProjectHermes.ShoppingList.Frontend.Models.Shared;

namespace ShoppingList.Frontend.Redux.Manufacturers.States
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