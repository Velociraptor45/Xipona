using ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Manufacturers.Models
{
    public class ManufacturersState
    {
        private List<ManufacturerSearchResult> _searchResults;

        public ManufacturersState()
        {
            _searchResults = new List<ManufacturerSearchResult>();
        }

        public IReadOnlyCollection<ManufacturerSearchResult> SearchResults => _searchResults;
        public Manufacturer EditedManufacturer { get; set; }

        public void RegisterSearchResults(IEnumerable<ManufacturerSearchResult> searchResults)
        {
            _searchResults = searchResults.ToList();
        }

        public void SetEditedManufacturer(Manufacturer manufacturer)
        {
            EditedManufacturer = manufacturer;
        }

        public void ResetEditedManufacturer()
        {
            SetEditedManufacturer(null);
        }
    }
}