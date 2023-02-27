// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryController.cs" company="UCA Clermont-Ferrand">
//     Copyright (c) UCA Clermont-Ferrand All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Model
{
    /// <summary>
    /// The item.
    /// </summary>
    public partial class Item: IEquatable<Item>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        public Item()
        {
            EnchantCategories = new List<string>();
            RepairWith = new List<string>();
        }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the enchant categories.
        /// </summary>
        public List<string> EnchantCategories { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the image base64.
        /// </summary>
        public string ImageBase64 { get; set; }

        /// <summary>
        /// Gets or sets the maximum durability.
        /// </summary>
        public int MaxDurability { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the repair with.
        /// </summary>
        public List<string> RepairWith { get; set; }

        /// <summary>
        /// Gets or sets the size of the stack.
        /// </summary>
        public int StackSize { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Compare an item to this one.
        /// </summary>
        /// <param name="other">The item to compare.</param>
        /// <returns>True if the item is equal, false otherwise.</returns>
        public bool Equals(Item? other)
            => other != null 
               && (other.Id == 0 || Id == other.Id ? FullComparer.Equals(this, other) : Id == other.Id);

        /// <summary>
        /// Compare an object to this one.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True if the object is equal, false otherwise.</returns>
        public override bool Equals(object? obj) 
            => obj is Item item && Equals(item);
        }
}