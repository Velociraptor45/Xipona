namespace Xipona.Frontend.Redux.Items.States.Merges;
public record ItemMerge(MergeItemSelector Selector, MergedItem? Item, bool IsSaving,
    ItemMergeValidationResult ValidationResult);