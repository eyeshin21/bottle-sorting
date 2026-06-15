using UnityEngine;

namespace Anvil
{
    public static partial class ExtensionMethod
    {
        public static bool GetPrefabSafe(this IPoolTable poolTable, string address,out GameObject prefab)
        {
            if (poolTable == null)
            {
                prefab = null;
                return false;
            }

            return poolTable.GetPrefab(address,out prefab);
        }

    }
}
