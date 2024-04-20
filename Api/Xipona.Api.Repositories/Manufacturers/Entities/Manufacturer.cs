using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.Xipona.Api.Repositories.Manufacturers.Entities;

public class Manufacturer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public bool Deleted { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}