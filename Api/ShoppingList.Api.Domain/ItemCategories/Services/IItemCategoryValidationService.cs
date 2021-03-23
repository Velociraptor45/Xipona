using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services
{
    public interface IItemCategoryValidationService
    {
        Task Validate(ItemCategoryId itemCategoryId, CancellationToken cancellationToken);
    }
}