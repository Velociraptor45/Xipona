namespace ShoppingList.Api.Infrastructure.Extensions.Entities
{
    public static class StoreExtensions
    {
        public static Domain.Models.Store ToDomain(this Infrastructure.Entities.Store entity)
        {
            return new Domain.Models.Store(
                new Domain.Models.StoreId(entity.Id),
                entity.Name,
                entity.Deleted);
        }
    }
}