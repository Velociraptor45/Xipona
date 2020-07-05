using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Database.Entities;

namespace ShoppingList.Database
{
    public class ShoppingRepositoryFactory : IShoppingRepositoryFactory
    {
        private readonly IMapper mapper;
        private readonly DbContextOptions<ShoppingContext> dbContextOptions;

        public ShoppingRepositoryFactory(DbContextOptions<ShoppingContext> contextOptions, IMapper mapper)
        {
            this.mapper = mapper;
            this.dbContextOptions = contextOptions;
        }

        public IShoppingRepository Create()
        {
            return new ShoppingRepository(dbContextOptions, mapper);
        }
    }
}
