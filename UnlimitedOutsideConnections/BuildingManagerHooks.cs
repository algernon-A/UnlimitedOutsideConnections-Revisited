﻿using ColossalFramework;
using ColossalFramework.Math;
using UnlimitedOutsideConnections.Detours;

namespace UnlimitedOutsideConnections
{
    public static class BuildingManagerHooks
    {
        public static void Deploy()
        {
            Revert();
            BuildingManager.instance.EventBuildingCreated += OnBuildingCreated;
            BuildingManager.instance.EventBuildingReleased += OnBuildingReleased;
        }

        public static void Revert()
        {
            BuildingManager.instance.EventBuildingCreated -= OnBuildingCreated;
            BuildingManager.instance.EventBuildingReleased -= OnBuildingReleased;
        }

        public static void OnBuildingCreated(ushort buildingID)
        {
#if DEBUG
            Debug.Log($"UnlimitedOutsideConnections - OnBuildingCreated. buildingID={buildingID}");
#endif
            var instance = BuildingManager.instance;
            var serviceBuildings = BuildingUtil.FindServiceBuildings(buildingID);
            foreach (var id in serviceBuildings)
            {
                var ai = instance.m_buildings.m_buffer[id].Info.GetAI() as TransportStationAI;
                if (ai?.m_transportLineInfo == null)
                {
                    continue;
                }
                var gateIndex = 0;
                if (ai.m_spawnPoints != null && ai.m_spawnPoints.Length != 0)
                {
                    var randomizer = new Randomizer(id);
                    gateIndex = randomizer.Int32((uint)ai.m_spawnPoints.Length);
                }
                instance.m_buildings.m_buffer[buildingID].m_flags |= Building.Flags.IncomingOutgoing;
                TransportStationAIDetour.CreateConnectionLines(ai, id, ref instance.m_buildings.m_buffer[id], buildingID, ref instance.m_buildings.m_buffer[buildingID], gateIndex);
                BuildingUtil.ReleaseOwnVehicles(id);
            }
        }

        public static void OnBuildingReleased(ushort buildingID)
        {
#if DEBUG
            Debug.Log($"UnlimitedOutsideConnections - OnBuildingReleased. buildingID={buildingID}");
#endif
            var data = BuildingManager.instance.m_buildings.m_buffer[buildingID];
            var buildingInfo = data.Info;
            var connectionAi = buildingInfo?.m_buildingAI as OutsideConnectionAI;
            if (connectionAi != null)
            {
                BuildingUtil.ReleaseOwnVehicles(buildingID);
            }
            var serviceBuildings = BuildingUtil.FindServiceBuildings(buildingID);
            foreach (var id in serviceBuildings)
            {
                var ai = Singleton<BuildingManager>.instance.m_buildings.m_buffer[id].Info.GetAI() as TransportStationAI;
                if (ai == null)
                {
                    continue;
                }
                BuildingUtil.ReleaseOwnVehicles(id);
            }
        }
    }
}