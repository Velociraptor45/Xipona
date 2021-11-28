using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities
{
    public class ItemType
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public ItemType()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            AvailableAt ??= new List<ItemTypeAvailableAt>();
        }

        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        [InverseProperty("ItemType")]
        public ICollection<ItemTypeAvailableAt> AvailableAt { get; set; }
    }
}