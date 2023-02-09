using Controllers;
using Kitchen;
using Unity.Entities;

namespace KitchenIDontTrustYou
{
    public static class Helpers
    {
        public static bool IsHost(CPlayer player)
        {
            return player.InputSource == InputSourceIdentifier.Identifier;
        }
    }
}
