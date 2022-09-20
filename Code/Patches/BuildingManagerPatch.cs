﻿// <copyright file="BuildingManagerPatch.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace UOCRevisited.Patches
{
    using HarmonyLib;
    /// <summary>
    /// Harmony Postfix patch for BuildingManager.CalculateOutsideConnectionCount.
    /// </summary>
    [HarmonyPatch(typeof(BuildingManager))]
    [HarmonyPatch("CalculateOutsideConnectionCount")]
    public static class CalculateOutsideConnectionCountPatch
    {
        /// <summary>
		/// Harmony Postfix patch for BuildingManager.CalculateOutsideConnectionCount to make sure 'incoming' and 'outgoing' args are capped at 3.
        /// These arguments represent the total number of incoming and outgoing (respectively) connections on the map.
        /// The game includes multiple checks to make sure these aren't greater than three (= max of four connections, zero-based);
        /// capping these return fools the game into thinking we don't have any more connections than the game allows.
        /// </summary>
        public static void Postfix(ItemClass.Service service, ItemClass.SubService subService, ref int incoming, ref int outgoing)
        {
            // Cap incoming and outgoing out args at 3.
            if (incoming > 3)
            {
                incoming = 3;
            }
            if (outgoing > 3)
            {
                outgoing = 3;
            }
        }
    }
}