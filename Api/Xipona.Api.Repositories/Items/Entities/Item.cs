﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.Xipona.Api.Repositories.Items.Entities;

public class Item
{
    public Item()
    {
        AvailableAt ??= new List<AvailableAt>();
        Name = string.Empty;
        Comment = string.Empty;
        ItemTypes ??= new List<ItemType>();
        RowVersion = Array.Empty<byte>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    public bool Deleted { get; set; }
    public string Comment { get; set; }
    public bool IsTemporary { get; set; }
    public int QuantityType { get; set; }
    public float? QuantityInPacket { get; set; }
    public int? QuantityTypeInPacket { get; set; }
    public Guid? ItemCategoryId { get; set; }
    public Guid? ManufacturerId { get; set; }
    public Guid? CreatedFrom { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public Guid? PredecessorId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }

    [ForeignKey("PredecessorId")]
    public Item? Predecessor { get; set; }

    [InverseProperty("Item")]
    public ICollection<ItemType> ItemTypes { get; set; }

    [InverseProperty("Item")]
    public ICollection<AvailableAt> AvailableAt { get; set; }
}