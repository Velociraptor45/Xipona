﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;

public class AvailableAt
{
    [Key]
    [Column(Order = 1)]
    public int ItemId { get; set; }

    [Key]
    [Column(Order = 2)]
    public int StoreId { get; set; }

    public float Price { get; set; }
    public int DefaultSectionId { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [ForeignKey("ItemId")]
    public Item Item { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}