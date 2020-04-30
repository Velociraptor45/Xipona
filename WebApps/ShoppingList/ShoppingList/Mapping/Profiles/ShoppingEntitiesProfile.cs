using AutoMapper;
using ShoppingList.Database.Entities;
using ShoppingList.EntityModels.DataTransfer;

namespace ShoppingList.Mapping.Profiles
{
    public class ShoppingEntitiesProfile : Profile
    {
        public ShoppingEntitiesProfile()
        {
            CreateMap(typeof(Store), typeof(StoreDto));
            CreateMap(typeof(StoreDto), typeof(Store));
        }
    }
}
