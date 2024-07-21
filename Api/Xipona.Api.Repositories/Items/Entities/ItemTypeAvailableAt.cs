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

    public decimal Price { get; set; }
    public Guid DefaultSectionId { get; set; }

    [ForeignKey("ItemTypeId")]
    public ItemType? ItemType { get; set; } = null;
}