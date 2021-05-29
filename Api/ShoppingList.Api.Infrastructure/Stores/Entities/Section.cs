using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities
{
    public class Section
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int StoreId { get; set; }
        public int SortIndex { get; set; }
        public bool IsDefaultSection { get; set; }

        [ForeignKey("StoreId")]
        public Store Store { get; set; }

        [InverseProperty("Section")]
        public ICollection<AvailableAt> DefaultItemsInSection { get; set; }

        [InverseProperty("Section")]
        public ICollection<ItemsOnList> ActualItemsSections { get; set; }
    }
}