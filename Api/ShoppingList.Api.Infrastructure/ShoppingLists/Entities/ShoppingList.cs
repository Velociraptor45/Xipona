using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;

public class ShoppingList
{
    public ShoppingList()
    {
        ItemsOnList ??= new List<ItemsOnList>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTime? CompletionDate { get; set; }
    public int StoreId { get; set; }

    [InverseProperty("ShoppingList")]
    public ICollection<ItemsOnList> ItemsOnList { get; set; }
}