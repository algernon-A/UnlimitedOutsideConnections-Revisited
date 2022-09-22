// <copyright file="OptionsPanel.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace EightyOne2
{
    using AlgernonCommons.UI;
    using ColossalFramework.UI;

    /// <summary>
    /// The mod's options panel.
    /// </summary>
    public class OptionsPanel : UIPanel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsPanel"/> class.
        /// </summary>
        public OptionsPanel()
        {
            UILabels.AddLabel(this, 5f, 5f, "Unlimited Outside Connections Revisited has been enabled");
            UILabels.AddLabel(this, 5f, 25f, "Current loaded status is: " + (Loading.IsLoaded ? "loaded" : "unloaded (not in game)"));
        }
    }
}