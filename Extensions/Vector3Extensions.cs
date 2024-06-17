using UnityEngine;

namespace KitchenIDontTrustYou.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 MulComponents(this Vector3 v1, Vector3 v2)
        {
            v1.x *= v2.x;
            v1.y *= v2.y;
            v1.z *= v2.z;
            return v1;
        }
    }
}
