using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Entities;

public class ShoppingList
{
    public ShoppingList()
    {
        ItemsOnList ??= new List<ItemsOnList>();
        RowVersion ??= Array.Empty<byte>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public DateTimeOffset? CompletionDate { get; set; }
    public Guid StoreId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    [InverseProperty("ShoppingList")]
    public ICollection<ItemsOnList> ItemsOnList { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }
}