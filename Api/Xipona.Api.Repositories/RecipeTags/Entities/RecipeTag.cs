using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.Xipona.Api.Repositories.RecipeTags.Entities;

public class RecipeTag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}