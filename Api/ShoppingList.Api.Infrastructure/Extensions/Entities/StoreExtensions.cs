using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using System.Linq;
using CommonModels = ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Entities
{
    public static class StoreExtensions
    {
        public static IStore ToDomain(this Infrastructure.Entities.Store entity)
        {
            return new Store(
                new StoreId(entity.Id),
                entity.Name,
                entity.Deleted,
                entity.Sections
                    .Select(section => new StoreSection(
                        new StoreSectionId(section.Id),
                        section.Name,
                        section.SortIndex,
                        section.Id == entity.DefaultSectionId)));
        }

        public static CommonModels.IStore ToCommonDomain(this Infrastructure.Entities.Store entity)
        {
            return new CommonModels.Store(
                new CommonModels.StoreId(entity.Id),
                entity.Name,
                entity.Deleted);
        }
    }
}