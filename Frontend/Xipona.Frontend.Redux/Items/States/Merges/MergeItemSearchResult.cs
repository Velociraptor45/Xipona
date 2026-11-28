namespace Xipona.Frontend.Redux.Items.States.Merges;

public record MergeItemSearchResult(Guid Id, string Name, Guid ItemCategory, Guid? Manufacturer, int QuantityType,
    int? QuantityTypeInPacket);