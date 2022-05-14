using ProjectHermes.ShoppingList.Api.Contracts.Manufacturers.Commands;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.Manufacturers;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Manufacturers.ToContract
{
    public class ModifyManufacturerContractConverter :
        IToContractConverter<ModifyManufacturerRequest, ModifyManufacturerContract>
    {
        public ModifyManufacturerContract ToContract(ModifyManufacturerRequest source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new ModifyManufacturerContract(source.ManufacturerId, source.Name);
        }
    }
}