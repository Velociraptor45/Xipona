using ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool Deleted { get; set; }
        public string Comment { get; set; }
        public bool IsTemporary { get; set; }
        public int QuantityType { get; set; }
        public float QuantityInPacket { get; set; }
        public int QuantityTypeInPacket { get; set; }
        public int? ItemCategoryId { get; set; }
        public int? ManufacturerId { get; set; }
        public Guid? CreatedFrom { get; set; }
        public int? PredecessorId { get; set; }

        [ForeignKey("ManufacturerId")]
        public Manufacturer Manufacturer { get; set; }

        [ForeignKey("ItemCategoryId")]
        public ItemCategory ItemCategory { get; set; }

        [ForeignKey("PredecessorId")]
        public Item Predecessor { get; set; }

        [InverseProperty("Item")]
        public ICollection<AvailableAt> AvailableAt { get; set; }
    }
}