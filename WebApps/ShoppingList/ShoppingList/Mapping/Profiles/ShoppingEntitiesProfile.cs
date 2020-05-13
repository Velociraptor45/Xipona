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

            CreateMap(typeof(ItemCategoryDto), typeof(ItemCategory));
            CreateMap(typeof(ItemCategory), typeof(ItemCategoryDto));

            CreateMap(typeof(Manufacturer), typeof(ManufacturerDto));
            CreateMap(typeof(ManufacturerDto), typeof(Manufacturer));
        }
    }
}
