using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;

public class ShoppingList
{
    public ShoppingList()
    {
        ItemsOnList ??= new List<ItemsOnList>();
        Discounts ??= new List<Discount>();
        ListDiscounts ??= new List<ShoppingListDiscount>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public DateTimeOffset? CompletionDate { get; set; }
    public Guid StoreId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    [InverseProperty("ShoppingList")]
    public ICollection<ItemsOnList> ItemsOnList { get; set; }

    [InverseProperty("ShoppingList")]
    public ICollection<Discount> Discounts { get; set; }

    [InverseProperty("ShoppingList")]
    public ICollection<ShoppingListDiscount> ListDiscounts { get; set; }

    [Timestamp]
    public uint RowVersion { get; set; }
}