using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities;

public class Store
{
    public Store()
    {
        Sections ??= new List<Section>();
        Name ??= string.Empty;
        RowVersion ??= Array.Empty<byte>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    public bool Deleted { get; set; }

    [InverseProperty("Store")]
    public ICollection<Section> Sections { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }
}