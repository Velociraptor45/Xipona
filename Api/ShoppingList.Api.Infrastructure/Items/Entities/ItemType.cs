using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities;

public class ItemType
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public ItemType()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        AvailableAt ??= new List<ItemTypeAvailableAt>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public Guid ItemId { get; set; }
    public string Name { get; set; }
    public Guid? PredecessorId { get; set; }

    [ForeignKey("ItemId")]
    public Item Item { get; set; }

    [ForeignKey("PredecessorId")]
    public ItemType? Predecessor { get; set; }

    [InverseProperty("ItemType")]
    public ICollection<ItemTypeAvailableAt> AvailableAt { get; set; }
}