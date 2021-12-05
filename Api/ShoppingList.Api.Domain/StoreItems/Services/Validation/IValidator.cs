using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validation
{
    public interface IValidator
    {
        Task ValidateAsync(IEnumerable<IStoreItemAvailability> availabilities);

        Task ValidateAsync(ItemCategoryId itemCategoryId);

        Task ValidateAsync(ManufacturerId manufacturerId);
    }
}