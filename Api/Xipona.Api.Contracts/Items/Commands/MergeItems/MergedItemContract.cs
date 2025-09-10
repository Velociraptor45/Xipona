using System.Collections.Generic;

namespace Xipona.Api.Contracts.Items.Commands.MergeItems
{
    /// <summary>
    /// Represents a contract for an item with types that results from merging multiple items.
    /// </summary>
    public class MergedItemContract
    {
        /// <summary> 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="types"></param>
        public MergedItemContract(string name, IEnumerable<MergedItemTypeContract> types)
        {
            Name = name;
            Types = types;
        }

        /// <summary>
        /// The name of the new item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The types of the new item, originating from the merged items.
        /// </summary>
        public IEnumerable<MergedItemTypeContract> Types { get; set; }
    }
}
