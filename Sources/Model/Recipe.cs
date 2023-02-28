﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryController.cs" company="UCA Clermont-Ferrand">
//     Copyright (c) UCA Clermont-Ferrand All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Model
{
    /// <summary>
    /// The recipe.
    /// </summary>
    public class Recipe
    {
        /// <summary>
        /// Gets or sets the give.
        /// </summary>
        public Item.Item Give { get; set; }

        /// <summary>
        /// Gets or sets the have.
        /// </summary>
        public List<List<string>> Have { get; set; }
    }
}