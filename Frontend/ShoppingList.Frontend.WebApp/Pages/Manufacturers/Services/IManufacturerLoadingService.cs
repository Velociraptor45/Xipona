using ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Manufacturers.Services
{
    public interface IManufacturerLoadingService
    {
        Task<IEnumerable<ManufacturerSearchResult>> SearchAsync(string searchInput);
        Task<Manufacturer> GetAsync(Guid manufacturerId);
    }
}