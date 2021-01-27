using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Entities
{
    public class Section
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public int StoreId { get; set; }
        public int SortIndex { get; set; }

        public Store Store { get; set; }
        public ICollection<AvailableAt> DefaultItemsInSection { get; set; }
        public ICollection<ItemsOnList> ActualItemsSections { get; set; }
    }
}