namespace ShoppingList.Api.Infrastructure.Extensions.Models
{
    public static class StoreExtensions
    {
        public static Infrastructure.Entities.Store ToEntity(this Domain.Models.Store model)
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