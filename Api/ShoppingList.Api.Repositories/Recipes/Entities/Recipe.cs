using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities;

public class Recipe
{
    public Recipe()
    {
        Name ??= string.Empty;
        RowVersion ??= Array.Empty<byte>();

        Ingredients ??= new List<Ingredient>();
        PreparationSteps ??= new List<PreparationStep>();
        Tags ??= new List<TagsForRecipe>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public int NumberOfServings { get; set; }

    [Required]
    public string Name { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }

    [InverseProperty("Recipe")]
    public ICollection<Ingredient> Ingredients { get; set; }

    [InverseProperty("Recipe")]
    public ICollection<PreparationStep> PreparationSteps { get; set; }

    [InverseProperty("Recipe")]
    public ICollection<TagsForRecipe> Tags { get; set; }
}