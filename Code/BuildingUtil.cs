// <copyright file="BuildingUtil.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard), BloodyPenguin (Egor Aralov). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace UOCRevisited
{
    using System.Collections.Generic;
    using ColossalFramework;

    /// <summary>
    /// Building utilities.
    /// </summary>
    internal static class BuildingUtil
    {
        /// <summary>
        /// Returns a list of all service buildings matching the specified outside connection.
        /// </summary>
        /// <param name="connectionBuildingID">Outside connection building ID.</param>
        /// <returns>New list of service buildings (empty list if none).</returns>
        internal static IEnumerable<ushort> FindServiceBuildings(uint connectionBuildingID)
        {
            // Return list.
            List<ushort> buildingList = new List<ushort>();

            // Need valid building ID.
            if (connectionBuildingID == 0)
            {
                // Invalid building ID - return empty list.
                return buildingList;
            }

            // Local references.
            BuildingManager buildingManager = Singleton<BuildingManager>.instance;
            Building[] buildingBuffer = buildingManager.m_buildings.m_buffer;
            BuildingInfo buildingInfo = buildingBuffer[connectionBuildingID].Info;
            OutsideConnectionAI connectionAI = buildingInfo?.GetAI() as OutsideConnectionAI;

            // Make sure this is an outside connection building with the PublicTransport service.
            if (connectionAI == null || buildingInfo.GetService() != ItemClass.Service.PublicTransport)
            {
                // Not a valid connection - return empty list.
                return buildingList;
            }

            // Get all current public transport service buildings.
            FastList<ushort> serviceBuildings = buildingManager.GetServiceBuildings(ItemClass.Service.PublicTransport);
            if (serviceBuildings == null || serviceBuildings.m_size == 0)
            {
                // No public transport buildings - return empty list.
                return buildingList;
            }

            // Iterate through each public transport building, looking for a subservice match with this one.
            ItemClass.Service service = buildingInfo.GetService();
            ItemClass.SubService subService = buildingInfo.GetSubService();
            foreach (ushort buildingID in serviceBuildings)
            {
                BuildingInfo serviceInfo = buildingBuffer[buildingID].Info;

                // Note that intercity bus routes need to match to roads (no direct subservice match).
                if (buildingBuffer[buildingID].m_flags != Building.Flags.None
                    && serviceInfo != null
                    && ((service == ItemClass.Service.Road && serviceInfo.GetSubService() == ItemClass.SubService.PublicTransportBus) || serviceInfo.GetSubService() == subService))
                {
                    buildingList.Add(buildingID);
                }
            }

            return buildingList;
        }

        /// <summary>
        /// Releases any vehicles travelling to/from the given building.
        /// </summary>
        /// <param name="buildingID">Bulding ID.</param>
        internal static void ReleaseTargetedVehicles(ushort buildingID)
        {
            // Valid building check.
            if (buildingID == 0)
            {
                return;
            }

            // Check for valud building AI.
            BuildingAI buildingAI = BuildingManager.instance.m_buildings.m_buffer[buildingID].Info?.m_buildingAI;
            if (buildingAI == null)
            {
                return;
            }

            // Local references.
            VehicleManager vehicleManager = Singleton<VehicleManager>.instance;
            Vehicle[] vehicles = vehicleManager.m_vehicles.m_buffer;

            // Iterate through all vehicles and remove any related to this building.
            for (ushort i = 1; i < vehicles.Length; ++i)
            {
                if (vehicles[i].m_sourceBuilding == buildingID || vehicles[i].m_targetBuilding == buildingID)
                {
                    vehicleManager.ReleaseVehicle(i);
                }
            }
        }

        /// <summary>
        /// Clears the target of any vehicles owned by the given building.
        /// This forces a recalculation (and possible despawning) of these vehicles.
        /// Done this way (instead of straight release) to properly handle dummy traffic.
        /// </summary>
        /// <param name="buildingID">Bulding ID.</param>
        internal static void ReleaseOwnVehicles(ushort buildingID)
        {
            // Valid building check.
            if (buildingID == 0)
            {
                return;
            }


            // Local references.
            BuildingManager buildingManager = Singleton<BuildingManager>.instance;
            Building[] buildings = buildingManager.m_buildings.m_buffer;

            // Check for valud building AI.
            BuildingInfo buildingInfo = BuildingManager.instance.m_buildings.m_buffer[buildingID].Info;
            BuildingAI buildingAI = buildingInfo?.m_buildingAI;
            if (buildingAI == null)
            {
                return;
            }

            // Local references.
            VehicleManager vehicleManager = Singleton<VehicleManager>.instance;
            Vehicle[] vehicles = vehicleManager.m_vehicles.m_buffer;

            // Iterate through each vehicle in building's linked list.
            ushort vehicleID = buildings[buildingID].m_ownVehicles;
            while (vehicleID != 0)
            {
                if (vehicles[vehicleID].m_transportLine == 0)
                {
                    // Clear the target for any vehicles owned by this building.
                    VehicleInfo vehicleInfo = vehicles[vehicleID].Info;
                    if (vehicleInfo.m_class.m_service == buildingInfo.m_class.m_service && vehicleInfo.m_class.m_subService == buildingInfo.m_class.m_subService)
                    {
                        vehicleInfo.m_vehicleAI.SetTarget(vehicleID, ref vehicles[vehicleID], 0);
                    }
                }

                vehicleID = vehicles[vehicleID].m_nextOwnVehicle;
            }
        }
    }
}