using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Entities;

public class ItemCategory
{
    public ItemCategory()
    {
        Name = string.Empty;
        RowVersion = Array.Empty<byte>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    public bool Deleted { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }
}