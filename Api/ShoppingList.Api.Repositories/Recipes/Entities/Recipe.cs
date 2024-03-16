using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities;

public class Recipe
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public int NumberOfServings { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    [InverseProperty("Recipe")]
    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    [InverseProperty("Recipe")]
    public ICollection<PreparationStep> PreparationSteps { get; set; } = new List<PreparationStep>();

    [InverseProperty("Recipe")]
    public ICollection<TagsForRecipe> Tags { get; set; } = new List<TagsForRecipe>();
}