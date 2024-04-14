using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;

public class ItemsOnList
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Guid ShoppingListId { get; set; }
    public Guid ItemId { get; set; }
    public Guid? ItemTypeId { get; set; }
    public bool InBasket { get; set; }
    public float Quantity { get; set; }
    public Guid SectionId { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [ForeignKey("ShoppingListId")]
    public ShoppingList ShoppingList { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}