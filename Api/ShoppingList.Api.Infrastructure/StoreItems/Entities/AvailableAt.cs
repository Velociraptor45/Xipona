using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities
{
    public class AvailableAt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ItemId { get; set; }
        public int StoreId { get; set; }
        public float Price { get; set; }
        public int DefaultSectionId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        [ForeignKey("StoreId")]
        public Store Store { get; set; }

        [ForeignKey("DefaultSectionId")]
        public Section Section { get; set; }
    }
}