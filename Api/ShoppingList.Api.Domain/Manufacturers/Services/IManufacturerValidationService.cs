using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services
{
    public interface IManufacturerValidationService
    {
        Task ValidateAsync(ManufacturerId manufacturerId, CancellationToken cancellationToken);
    }
}