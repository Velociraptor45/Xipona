using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;

public class ShoppingListDiscount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    [Required]
    public Guid ShoppingListId { get; set; }

    public decimal? DiscountPrice { get; set; }
    public decimal? DiscountPercentage { get; set; }

    [ForeignKey("ShoppingListId")]
    public ShoppingList? ShoppingList { get; set; }

}
