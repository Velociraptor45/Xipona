﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Entities;

public class ShoppingList
{
    public ShoppingList()
    {
        ItemsOnList ??= new List<ItemsOnList>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public DateTimeOffset? CompletionDate { get; set; }
    public Guid StoreId { get; set; }

    [InverseProperty("ShoppingList")]
    public ICollection<ItemsOnList> ItemsOnList { get; set; }
}