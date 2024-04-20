using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.Xipona.Api.Repositories.Recipes.Entities;

public class TagsForRecipe
{
    [Key]
    [Column(Order = 1)]
    public Guid RecipeId { get; set; }

    [Key]
    [Column(Order = 2)]
    public Guid RecipeTagId { get; set; }

    [ForeignKey("RecipeId")]
    public Recipe? Recipe { get; set; }
}