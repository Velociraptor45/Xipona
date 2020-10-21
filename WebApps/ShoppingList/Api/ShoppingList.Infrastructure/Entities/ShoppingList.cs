using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingList.Infrastructure.Entities
{
    public class ShoppingList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? CompletionDate { get; set; }
        public int StoreId { get; set; }

        public Store Store { get; set; }
        public ICollection<ItemsOnList> ItemsOnList { get; set; }
    }
}