using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.Xipona.Api.Repositories.Items.Entities;

public class ItemType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public Guid ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? PredecessorId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    [ForeignKey("ItemId")]
    public Item? Item { get; set; }

    [ForeignKey("PredecessorId")]
    public ItemType? Predecessor { get; set; }

    [InverseProperty("ItemType")]
    public ICollection<ItemTypeAvailableAt> AvailableAt { get; set; } = new List<ItemTypeAvailableAt>();
}