using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities
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
        public int? SectionId { get; set; }

        [ForeignKey("ShoppingListId")]
        public ShoppingList ShoppingList { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        [ForeignKey("SectionId")]
        public Section Section { get; set; }
    }
}