using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Models
{
    public static class StoreExtensions
    {
        public static Infrastructure.Entities.Store ToEntity(this Store model)
        {
            return new Infrastructure.Entities.Store()
            {
                Id = model.Id.Value,
                Name = model.Name,
                Deleted = model.IsDeleted
            };
        }
    }
}