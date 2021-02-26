using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Models
{
    public static class StoreExtensions
    {
        public static Infrastructure.Entities.Store ToEntity(this IStore model)
        {
            return new Infrastructure.Entities.Store()
            {
                Id = model.Id.Value,
                Name = model.Name,
                Deleted = model.IsDeleted,
                DefaultSectionId = model.Sections.Single(s => s.IsDefaultSection).Id.Value,
                Sections = model.Sections.Select(s => s.ToEntity()).ToList()
            };
        }
    }
}