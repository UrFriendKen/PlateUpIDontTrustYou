using Kitchen;
using Kitchen.Layouts;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace KitchenIDontTrustYou
{
    internal class UnstuckPlayers : GenericSystemBase, IModSystem
    {
        EntityQuery Players;

        protected override void Initialise()
        {
            base.Initialise();
            Players = GetEntityQuery(typeof(CPlayer), typeof(CPosition));
        }

        protected override void OnUpdate()
        {
            if (!Main.PrefManager.Get<bool>(Main.PREVENT_NO_CLIP_ID))
                return;

            using (NativeArray<Entity> entities = Players.ToEntityArray(Allocator.Temp))
            {
                using (NativeArray<CPosition> positions = Players.ToComponentDataArray<CPosition>(Allocator.Temp))
                {
                    for (int i = 0; i < entities.Length; i++)
                    {
                        Entity entity = entities[i];
                        CPosition position = positions[i];

                        CLayoutRoomTile tile = GetTile(position);
                        if (!LayoutHelpers.IsInside(tile.Type))
                            continue;
                        int playerRoomID = tile.RoomID;

                        Entity occupant = GetOccupant(position);
                        if (occupant == default ||
                            !Require(occupant, out CAppliance appliance) ||
                            !Helpers.IsFullColliderAppliance(appliance.ID))
                            continue;

                        Main.LogInfo($"{entity} is stuck");

                        bool isValidTileFound = false;
                        float closestSqrDist = float.MaxValue;
                        Vector3 closestTilePos = Vector3.zero;
                        foreach (Vector3 offset in LayoutHelpers.AllNearbyRange2)
                        {
                            Vector3 candidatePos = position + offset;
                            if (GetTile(candidatePos).RoomID != playerRoomID)
                                continue;

                            Entity offsetOccupant = GetOccupant(candidatePos);
                            if (offsetOccupant != default &&
                                Require(offsetOccupant, out appliance) &&
                                Helpers.IsFullColliderAppliance(appliance.ID))
                                continue;

                            isValidTileFound = true;

                            float sqrDist = (candidatePos - position).sqrMagnitude;
                            if (sqrDist < closestSqrDist)
                            {
                                closestSqrDist = sqrDist;
                                closestTilePos = candidatePos;
                            }
                        }

                        if (isValidTileFound)
                        {
                            position.ForceSnap = true;
                            position.Position = closestTilePos;
                            Set(entity, position);
                        }
                    }
                }
            }
        }
    }
}
