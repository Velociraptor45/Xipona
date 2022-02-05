﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;

public class ItemsOnList
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int ShoppingListId { get; set; }
    public int ItemId { get; set; }
    public int? ItemTypeId { get; set; }
    public bool InBasket { get; set; }
    public float Quantity { get; set; }
    public int SectionId { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [ForeignKey("ShoppingListId")]
    public ShoppingList ShoppingList { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}