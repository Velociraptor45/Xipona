using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities;

public class Ingredient
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public Guid RecipeId { get; set; }
    public Guid ItemCategoryId { get; set; }
    public int QuantityType { get; set; }
    public float Quantity { get; set; }
    public Guid? DefaultItemId { get; set; }
    public Guid? DefaultItemTypeId { get; set; }

    [ForeignKey("RecipeId")]
    public Recipe? Recipe { get; set; }
}