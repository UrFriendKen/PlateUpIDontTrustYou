using Controllers;
using Kitchen;
using KitchenData;
using KitchenIDontTrustYou.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenIDontTrustYou
{
    internal static class Helpers
    {
        private static HashSet<int> FullColliderAppliances = new HashSet<int>();

        public static bool IsHost(CPlayer player)
        {
            return player.InputSource == InputSourceIdentifier.Identifier;
        }

        public static void InitFullColliderAppliances()
        {
            FullColliderAppliances.Clear();

            foreach (Appliance appliance in GameData.Main.Get<Appliance>())
            {
                if (appliance.Prefab == null)
                    continue;

                Collider[] colliders = appliance.Prefab.GetComponentsInChildren<Collider>();
                if (colliders.Length != 1)
                    continue;

                Vector3 colliderPos = Vector3.zero;
                Quaternion colliderRotation = Quaternion.identity;
                Vector3 colliderScale = Vector3.one;

                if (colliders.Length > 0)
                {
                    Collider collider = colliders[0];
                    if (collider is BoxCollider boxCollider)
                    {
                        colliderPos += boxCollider.center;
                        colliderScale = colliderScale.MulComponents(boxCollider.size);
                    }

                    Transform root = collider.transform.root;
                    Transform parentTransform = collider.transform;
                    while (parentTransform != null && parentTransform != root)
                    {
                        colliderPos = parentTransform.localPosition + colliderPos.MulComponents(parentTransform.localScale);
                        colliderRotation *= parentTransform.localRotation;
                        colliderScale = colliderScale.MulComponents(parentTransform.localScale);
                        parentTransform = parentTransform.parent;
                    }
                }
                else
                {
                    colliderScale = Vector3.zero;
                }

                if (colliderPos.x != 0f ||
                    colliderPos.z != 0f ||
                    colliderScale.x != 1f ||
                    colliderScale.z != 1f)
                    continue;

                FullColliderAppliances.Add(appliance.ID);
            }
        }

        public static bool IsFullColliderAppliance(int applianceID)
        {
            return FullColliderAppliances?.Contains(applianceID) ?? false;
        }
    }
}
