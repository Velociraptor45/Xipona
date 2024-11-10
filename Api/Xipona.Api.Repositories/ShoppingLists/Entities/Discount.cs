using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;

[Index("ItemId")]
[Index("ItemTypeId")]
public class Discount
{
    [Key]
    [Column(Order = 1)]
    public Guid ShoppingListId { get; set; }

    [Key]
    [Column(Order = 2)]
    public Guid ItemId { get; set; }

    [Key]
    [Column(Order = 3)]
    public Guid? ItemTypeId { get; set; }

    [Required]
    public decimal DiscountPrice { get; set; }

    [ForeignKey("ShoppingListId")]
    public ShoppingList? ShoppingList { get; set; }
}