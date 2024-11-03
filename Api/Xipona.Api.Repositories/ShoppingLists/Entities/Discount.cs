using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;

[Index("ItemId")]
[Index("ItemTypeId")]
public class Discount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    [Required]
    public decimal DiscountPrice { get; set; }

    [Required]
    public Guid ItemId { get; set; }

    [Required]
    public Guid? ItemTypeId { get; set; }

    [Required]
    public Guid ShoppingListId { get; set; }

    [ForeignKey("ShoppingListId")]
    public ShoppingList? ShoppingList { get; set; }
}