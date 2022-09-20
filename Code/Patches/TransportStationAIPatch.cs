// <copyright file="TransportStationAIPatch.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace UOCRevisited.Patches
{
    using System;
    using System.Runtime.CompilerServices;
    using AlgernonCommons;
    using HarmonyLib;

    /// <summary>
    /// Harmony reverse patch to access TransportStationAI.CreateConnectionLines (private method).
    /// </summary>
    [HarmonyPatch]
    internal class TransportStationAIPatch
    {
        /// <summary>
        /// Harmony reverse patch to access TransportStationAI.CreateConnectionLines (private method).
        /// </summary>
        /// <param name="ai">Builign AI instance.</param>
        /// <param name="buildingID">Buildng ID.</param>
        /// <param name="data">Building data.</param>
        /// <exception cref="NotImplementedException">Harmony reverse patch wasn't applied.</exception>
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(TransportStationAI), "CreateConnectionLines")]
        [HarmonyPatch(new Type[] { typeof(ushort), typeof(Building) },
            new ArgumentType[] { ArgumentType.Normal, ArgumentType.Ref })]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void CreateConnectionLines(TransportStationAI ai, ushort buildingID, ref Building data)
        {
            string message = "TransportStationAI.CreateConnectionLines reverse Harmony patch wasn't applied";
            Logging.Error(message, ai.ToString(), buildingID.ToString(), data.ToString());
            throw new NotImplementedException(message);
        }
    }
}