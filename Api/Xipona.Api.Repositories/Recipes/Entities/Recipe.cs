using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.Xipona.Api.Repositories.Recipes.Entities;

public class Recipe
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public int NumberOfServings { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public Guid? SideDishId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Timestamp]
    public byte[] RowVersion { get; set; } = [];

    [InverseProperty("Recipe")]
    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    [InverseProperty("Recipe")]
    public ICollection<PreparationStep> PreparationSteps { get; set; } = new List<PreparationStep>();

    [InverseProperty("Recipe")]
    public ICollection<TagsForRecipe> Tags { get; set; } = new List<TagsForRecipe>();

    [ForeignKey("SideDishId")]
    public Recipe? SideDish { get; set; }
}