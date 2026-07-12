using Xipona.Api.Repositories.Items.Entities;

namespace Xipona.Api.Repositories.TestKit.Manufacturers.Entities;

public class ManufacturerEntityGodmother
{
    private Item? _item;

    public ManufacturerEntityGodmother For(Item item)
    {
        _item = item;
        return this;
    }

    public ManufacturerEntityBuilder GetFoundation()
    {
        if (_item?.ManufacturerId is null)
            return new ManufacturerEntityBuilder();
        
        return new ManufacturerEntityBuilder()
            .WithId(_item.ManufacturerId.Value);
    }
}