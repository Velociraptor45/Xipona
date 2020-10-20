using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingList.Infrastructure.Entities
{
    public class AvailableAt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ItemId { get; set; }
        public int ShopId { get; set; }
        public float Price { get; set; }

        public Item Item { get; set; }
        public Store Store { get; set; }
    }
}