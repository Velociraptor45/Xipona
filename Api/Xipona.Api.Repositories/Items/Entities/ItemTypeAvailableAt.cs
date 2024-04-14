using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.Xipona.Api.Repositories.Items.Entities;

public class ItemTypeAvailableAt
{
    [Key]
    [Column(Order = 1)]
    public Guid ItemTypeId { get; set; }

    [Key]
    [Column(Order = 2)]
    public Guid StoreId { get; set; }

    public float Price { get; set; }
    public Guid DefaultSectionId { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [ForeignKey("ItemTypeId")]
    public ItemType ItemType { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}