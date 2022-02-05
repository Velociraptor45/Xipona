using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;

public class Item
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public Item()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        AvailableAt ??= new List<AvailableAt>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public bool Deleted { get; set; }
    public string Comment { get; set; }
    public bool IsTemporary { get; set; }
    public int QuantityType { get; set; }
    public float QuantityInPacket { get; set; }
    public int QuantityTypeInPacket { get; set; }
    public int? ItemCategoryId { get; set; }
    public int? ManufacturerId { get; set; }
    public Guid? CreatedFrom { get; set; }
    public int? PredecessorId { get; set; }

    [ForeignKey("PredecessorId")]
    public Item? Predecessor { get; set; }

    [InverseProperty("Item")]
    public ICollection<ItemType> ItemTypes { get; set; }

    [InverseProperty("Item")]
    public ICollection<AvailableAt> AvailableAt { get; set; }
}