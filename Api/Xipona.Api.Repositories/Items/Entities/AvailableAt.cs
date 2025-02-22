using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.Xipona.Api.Repositories.Items.Entities;

public class AvailableAt
{
    [Key]
    [Column(Order = 1)]
    public Guid ItemId { get; set; }

    [Key]
    [Column(Order = 2)]
    public Guid StoreId { get; set; }

    public decimal Price { get; set; }
    public Guid DefaultSectionId { get; set; }

    [ForeignKey("ItemId")]
    public Item? Item { get; set; } = null;
}