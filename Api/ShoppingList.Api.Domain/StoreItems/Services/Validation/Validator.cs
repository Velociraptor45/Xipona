using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validation
{
    public class Validator : IValidator
    {
        private readonly IAvailabilityValidationService _availabilityValidationService;
        private readonly IItemCategoryValidationService _itemCategoryValidationService;
        private readonly IManufacturerValidationService _manufacturerValidationService;
        private readonly CancellationToken _cancellationToken;

        public Validator(IAvailabilityValidationService availabilityValidationService,
            IItemCategoryValidationService itemCategoryValidationService,
            IManufacturerValidationService manufacturerValidationService,
            CancellationToken cancellationToken)
        {
            _availabilityValidationService = availabilityValidationService;
            _itemCategoryValidationService = itemCategoryValidationService;
            _manufacturerValidationService = manufacturerValidationService;
            _cancellationToken = cancellationToken;
        }

        public async Task ValidateAsync(IEnumerable<IStoreItemAvailability> availabilities)
        {
            if (availabilities is null)
                throw new ArgumentNullException(nameof(availabilities));

            await _availabilityValidationService.ValidateAsync(availabilities, _cancellationToken);
        }

        public async Task ValidateAsync(ItemCategoryId itemCategoryId)
        {
            if (itemCategoryId is null)
                throw new ArgumentNullException(nameof(itemCategoryId));

            await _itemCategoryValidationService.ValidateAsync(itemCategoryId, _cancellationToken);
        }

        public async Task ValidateAsync(ManufacturerId manufacturerId)
        {
            if (manufacturerId is null)
                throw new ArgumentNullException(nameof(manufacturerId));

            await _manufacturerValidationService.ValidateAsync(manufacturerId, _cancellationToken);
        }
    }
}