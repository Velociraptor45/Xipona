using Xipona.Api.Repositories.Items.Entities;

namespace Xipona.Api.Repositories.TestKit.ItemCategories.Entities;

public class ItemCategoryEntityGodmother
{
    private Item? _item;

    public ItemCategoryEntityGodmother For(Item item)
    {
        _item = item;
        return this;
    }

    public ItemCategoryEntityBuilder GetBasics()
    {
        if (_item?.ItemCategoryId is null)
            return new ItemCategoryEntityBuilder();
        
        return new ItemCategoryEntityBuilder()
            .WithId(_item.ItemCategoryId.Value);
    }
}