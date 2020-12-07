using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Entities
{
    public class ItemsOnList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ShoppingListId { get; set; }
        public int ItemId { get; set; }
        public bool InBasket { get; set; }
        public float Quantity { get; set; }

        public ShoppingList ShoppingList { get; set; }
        public Item Item { get; set; }
    }
}