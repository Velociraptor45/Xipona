using System;

namespace Xipona.Api.Contracts.Items.Commands.MergeItems
{
    /// <summary>
    /// Represents a contract for an item type that results from merging multiple items.
    /// </summary>
    public class MergedItemTypeContract
    {
        /// <summary> 
        /// </summary>
        /// <param name="originatingItemId"></param>
        /// <param name="name"></param>
        public MergedItemTypeContract(Guid originatingItemId, string name)
        {
            OriginatingItemId = originatingItemId;
            Name = name;
        }

        /// <summary>
        /// The ID of the item from which this type originates.
        /// </summary>
        public Guid OriginatingItemId { get; set; }

        /// <summary>
        /// The name of the new item type.
        /// </summary>
        public string Name { get; set; }
    }
}
