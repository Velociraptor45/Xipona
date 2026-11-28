namespace Xipona.Api.Contracts.Items.Commands.MergeItems
{
    /// <summary>
    /// Represents a contract for merging multiple items into one with many types.
    /// </summary>
    public class MergeItemsContract
    {
        /// <summary>
        /// </summary>
        /// <param name="item"></param>
        public MergeItemsContract(MergedItemContract item)
        {
            Item = item;
        }

        /// <summary>
        /// The item that results from merging multiple items.
        /// </summary>
        public MergedItemContract Item { get; set; }
    }
}
