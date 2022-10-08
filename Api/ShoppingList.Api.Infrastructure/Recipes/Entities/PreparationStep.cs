using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities;

public class PreparationStep
{
    public PreparationStep()
    {
        Instruction = string.Empty;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public Guid RecipeId { get; set; }

    [Required]
    public string Instruction { get; set; }

    public int SortingIndex { get; set; }

    [ForeignKey("RecipeId")]
    public Recipe? Recipe { get; set; }
}