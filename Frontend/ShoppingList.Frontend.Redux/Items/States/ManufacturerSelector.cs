using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;

namespace ShoppingList.Frontend.Redux.Items.States;
public record ManufacturerSelector(IReadOnlyCollection<ManufacturerSearchResult> Manufacturers, string Input);