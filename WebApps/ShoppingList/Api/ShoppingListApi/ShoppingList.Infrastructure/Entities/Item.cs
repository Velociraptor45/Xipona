using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingList.Infrastructure.Entities
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public bool Deleted { get; set; }
        public string Comment { get; set; }
        public bool IsTemporary { get; set; }
        public int QuantityType { get; set; }
        public float QuantityInPacket { get; set; }
        public int QuantityTypeInPacket { get; set; }
        public int ItemCategoryId { get; set; }
        public int ManufacturerId { get; set; }

        public Manufacturer Manufacturer { get; set; }
        public ItemCategory ItemCategory { get; set; }
        public ICollection<AvailableAt> AvailableAt { get; set; }
    }
}