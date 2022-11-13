using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;

public class Item
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public Item()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        AvailableAt ??= new List<AvailableAt>();
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

    [ForeignKey("PredecessorId")]
    public Item? Predecessor { get; set; }

    [InverseProperty("Item")]
    public ICollection<ItemType> ItemTypes { get; set; }

    [InverseProperty("Item")]
    public ICollection<AvailableAt> AvailableAt { get; set; }
}