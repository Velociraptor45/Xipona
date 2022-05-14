using ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models;
using System;
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

        public void RemoveFromSearchResultsIfExists(Guid manufacturerId)
        {
            var manufacturer = _searchResults.FirstOrDefault(r => r.Id == manufacturerId);
            if (manufacturer is null)
                return;

            _searchResults.Remove(manufacturer);
        }

        public void UpdateManufacturerSearchResultName(Guid manufacturerId, string name)
        {
            var manufacturer = _searchResults.FirstOrDefault(r => r.Id == manufacturerId);
            if (manufacturer is null)
                return;

            manufacturer.ChangeName(name);
        }

        public void SetEditedManufacturer(Manufacturer manufacturer)
        {
            EditedManufacturer = manufacturer;
        }

        public void SetNewEditedManufacturer()
        {
            SetEditedManufacturer(new Manufacturer(Guid.Empty, "New Manufacturer"));
        }

        public void ResetEditedManufacturer()
        {
            SetEditedManufacturer(null);
        }
    }
}