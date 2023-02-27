// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryController.cs" company="UCA Clermont-Ferrand">
//     Copyright (c) UCA Clermont-Ferrand All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Model.Inventory
{
    /// <summary>
    /// The inventory model.
    /// </summary>
    public class InventoryModel
    {
        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets the number item.
        /// </summary>
        public int NumberItem { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public int Position { get; set; }
    }
}