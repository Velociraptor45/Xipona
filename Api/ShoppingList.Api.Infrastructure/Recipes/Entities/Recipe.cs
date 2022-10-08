using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities;

public class Recipe
{
    public Recipe()
    {
        Name = string.Empty;
        Ingredients = new List<Ingredient>();
        PreparationSteps = new List<PreparationStep>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [InverseProperty("Recipe")]
    public ICollection<Ingredient> Ingredients { get; set; }

    [InverseProperty("Recipe")]
    public ICollection<PreparationStep> PreparationSteps { get; set; }
}