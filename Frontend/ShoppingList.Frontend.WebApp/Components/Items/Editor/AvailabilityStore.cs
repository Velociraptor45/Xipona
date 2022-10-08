using System;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Components.Items.Editor;

public class AvailabilityStore
{
    public AvailabilityStore(Guid id, string name, bool isDisabled)
    {
        Id = id;
        Name = name;
        IsDisabled = isDisabled;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsDisabled { get; set; }
}