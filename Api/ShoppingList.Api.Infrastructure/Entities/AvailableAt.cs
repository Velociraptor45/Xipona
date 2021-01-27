using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Entities
{
    public class AvailableAt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ItemId { get; set; }
        public int StoreId { get; set; }
        public float Price { get; set; }
        public int? DefaultSectionId { get; set; }

        public Item Item { get; set; }
        public Store Store { get; set; }
        public Section Section { get; set; }
    }
}