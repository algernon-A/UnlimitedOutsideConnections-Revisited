// <copyright file="Loading.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace EightyOne2
{
    using System.Collections.Generic;
    using AlgernonCommons;
    using ICities;

    /// <summary>
    /// Main loading class: the mod runs from here.
    /// Using LoadingBase instead of PatcherLoadingBase as we don't really care about the patch checks for this mod.
    /// </summary>
    public sealed class Loading : LoadingBase<OptionsPanel>
    {
        /// <summary>
        /// Gets a list of permitted loading modes.
        /// </summary>
        protected override List<AppMode> PermittedModes => new List<AppMode> { AppMode.Game, AppMode.MapEditor, AppMode.AssetEditor, AppMode.ScenarioEditor };
    }
}