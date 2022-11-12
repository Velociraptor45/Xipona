using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Services;
using System;
using System.Timers;

namespace ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models
{
    public class ShoppingListItem
    {
        public ShoppingListItem(ShoppingListItemId id, Guid? typeId, string name, bool isTemporary, float pricePerQuantity, QuantityType quantityType,
            float? quantityInPacket, QuantityTypeInPacket quantityInPacketType, string itemCategory,
            string manufacturer, bool isInBasket, float quantity)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            TypeId = typeId;
            Name = name;
            IsTemporary = isTemporary;
            PricePerQuantity = pricePerQuantity;
            QuantityType = quantityType ?? throw new ArgumentNullException(nameof(quantityType));
            QuantityInPacket = quantityInPacket;
            QuantityInPacketType = quantityInPacketType;
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            IsInBasket = isInBasket;
            Quantity = quantity;
            Hide = IsInBasket;
        }

        public ShoppingListItemId Id { get; }
        public Guid? TypeId { get; }
        public string Name { get; }
        public bool IsTemporary { get; }
        public float PricePerQuantity { get; }
        public QuantityType QuantityType { get; }
        public float? QuantityInPacket { get; }
        public QuantityTypeInPacket QuantityInPacketType { get; }
        public string ItemCategory { get; }
        public string Manufacturer { get; }
        public bool IsInBasket { get; set; }
        public float Quantity { get; private set; }
        public bool IsItemType => TypeId is not null;
        public bool Hide { get; private set; }

        private Timer _hideItemTimer;

        public void PutInBasket(Action onHide)
        {
            IsInBasket = true;
            if (_hideItemTimer is not null)
                _hideItemTimer.Stop();

            // this does not belong here but will be fixed in #298
            _hideItemTimer = new(1000d);
            _hideItemTimer.AutoReset = false;
            _hideItemTimer.Elapsed += (_, _) =>
            {
                Hide = true;
                onHide();
            };
            _hideItemTimer.Start();
        }

        public void RemoveFromBasket()
        {
            IsInBasket = false;
            if (_hideItemTimer is not null)
                _hideItemTimer.Stop();

            Hide = false;
        }

        public float GetTotalPrice(IItemPriceCalculationService priceCalculationService)
        {
            return priceCalculationService.CalculatePrice(QuantityType.Id, PricePerQuantity, Quantity);
        }

        public void ChangeQuantity(float difference)
        {
            Quantity += difference;
            CorrectQuantity();
        }

        public void SetQuantity(float quantity)
        {
            Quantity = quantity;
            CorrectQuantity();
        }

        private void CorrectQuantity()
        {
            if (Quantity <= 0)
                Quantity = 1;
        }
    }
}