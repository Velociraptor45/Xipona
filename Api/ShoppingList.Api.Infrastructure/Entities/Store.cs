using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Entities
{
    public class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool Deleted { get; set; }
        public int DefaultSectionId { get; set; }

        [ForeignKey("DefaultSectionId")]
        public Section DefaultSection { get; set; }

        public ICollection<AvailableAt> AvailableItems { get; set; }
        public ICollection<Section> Sections { get; set; }
    }
}