// <copyright file="GetInfoPatches.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace UOCRevisited.Patches
{
    using System.Collections.Generic;
    using System.Reflection;
    using HarmonyLib;

    /// <summary>
    /// Harmony patch to remove 'too many outside connections' errors.
    /// </summary>
    [HarmonyPatch]
    internal class GetInfoPatches
    {
        /// <summary>
        /// Determines list of target methods to patch - in this case, key NetAI GetInfo methods.
        /// </summary>
        /// <returns>List of target methods to patch.</returns>
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(RoadAI), nameof(RoadAI.GetInfo));
            yield return AccessTools.Method(typeof(TrainTrackAI), nameof(TrainTrackAI.GetInfo));
            yield return AccessTools.Method(typeof(ShipPathAI), nameof(ShipPathAI.GetInfo));
            yield return AccessTools.Method(typeof(FlightPathAI), nameof(FlightPathAI.GetInfo));
        }

        /// <summary>
        /// Harmony transpiler for BetAI GetInfo methods to remove 'too many outside connections' errors.
        /// Nothing too fancy here - we could drop the entire conditional statement, but for simplicity and reliability we just set the error mask to 0.
        /// </summary>
        /// <param name="instructions">Original ILCode.</param>
        /// <returns>Modified ILCode.</returns>
        internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            IEnumerator<CodeInstruction> instructionsEnumerator = instructions.GetEnumerator();
            while (instructionsEnumerator.MoveNext())
            {
                CodeInstruction instruction = instructionsEnumerator.Current;

                if (instruction.LoadsConstant(ToolBase.ToolErrors.TooManyConnections))
                {
                    instruction.operand = 0;
                }

                yield return instruction;
            }
        }
    }
}
